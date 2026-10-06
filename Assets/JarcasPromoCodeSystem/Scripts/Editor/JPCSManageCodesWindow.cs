//-----------------------------------------------------
// Jarcas Promo Code System
// Copyright 2014 Jarcas Studios
//-----------------------------------------------------
using UnityEditor;
using UnityEngine;
using System.Collections;
using LitJson;

public class JPCSManageCodesWindow : EditorWindow {

	private enum ServerCallEnum {
		Generate,
		List,
		Delete
	}


	public class JPCSCode {
		public int id;
		public string product_id;
		public string code;
		public int num_uses;
		public bool restorable;
		public string notes;
	}


	[ MenuItem( "Window/Jarcas Promo Code System/Manage Promo Codes", false, 20 ) ]
	public static void ShowWindow( ) {
		// First make sure Settings have been configured
		if ( EditorPrefs.HasKey( "jpcsPassword" ) ) {
			EditorWindow.GetWindow( typeof( JPCSManageCodesWindow ), false, "Promo Codes" );
		} else {
			if ( EditorUtility.DisplayDialog( "Setup Required", "You need to configure the Jarcas Promo Code System settings before generating or managing any promo codes", "OK" ) ) {
				JPCSSettingsWindow.ShowWindow( );
			}
		}
	}

	private const float COL_MIN_WIDTH = 100.0f;
	private const float COL_MAX_WIDTH = 160.0f;
	private const float COL3_WIDTH = 40.0f;

	private string productID = "product_id";
	private string notes = "Additional notes here";
	private bool restorable = true;
	private bool generateUCLetters = true;
	private bool generateLCLetters = false;
	private bool generateNumbers = true;
	private bool removeLookalikes = true;
	private int codeLength = 12;
	private int dashFrequency = 4;
	private int numUses = 1;
	private int numCodes = 1;

	private Vector2 scrollPosition = Vector2.zero;

	private float timeStartedWaiting;
	private bool waitingOnWWW = false;
	private ServerCallEnum serverCall;
	private WWW www;

