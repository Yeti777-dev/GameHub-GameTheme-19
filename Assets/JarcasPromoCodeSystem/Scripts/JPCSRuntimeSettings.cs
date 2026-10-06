//-----------------------------------------------------
// Jarcas Promo Code System
// Copyright 2014 Jarcas Studios
//-----------------------------------------------------
using UnityEngine;
using System.IO;
using System.Collections;

public class JPCSRuntimeSettings : ScriptableObject {

	private static JPCSRuntimeSettings _instance;
	public static JPCSRuntimeSettings instance {
		get { 
			if ( _instance == null ) {
				// Attempt to load the asset from disk
				_instance = Resources.Load< JPCSRuntimeSettings >( "JPCSRuntimeSettings" );

				// Asset does not exist on disk!
				if ( _instance == null ) {
					// If game is running, log an error
					if ( Application.isPlaying ) {
						Debug.LogError( "Jarcas Promo Code System not configured! Please go to Window->Jarcas Promo Code System->Settings in the Unity Editor" );
					}

					// Instantiate (will be saved to disk/asset by JPCSSettingsWindow)
					_instance = ScriptableObject.CreateInstance< JPCSRuntimeSettings >( );
				}
			}
			
			return _instance;
		}
	}


	// The actual data stored in the settings asset
	public string serverURL;
	public string appID;
	public string salt;
}