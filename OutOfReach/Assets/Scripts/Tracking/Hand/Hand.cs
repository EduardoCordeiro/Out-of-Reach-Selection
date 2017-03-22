using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
	// Hand
	public GameObject joint;
	public GameObject elbow;
	public GameObject center;

	// Fingers
	public Transform thumb;
	public Transform index;
	public Transform middle;
	public Transform ring;
	public Transform pinky;

	// Hand manager
	public HandManager handManager;

	// Hand state
	private bool isClosed;

	/// <summary>
	/// Gets or sets a value indicating whether the hand is closed.
	/// </summary>
	public bool IsClosed 
	{
		get { return this.isClosed; }
		set { isClosed = value; }
	}

	private bool isEngaged;

	/// <summary>
	/// Gets or sets a value indicating whether the hand is engaged.
	/// </summary>
	public bool IsEngaged
	{
		get { return this.isEngaged; }
		set { isEngaged = value; }
	}

    private bool isInside;

    public bool IsInside {

        get { return this.isInside; }
        set { isInside = value; }
    }

	void Start()
	{
		isClosed = false;
		isEngaged = false;
	}

	/// <summary>
	/// Updates the hand open/close state and captures the initial hand position.
	/// </summary>
    public void UpdateHandState(bool isClosed) {

        if (this.isClosed != isClosed) {

            if (isClosed) {

                this.isClosed = true;

                isEngaged = true;
                handManager.EngagedHands.Add(this);
            }
            else {

                this.isClosed = false;

                if (isEngaged) {

                    isEngaged = false;
                    handManager.EngagedHands.Remove(this);
                }
            }
        }
    }

	/// <summary>
	/// Opens the hand.
	/// </summary>
	public void OpenHand()
	{
		OpenFinger(thumb);
		OpenFinger(index);
		OpenFinger(middle);
		OpenFinger(ring);
		OpenFinger(pinky);
	}

	/// <summary>
	/// Closes the hand.
	/// </summary>
	public void CloseHand()
	{
		bool left = gameObject.name.Equals("LeftHand") ? true : false;

		CloseThumb(thumb, left);
		CloseFinger(index, left);
		CloseFinger(middle, left);
		CloseFinger(ring, left);
		CloseFinger(pinky, left);
	}

	/// <summary>
	/// Opens the finger.
	/// </summary>
	private void OpenFinger(Transform finger) 
	{
		finger.localRotation = Quaternion.identity;
		finger = finger.GetChild(0);
		finger.localRotation = Quaternion.identity;
		finger = finger.GetChild(0);
		finger.localRotation = Quaternion.identity;
	}

	/// <summary>
	/// Closes the finger.
	/// </summary>
	private void CloseFinger(Transform finger, bool left) 
	{
		finger.localRotation = Quaternion.Euler(0, 0, 270 * (left ? -1.0f : 1.0f));
		finger = finger.GetChild(0);
		finger.localRotation = Quaternion.Euler(0, 0, 240 * (left ? -1.0f : 1.0f));
		finger = finger.GetChild(0);
		finger.localRotation = Quaternion.Euler(0, 0, 270 * (left ? -1.0f : 1.0f));
	}

	/// <summary>
	/// Closes the thumb.
	/// </summary>
	private void CloseThumb(Transform finger, bool left) 
	{
		finger.localRotation = Quaternion.identity;
		finger = finger.GetChild(0);
		finger.localRotation = Quaternion.Euler(60, 0, 315 * (left ? -1.0f : 1.0f));
		finger = finger.GetChild(0);
		finger.localRotation = Quaternion.Euler(90, 0, 0 * (left ? -1.0f : 1.0f));
	}
}