	private JPCSCode[] codesToList;


#region GUI_STUFF
	private void OnGUI( ) {
		// Fields for Generate section
		EditorGUILayout.LabelField( "Generate Promo Codes", EditorStyles.boldLabel );
		productID = EditorGUILayout.TextField( new GUIContent( "Product ID", "An identifier for use in-game to let the game know what purchase has been redeemed" ), productID );
		notes = EditorGUILayout.TextField( new GUIContent( "Notes", "Your personal notes on these codes - who/when they were given to, etc." ), notes );
		restorable = EditorGUILayout.Toggle( new GUIContent( "Restorable", "Restorable should be set to true unless your promo code will be providing a consumable purchase" ), restorable );
		generateUCLetters = EditorGUILayout.Toggle( new GUIContent( "Generate UC Letters", "Should the generated promo code include uppercase letters" ), generateUCLetters );
		generateLCLetters = EditorGUILayout.Toggle( new GUIContent( "Generate LC Letters", "Should the generated promo code include lowercase letters" ), generateLCLetters );
		generateNumbers = EditorGUILayout.Toggle( new GUIContent( "Generate Numbers", "Should the generated promo code include numbers" ), generateNumbers );
		removeLookalikes = EditorGUILayout.Toggle( new GUIContent( "Remove Lookalikes", "Don't use characters that may cause confusion because they look alike (specifically I, O, L, 0, and 1)" ), removeLookalikes );
		codeLength = EditorGUILayout.IntField( new GUIContent( "Code Length", "How many characters long (not counting dashes) should the generated code be" ), codeLength );
		dashFrequency = EditorGUILayout.IntField( new GUIContent( "Dash Frequency", "How many characters between each dash (0 for no dashes)" ), dashFrequency );
		numUses = EditorGUILayout.IntField( new GUIContent( "Uses Per Code", "How many different people(devices) can use each code" ), numUses );
		numCodes = EditorGUILayout.IntField( new GUIContent( "Num Codes", "How many codes you want to generate" ), numCodes );
		if ( GUILayout.Button( "Generate", GUILayout.Width( 100.0f ) ) ) {
			GenerateCodes( );
		}

		// Separator
		EditorGUILayout.Space( );
		GUILayout.Box("", GUILayout.ExpandWidth( true ), GUILayout.Height( 1 ) );
		EditorGUILayout.Space( );

		EditorGUILayout.LabelField( "Current Promo Codes", EditorStyles.boldLabel );
		if ( GUILayout.Button( "Refresh List", GUILayout.Width( 100.0f ) ) ) {
			ListCodes( );
		}

		// Display listed codes
		if ( codesToList != null ) {
			scrollPosition = EditorGUILayout.BeginScrollView( scrollPosition );

			// Column headers
			EditorGUILayout.BeginHorizontal( );
			EditorGUILayout.LabelField( "Product ID", EditorStyles.largeLabel, GUILayout.MinWidth( COL_MIN_WIDTH ), GUILayout.MaxWidth( COL_MAX_WIDTH ) );
			EditorGUILayout.LabelField( "Code", EditorStyles.largeLabel, GUILayout.MinWidth( COL_MIN_WIDTH ), GUILayout.MaxWidth( COL_MAX_WIDTH ) );
			EditorGUILayout.LabelField( "Uses", EditorStyles.largeLabel, GUILayout.Width( COL3_WIDTH ) );
			EditorGUILayout.LabelField( "Rstr", EditorStyles.largeLabel, GUILayout.Width( COL3_WIDTH ) );
			EditorGUILayout.LabelField( "Notes", EditorStyles.largeLabel, GUILayout.MinWidth( COL_MIN_WIDTH ) );
			GUILayout.Space( 60.0f );
			EditorGUILayout.EndHorizontal( );

			// List all the codes
			for ( int i = 0; i < codesToList.Length; i++ ) {
				EditorGUILayout.BeginHorizontal( );
				EditorGUILayout.LabelField( codesToList[ i ].product_id, GUILayout.MinWidth( COL_MIN_WIDTH ), GUILayout.MaxWidth( COL_MAX_WIDTH ) );
				EditorGUILayout.SelectableLabel( codesToList[ i ].code, GUILayout.Height( 20.0f ), GUILayout.MinWidth( COL_MIN_WIDTH ), GUILayout.MaxWidth( COL_MAX_WIDTH ) );
				EditorGUILayout.LabelField( codesToList[ i ].num_uses.ToString( ), GUILayout.Width( COL3_WIDTH ) );
				EditorGUILayout.LabelField( codesToList[ i ].restorable ? "Y" : "N", GUILayout.Width( COL3_WIDTH ) );
				EditorGUILayout.LabelField( codesToList[ i ].notes, GUILayout.MinWidth( COL_MIN_WIDTH ) );
				if ( GUILayout.Button( "Delete", GUILayout.Width( 60.0f ) ) ) {
					if ( EditorUtility.DisplayDialog( "Confirm Delete", "Are you sure you want to delete this promo code?", "Delete", "Cancel" ) ) {
						DeleteCode( codesToList[ i ].id );
					}
				}
				EditorGUILayout.EndHorizontal( );
			}

			EditorGUILayout.EndScrollView( );
		}
	}


	private void Update( ) {
		// Handle waiting on response from server
		if ( waitingOnWWW ) {
			if ( www.isDone ) {
				// Yay! Finished waiting!
				waitingOnWWW = false;
				EditorUtility.ClearProgressBar( );

				// Check for HTTP error
				if ( !string.IsNullOrEmpty( www.error ) ) {
					EditorUtility.DisplayDialog( "Jarcas Promo Code System Error", "HTTP Error - " + www.error, "OK" );
					return;
				}

				// Get response as a JSON object
				JsonData responseJson = JsonMapper.ToObject( www.text );

				// Check for server error encoded in JSON
				if ( responseJson.IsObject && responseJson.Keys.Contains( "error" ) ) {
					EditorUtility.DisplayDialog( "Jarcas Promo Code System Error", "Server Error - " + responseJson[ "error" ], "OK" );
					return;
				}

				// Handle the actual response from the server
				switch ( serverCall ) {
				case ServerCallEnum.Generate:
					HandleGenerateResponse( responseJson );
					break;
				case ServerCallEnum.List:
					HandleListResponse( www.text );
					break;
				case ServerCallEnum.Delete:
					HandleDeleteResponse( responseJson );
					break;
				}
			} else {
				// Still waiting, show progress bar
				EditorUtility.DisplayProgressBar( "Jarcas Promo Code System", "Communicating with server", ( ( float )EditorApplication.timeSinceStartup - timeStartedWaiting ) / 5.0f );
			}
		}
	}


