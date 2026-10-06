// Copyright (C), Insalgo, 2013-2015.
// Unauthorized copying of this file, via any medium is strictly prohibited.
//
// File name:   NotificationParams.cs
// Created:     21/07/2015 by Michal Lipski <lipski@insalgo.com>.
// Description: Stores params for single notification.

using UnityEngine;

/*!
 * \brief Sets default parameters for notification.
 */
public class NotificationParams {
	
	/*!
 	 * All notifications have to meet basic requirements. Every notification
	 * contains its own notificationID, ticker Text, content title, content
	 * text and small icon. 
 	 * 
 	 * @param notificationID Notification's ID.
 	 * @param tickerText Text of your notification that will be displayed
	 *	in the notification bar.
 	 * @param contentTitle Content title of your notification that will be
	 * displayed in the notification bar.
 	 * @param contentText Content text of your notification that will be
	 *  displayed in the notification bar.
 	 * @param smallIcon Referes to the file name (without extension) located in
	 *	Assets\\Plugins\\Android\\res\\drawable. If there is no such file,
	 *	default unity icon will be set.
 	 *
	 * \note If two or more notifications have the same id, they will override
	 * themselves.
	 * 
 	 * \warning There is a difference between Android 5.0 and lower versions in
	 * style and convention of creating small icons for notifications.
	 * Notification Service plugin supports Android 5.0 and the lower versions.
	 * It's necessary to create two versions of small icons. For older Android
	 * versions there are no restrictions of file extension for small icon. For
	 * new icon style used from Android 5.0 read more info at
	 * https://www.google.com/design/spec/style/icons.html	
 	 */
	public NotificationParams(int notificationID, string tickerText,
		string contentTitle, string contentText, string smallIcon) {
		
		NotificationID = notificationID;
		this.TickerText = tickerText;
		this.ContentTitle = contentTitle;
		this.ContentText = contentText;
		this.SmallIcon = smallIcon;

		NewIconStyleColor = Color.black;
		Sound = false;
		Vibrate = false;
	}

	/*!
 	 * Set vibrations on or off when notification appears.
	 * 
	 * Default is `false`.
	 *	
	 * \note Works only on devices that support vibrations.
	 * 
	 * \note If vibrations doesn't work on your Android device, be sure that any
	 * options related to power saving mode are off.
     */
	public bool Vibrate { get; set; }

	/*!
	 * \brief Set sound on or off when notification appears.
 	 *	
 	 * Default is `false`.
 	 */
	public bool Sound { get; set; }

	/*!	
	 * \brief Set path for large icon.
   	 *	
 	 * It allows to set specific large notification icon. File has to be located
	 * in Assets\\Plugins\\Android\\res\\drawable. If file doesn't exist or
	 * wrong name is set, large icon will not be used, and warning will be
	 * displayed in device's log.
 	 */
	public string LargeIconPath { get; set; }

	/*!	
	 * \brief Set delay after which notification will be removed.
 	 */
	public int DelayInSeconds { get; set; }

	/*!
	 * \brief Set color for the new icon style.
	 * 
	 * For notification to work properly on devices with Android 5.0 or higher,
	 * small icon color needs to be set.
	 */
	public Color32 NewIconStyleColor { get; set; }

	/*!
	 * \brief Set LED parameters.
	 * 
	 * \note Available in Android SDK >= 21
 	 */
	public LedParams LEDParameters { get; set; }

	/*!
	 * \brief Ticker text.
 	 */
	public string TickerText { get; private set; }

	/*!
	 * \brief Content text.
 	 */
	public string ContentTitle { get; private set; }

	/*!
	 * \brief Content text.
 	 */
	public string ContentText { get; private set; }

	/*!
	 * \brief Small icon path.
 	 */
	public string SmallIcon { get; private set; }

	/*!
	 * \brief Notification ID.
 	 */
	public int NotificationID { get; private set; }
}