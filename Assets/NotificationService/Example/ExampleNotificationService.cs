// Copyright (C), Insalgo, 2013-2015.
// Unauthorized copying of this file, via any medium is strictly prohibited.
//
// File name:   ExampleNotificationService.cs
// Created:     09/11/2014 by Natalia Kolasinska <kolasinska@insalgo.com>.
// Updated: 	21/07/2015 by Michal Lipski <lipski@insalgo.com>.

using UnityEngine;
using System.Collections;
using System.Threading;
using System;

/*!
 * Example notification service usage.
 */
public class ExampleNotificationService : MonoBehaviour {

    void OnApplicationPause( bool pause ) {
		
		Init();

		if( pause ) { // Application goes to background.
		
			SetExampleNotifications();

			notificationService.StartNotificationService();
		}
		else // Application goes to foreground.
			notificationService.StopNotificationService();

		// Remove notification.
		// Notification with id = 2 will be removed within 35 seconds.
		notificationService.RemoveSingleNotification (2, 35);
	}

	void Init() {

		if( !inited ) {

			notificationService.Init();
			inited = true;
		}
	}

	void SetExampleNotifications() {

		// (Optional)
		notificationService.RemoveAllPreviousNotifications();
		
		// (Optional)
		notificationService.SetActivityClassName("com.unity3d.player.UnityPlayerNativeActivity");
		
		// (Optional)
		notificationService.KeepNotificationsAfterReboot();

		// Example icons (downloaded from https://icons8.com)
		NotificationParams paramsForFirstNotification = new NotificationParams(1, "This is first Notification", "This is first Title", "This is first Content", "icon1");
		paramsForFirstNotification.Vibrate = true;
		paramsForFirstNotification.LargeIconPath = "large1";
		paramsForFirstNotification.DelayInSeconds = 5;
		paramsForFirstNotification.Sound = true;
		paramsForFirstNotification.NewIconStyleColor = Color.blue;
		paramsForFirstNotification.LEDParameters = new LedParams( Color.green, 200, 200 );
		notificationService.CreateNotificationEvent(paramsForFirstNotification);

		NotificationParams paramsForSecondNotification = new NotificationParams(2, "This is second Notification", "This is second Title", "This is second Content", "icon2");
		paramsForSecondNotification.DelayInSeconds = 15;
		paramsForSecondNotification.LargeIconPath = "large1";
		paramsForSecondNotification.NewIconStyleColor = Color.cyan;
		notificationService.CreateNotificationEvent(paramsForSecondNotification);

		NotificationParams paramsForThirdNotification = new NotificationParams(3, "This is third Notification", "This is third Title", "This is first Content", "icon3");
		paramsForThirdNotification.DelayInSeconds = 30;
		paramsForThirdNotification.LargeIconPath = "large2";
		paramsForThirdNotification.NewIconStyleColor = Color.red;
		notificationService.CreateNotificationEvent(paramsForThirdNotification);

		NotificationParams paramsForForthNotification = new NotificationParams(4, "This is fourth Notification", "This is fourth Title", "This is fourth Content", "icon4");
		paramsForForthNotification.DelayInSeconds = 60;
		paramsForForthNotification.LargeIconPath = "large3";
		paramsForForthNotification.NewIconStyleColor = Color.yellow;

		notificationService.CreateNotificationEvent(paramsForForthNotification);
	}

	void OnGUI() {

		GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height / 2, 200, 200),
		    "Set your application to background to see example notifications" );
	}

	NotificationService notificationService = new NotificationService();
	
	private bool inited = false;
}