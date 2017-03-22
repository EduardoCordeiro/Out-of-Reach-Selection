using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class TouchManager
{
	// Timers
	private float firstTouchTime;
	public float doubleTouchDelay;

	// State flags
	private bool singleTouchUp;

	/// <summary>
	/// Gets a value indicating whether this <see cref="ClickManager"/>get single touch up.
	/// </summary>
	public bool GetSingleTouchUp   
	{ 
		get { return this.singleTouchUp; } 
	}

	private bool singleTouchDown;

	/// <summary>
	/// Gets a value indicating whether this <see cref="ClickManager"/> get single touch down.
	/// </summary>
	public bool GetSingleTouchDown 
	{ 
		get { return this.singleTouchDown; } 
	}

	private bool doubleTouchUp;

	/// <summary>
	/// Gets a value indicating whether this <see cref="ClickManager"/> get double touch up.
	/// </summary>
	public bool GetDoubleTouchUp   
	{ 
		get { return this.doubleTouchUp; } 
	}

	private bool doubleTouchDown;

	/// <summary>
	/// Gets a value indicating whether this <see cref="ClickManager"/> get double touch down.
	/// </summary>
	public bool GetDoubleTouchDown 
	{ 
		get { return this.doubleTouchDown; } 
	}

	// Auxiliary flags
	private bool singleTouch;
	private bool doubleTouch;

	private bool buttonPressed;
	private bool buttonDoublePressed;

	public TouchManager(float doubleTouchDelay)
	{
		singleTouchUp = false;
		singleTouchDown = false;

		doubleTouchUp = false;
		doubleTouchDown = false;

		singleTouch = false;
		doubleTouch = false;

		firstTouchTime = 0.0f;
		this.doubleTouchDelay = doubleTouchDelay;
	}

	/// <summary>
	/// Registers a double touch down
	/// </summary>
	public void DoubleTouchDown()
	{
		if (!buttonDoublePressed) 
		{
			buttonDoublePressed = true;

			if ((Time.time - firstTouchTime) < doubleTouchDelay)
			{
				singleTouch = false;
				doubleTouch = true;
				doubleTouchDown = true;
			}
		}
	}

	/// <summary>
	/// Registers a single touch down
	/// </summary>
	public void TouchDown()
	{
		if (!buttonPressed) 
		{
			buttonPressed = true;

			if (!singleTouch)
			{ 
				singleTouch = true;
				singleTouchDown = true;
				firstTouchTime = Time.time;
			} 
		}
	}

	/// <summary>
	/// Registers a single touch up or a double touch up
	/// </summary>
	public void TouchUp()
	{
		if (buttonPressed)
		{
			buttonPressed = false;
			buttonDoublePressed = false;

			if (doubleTouch)
			{
				doubleTouch = false;
				doubleTouchUp = true;
			} 
			else
			{
				singleTouchUp = true;
			}
		}
	}

	/// <summary>
	/// Resets the state flags to false.
	/// </summary>
	public void Reset()
	{
		singleTouchUp = false;
		singleTouchDown = false;

		doubleTouchUp = false;
		doubleTouchDown = false;

		if (singleTouch && (Time.time - firstTouchTime) > doubleTouchDelay)
		{
			singleTouch = false;
		}
	}
}