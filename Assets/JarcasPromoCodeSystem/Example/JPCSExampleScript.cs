//-----------------------------------------------------
// Jarcas Promo Code System
// Copyright 2014 Jarcas Studios
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

public class JPCSExampleScript : MonoBehaviour {

	private string msg = "";
	private string code = "";

	private int currCoins = 0;
	private bool premiumContentUnlocked = false;


	private void Start( ) {
		// Register event handlers
		PromoCodeServer.instance.OnCodeRedeemSuccess += OnCodeRedeemSuccess;
		PromoCodeServer.instance.OnCodeRedeemFailure += OnCodeRedeemFailure;
	}


	private void OnGUI( ) {
		GUILayout.BeginArea( new Rect( 20, 20, 400, 400 ) );

		// Player's inventory
		GUILayout.Label( "INVENTORY" );
		GUILayout.Label( "   Player currently has " + currCoins.ToString( ) + " coins." );
		GUILayout.Label( "   Level Pack 1 (Premium Content) is currently " + ( premiumContentUnlocked ? "Unlocked" : "Locked" ) );

		// Promo code entry
		GUILayout.Space( 20 );
		GUILayout.Label( "Enter Promo Code" );
		code = GUILayout.TextField( code, 30 );

		if ( GUILayout.Button( "Redeem Code" ) ) {
			RedeemPressed( );
		}

		// Server response
		GUILayout.Space( 20 );
		GUILayout.Label( msg );

		GUILayout.EndArea( );
	}


	private void RedeemPressed( ) {
		PromoCodeServer.instance.RedeemPromoCode( code );
	}


	private void OnCodeRedeemSuccess( string productID ) {
		switch ( productID ) {
		case "100_coins":
			msg = "Redeemed 100 coins!";
			currCoins += 100;
			break;
		case "level_pack_1":
			msg = "Redeemed access to Level Pack 1!";
			premiumContentUnlocked = true;
			break;
		default:
			msg = "Redeemed product ID: " + productID + "\n\nIf you'd like to try our example in-game responses to promo codes, generate and then redeem codes for product ID '100_coins' or 'level_pack_1'";
			break;
		}
	}

	private void OnCodeRedeemFailure( string errorMsg ) {
		if ( errorMsg.StartsWith( "HACK ATTEMPT" ) ) {
			// We've got a hacker, exit the app
			Debug.LogError( errorMsg );
			Application.Quit( );
		}
		msg = "Error: " + errorMsg;
	}
}