	private void OnInspectorUpdate() {
		// Only do extra repaints while waiting on WWW so that progress bar refreshes properly
		if ( waitingOnWWW ) {
			Repaint( );
		}
	}
#endregion
	

#region BUTTON_METHODS
	private void GenerateCodes( ) {
		// Build SQL query parameters
		WWWForm form = new WWWForm( );
		form.AddField( "jpcs_password", EditorPrefs.GetString( "jpcsPassword" ) );
		form.AddField( "app_id", JPCSRuntimeSettings.instance.appID );
		form.AddField( "product_id", productID );
		form.AddField( "notes", notes );
		form.AddField( "generate_uc_letters", generateUCLetters ? 1 : 0 );
		form.AddField( "generate_lc_letters", generateLCLetters ? 1 : 0 );
		form.AddField( "generate_numbers", generateNumbers ? 1 : 0 );
		form.AddField( "remove_lookalikes", removeLookalikes ? 1 : 0 );
		form.AddField( "dash_frequency", dashFrequency );
		form.AddField( "code_length", codeLength );
		form.AddField( "uses_per_code", numUses );
		form.AddField( "restorable", restorable ? 1 : 0 );
		form.AddField( "num_codes", numCodes );

		serverCall = ServerCallEnum.Generate;
		MakeServerCall( "generate.php", form );
	}


	private void ListCodes( ) {
		// Build SQL query parameters
		WWWForm form = new WWWForm( );
		form.AddField( "jpcs_password", EditorPrefs.GetString( "jpcsPassword" ) );
		form.AddField( "app_id", JPCSRuntimeSettings.instance.appID );

		serverCall = ServerCallEnum.List;
		MakeServerCall( "list.php", form );
	}


	private void DeleteCode( int codeID ) {
		// Build SQL query parameters
		WWWForm form = new WWWForm( );
		form.AddField( "jpcs_password", EditorPrefs.GetString( "jpcsPassword" ) );
		form.AddField( "code_id", codeID );

		serverCall = ServerCallEnum.Delete;
		MakeServerCall( "delete.php", form );
	}


	private void MakeServerCall( string page, WWWForm form ) {
		waitingOnWWW = true;
		timeStartedWaiting = ( float )EditorApplication.timeSinceStartup;
		www = new WWW( JPCSRuntimeSettings.instance.serverURL.TrimEnd( '/' ) + "/" + page, form );
	}


	private void HandleGenerateResponse( JsonData responseJson ) {
		// Display the returned codes and copy them to the clipboard
		string allCodes = "";
		for ( int i = 0; i < responseJson.Count; i++ ) {
			allCodes += responseJson[ i ] + "\r\n";
		}
		
		EditorGUIUtility.systemCopyBuffer = allCodes;
		EditorUtility.DisplayDialog( "Codes successfully generated!", "The following codes are now all in the server database and have also been copied to your system clipboard for convenience\n\n" + allCodes, "OK" );
		ListCodes( );
	}
	
	
	private void HandleListResponse( string responseString ) {
		codesToList = JsonMapper.ToObject< JPCSCode[] >( responseString );
	}


	private void HandleDeleteResponse( JsonData responseJson ) {
		EditorUtility.DisplayDialog( "Code successfully deleted!", "Selected code has been removed from the server database", "OK" );
		ListCodes( );
	}

#endregion
}
