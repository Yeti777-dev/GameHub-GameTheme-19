// Copyright (C), Insalgo, 2013-2015.
// Unauthorized copying of this file, via any medium is strictly prohibited.
//
// File name:   LedParams.cs
// Created:     21/07/2015 by Michal Lipski <lipski@insalgo.com>.
// Description: LedParams struct stores LED notification parameters.

using UnityEngine;
using System.Collections;

/*!
 * LED parameters
 */
public class LedParams {

	/*!
	 * \brief LED color in ARGB format.
	 */
	public Color32 LEDColorARGB { get; private set; }

	/*!
	 * \brief Number of milliseconds for the LED to be on while it's flashing.
	 */
	public int OnTimetInMiliseconds { get; private set; }

	/*!
	 * \brief Number of milliseconds for the LED to be off while it's flashing.
	 */
	public int OffTimeInMiliseconds { get; private set; }
	
	/*! 
	 * @param LEDColorARGB LED color in ARGB format.
 	 * @param offTimeInMiliseconds Number of milliseconds for the LED to be
	 *   off while it's flashing.
 	 * @param onTimetInMiliseconds Number of milliseconds for the LED to be
	 *   on while it's flashing.
	 */
	 public LedParams(Color32 LEDColorARGB, int offTimeInMiliseconds, int onTimetInMiliseconds) {
		
		this.LEDColorARGB = LEDColorARGB;
		this.OffTimeInMiliseconds = offTimeInMiliseconds;
		this.OnTimetInMiliseconds = onTimetInMiliseconds;    
	}
}