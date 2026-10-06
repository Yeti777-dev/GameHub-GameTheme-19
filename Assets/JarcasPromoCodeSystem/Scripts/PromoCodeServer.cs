//-----------------------------------------------------
// Jarcas Promo Code System
// Copyright 2014 Jarcas Studios
//-----------------------------------------------------
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using LitJson;


/// <summary>
/// Promo Code server
/// </summary>
public class PromoCodeServer : MonoBehaviour {


#region SINGLETON
	public static PromoCodeServer instance;

	protected virtual void Awake( ) {
		instance = this;
	}
#endregion

	/// <summary>
	/// Event is fired when a code is redeemed successfully. Will pass along the productID that was redeemed
	/// </summary>
	public event Action< string > OnCodeRedeemSuccess;

	/// <summary>
	/// Event is fired when a code redemption fails. Will pass along an error message
	/// </summary>
	public event Action< string > OnCodeRedeemFailure;


#region PUBLIC_METHODS
	/// <summary>
	/// Attempts to redeem a promo code for this device
	/// </summary>
	/// <param name='code'>
	/// The promo code.
	/// </param>
	public void RedeemPromoCode( string code ) {
		StartCoroutine( RedeemPromoCodeCoroutine( code ) );
	}
#endregion

	
#region PRIVATE_METHODS
	private IEnumerator RedeemPromoCodeCoroutine( string code ) {
		// Build SQL query parameters
		WWWForm form = new WWWForm( );
		form.AddField( "app_id", JPCSRuntimeSettings.instance.appID );
		form.AddField( "code", code );
		form.AddField( "device_id", SystemInfo.deviceUniqueIdentifier );

		// Make the actual call out to the server
		WWW www = new WWW( JPCSRuntimeSettings.instance.serverURL.TrimEnd( '/' ) + "/redeem.php", form );
		yield return www;

		// Catch HTTP errors
		if ( !String.IsNullOrEmpty( www.error ) ) {
			OnCodeRedeemFailure( "HTTP Error: " + www.error );
			yield break;
		}

		// Convert server response to JSON object
		JsonData responseJson = JsonMapper.ToObject( www.text );

		// Check for error
		if ( responseJson.Keys.Contains( "error" ) ) {
			// Fire the failure event
			OnCodeRedeemFailure( responseJson[ "error" ].ToString( ) );
			yield break;
		}

		// Security - Verify hash of promo code (no dashes) + salt + product_id + salt (reversed) + device ID
		char[] revSaltArray = JPCSRuntimeSettings.instance.salt.ToCharArray( );
		Array.Reverse( revSaltArray );
		string revSalt = new string( revSaltArray );
		string msg = code.Replace( "-", "" ) + JPCSRuntimeSettings.instance.salt + responseJson[ "product_id" ] + revSalt + SystemInfo.deviceUniqueIdentifier;

		if ( responseJson[ "hash_string" ].ToString( ) != ComputeSHA1Hash( msg ) ) {
			// Hash doesn't match, we probably have a hacker on our hands
			OnCodeRedeemFailure( "HACK ATTEMPT: Promo code hash from server doesn't match" );
			yield break;
		}

		// Fire the success event
		OnCodeRedeemSuccess( responseJson[ "product_id" ].ToString( ) );
	}


	private string ComputeSHA1Hash( string msg ) {
		// Compute the hash
		SHA1Managed hasher = new SHA1Managed( );
		byte[] hashBytes = hasher.ComputeHash( Encoding.ASCII.GetBytes( msg ) );

		// Convert bytes to hex string
		StringBuilder sb = new StringBuilder( );
		foreach ( byte b in hashBytes ) {
			string hex = b.ToString( "x2" );
			sb.Append( hex );
		}
		return sb.ToString( );
	}

#endregion

	void Start(){
		DontDestroyOnLoad(gameObject);
	}
}
