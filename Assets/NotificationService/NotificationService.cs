// Copyright (C), Insalgo, 2013-2015.
// Unauthorized copying of this file, via any medium is strictly prohibited.
//
// File name:   NotificationService.cs
// Created:     09/11/2014 by Natalia Kolasinska <kolasinska@insalgo.com>.
// Updated: 	21/07/2015 by Michal Lipski <lipski@insalgo.com>. 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*!
 * NotificationService class is a part of Notification Service Plugin that
 * allows you to create and run Android notifications for Unity based projects.
 */
public class NotificationService {

	//-- Public ----------------------------------------

	/*!	
	 * Create instance of com.unity3d.player.UnityPlayer and
	 * com.insalgo.notificationservice.NotificationActivity.
 	 */
	public void Init() {

		#if UNITY_ANDROID && !UNITY_EDITOR
		if (instanceObj  == null) {
			using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				playerActivityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			using (var pluginClass = new AndroidJavaClass("com.insalgo.notificationservice.NotificationActivity")) {
				if (pluginClass != null) {
					instanceObj  = pluginClass.CallStatic<AndroidJavaObject>("instance");
				}
			}
		}
		#endif
	}

	/*!
 	 * \brief Create notification event.
	 *	
 	 * @param notificationParameters Notification parameters.
 	 */
	public void CreateNotificationEvent(NotificationParams notificationParameters) {

		#if UNITY_ANDROID && !UNITY_EDITOR
		notificationEvents.Add(notificationParameters);
		#endif	
	}

	/*!
     *\brief Start notification service.
 	 */
	public void StartNotificationService() {
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaObject appContext = playerActivityContext.Call<AndroidJavaObject>("getApplicationContext");

		playerActivityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>{
			instanceObj.Call("createService", appContext, NotificationEventsToString(), removeNotification, className, onReboot);}));
		#endif	
    }

	/*!
 	 * \brief Stop notification service.
 	 */
    public void StopNotificationService() {
		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaObject appContext = playerActivityContext.Call<AndroidJavaObject>("getApplicationContext");
        notificationEvents.Clear();
    	playerActivityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>{
			instanceObj.Call("destroyService", appContext);}));
		#endif	
    }

	/*!
  	 * @param notificationID Notification's ID which will be removed.
 	 * @param delayInSeconds Delay in seconds after which notification will be removed.
 	 */
	public void RemoveSingleNotification(int notificationID, int delayInSeconds) {
		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaObject appContext = playerActivityContext.Call<AndroidJavaObject>("getApplicationContext");

		playerActivityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>{
			instanceObj.Call("removeNotification", appContext, notificationID, delayInSeconds);}));
		#endif	
	}

	/*!
 	 * \brief Remove all previous notifications.
     */
	public void RemoveAllPreviousNotifications() {

		#if UNITY_ANDROID && !UNITY_EDITOR
		removeNotification = "true";
		#endif
	}

	/*!
 	 * \brief Set default activity class name.
	 * 
	 * \note If activity class name differs from
	 * com.unity3d.player.UnityPlayerNativeActivity then it's necessary to
	 * update AndroidManifest file.
 	 */
	public void SetActivityClassName(string name) {
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		className = name;
		#endif
	}

	/*!
	 * \brief Keep all notifications after reboot.
	 * 
	 * \note Available in Android SDK >= 17
 	 */
	public void KeepNotificationsAfterReboot() {
	
		#if UNITY_ANDROID && !UNITY_EDITOR
		onReboot = "true";
		#endif
	}

	//-- Private ----------------------------------------

	/*!
 	 * This function takes all collected notification params, put them to one
	 * string variable and return.
 	 */
	private string NotificationEventsToString() {

		string notifications = notificationEvents.Count.ToString() + ";";

		for (int i = 0; i < notificationEvents.Count; i++) {

			notifications += notificationEvents[i].NotificationID.ToString() + ";";
			notifications += notificationEvents[i].DelayInSeconds.ToString() + ";";
			notifications += notificationEvents[i].TickerText + ";";
			notifications += notificationEvents[i].ContentTitle+ ";";
			notifications += notificationEvents[i].ContentText+ ";";
			notifications += notificationEvents[i].Vibrate.ToString() + ";";
			notifications += notificationEvents[i].SmallIcon + ";";
			notifications += notificationEvents[i].LargeIconPath + ";";
			notifications += notificationEvents[i].Sound.ToString() + ";";
			
			if( notificationEvents[i].LEDParameters == null ) {

				notifications += "false;"; // LED off
				notifications += "0;";
				notifications += "0;";
				notifications += "0;";
			} else {

				notifications += "true;"; // LED on
				notifications += ColorToARGBInBits(notificationEvents[i].LEDParameters.LEDColorARGB).ToString() + ";";
				notifications += notificationEvents[i].LEDParameters.OffTimeInMiliseconds.ToString() + ";";
				notifications += notificationEvents[i].LEDParameters.OnTimetInMiliseconds.ToString() + ";";
			}
			
			notifications += ColorToARGBInBits(notificationEvents[i].NewIconStyleColor).ToString() + ";";
		}

		return(notifications);
	}

#if UNITY_ANDROID && !UNITY_EDITOR
	private AndroidJavaObject instanceObj = null;

	private AndroidJavaObject playerActivityContext = null;

	private string onReboot = "false";

	private string className = "com.unity3d.player.UnityPlayerNativeActivity";

	private string removeNotification = "false";
#endif

	private List<NotificationParams> notificationEvents = new List<NotificationParams>();

	private int ColorToARGBInBits( Color32 color ) {
		
		return (color.a << 24) | (color.r << 16) | (color.g << 8) | color.b;
	}
}