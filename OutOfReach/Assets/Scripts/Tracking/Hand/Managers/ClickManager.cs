using UnityEngine;

using System;
using System.Collections.Generic;

public class ClickManager
{
	// Timers
	private float firstClickTime;
	public float doubleClickDelay;

	// State flags
	private bool singleClickUp;

	/// <summary>
	/// Gets a value indicating whether this <see cref="ClickManager"/>get single click up.
	/// </summary>
	public bool GetSingleClickUp   
	{ 
		get { return this.singleClickUp; } 
	}

	private bool singleClickDown;

	/// <summary>
	/// Gets a value indicating whether this <see cref="ClickManager"/> get single click down.
	/// </summary>
	public bool GetSingleClickDown 
	{ 
		get { return this.singleClickDown; } 
	}

	private bool doubleClickUp;

	/// <summary>
	/// Gets a value indicating whether this <see cref="ClickManager"/> get double click up.
	/// </summary>
	public bool GetDoubleClickUp   
	{ 
		get { return this.doubleClickUp; } 
	}

	private bool doubleClickDown;

	/// <summary>
	/// Gets a value indicating whether this <see cref="ClickManager"/> get double click down.
	/// </summary>
	public bool GetDoubleClickDown 
	{ 
		get { return this.doubleClickDown; } 
	}

	// Auxiliary flags
	private bool singleClick;
	private bool doubleClick;
	private bool buttonPressed;

	public ClickManager(float doubleClickDelay)
	{
		singleClickUp = false;
		singleClickDown = false;

		doubleClickUp = false;
		doubleClickDown = false;

		singleClick = false;
		doubleClick = false;

		firstClickTime = 0.0f;
		this.doubleClickDelay = doubleClickDelay;
	}
		

	/// <summary>
	/// Registers a single click down or a double click down
	/// </summary>
	public void ClickDown()
	{
		if (!buttonPressed) 
		{
			buttonPressed = true;

			if (!singleClick) // Start over with a first click
			{ 
				singleClick = true;
				singleClickDown = true;
				firstClickTime = Time.time;
			} 
			else // First click registered, check if the second one is within the time range
			{ 
				if ((Time.time - firstClickTime) < doubleClickDelay) 
				{
					singleClick = false;
					doubleClick = true;
					doubleClickDown = true;
				}
			}
		}
	}

	/// <summary>
	/// Registers a single click up or a double click up
	/// </summary>
	public void ClickUp()
	{
		if (buttonPressed)
		{
			buttonPressed = false;

			if (doubleClick) // Check if we reached a doubleclick
			{
				doubleClick = false;
				doubleClickUp = true;
			} 
			else // Otherwise we consider a singleclick
			{
				singleClickUp = true;
			}
		}
	}
		
	/// <summary>
	/// Resets the state flags to false.
	/// </summary>
	public void Reset()
	{
		singleClickUp = false;
		singleClickDown = false;

		doubleClickUp = false;
		doubleClickDown = false;

		if (singleClick && (Time.time - firstClickTime) > doubleClickDelay)
		{
			singleClick = false;
		}
	}
}