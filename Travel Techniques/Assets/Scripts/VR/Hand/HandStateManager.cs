using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandStateManager : MonoBehaviour 
{
	// Hands
	public GameObject leftHand;
	public Transform leftThumb;
	public Transform leftIndex;
	public Transform leftMiddle;
	public Transform leftRing;
	public Transform leftPinky;

	// Right Hand
	public GameObject rightHand;
	public Transform rightThumb;
	public Transform rightIndex;
	public Transform rightMiddle;
	public Transform rightRing;
	public Transform rightPinky;

	private List<Hand> engagedHands;
	public List<Hand> EngagedHands { get { return this.engagedHands; } }

	private Dictionary<string, Hand> registeredHands;
	public Dictionary<string, Hand> RegisteredHands { get { return this.registeredHands; } }

	void Awake()
	{
		engagedHands = new List<Hand>(2);
		registeredHands = new Dictionary<string, Hand>(2);

		registeredHands["LeftHand"] = new Hand(leftHand);
		registeredHands["RightHand"] = new Hand(rightHand);
	}
		
	// Update is called once per frame
	void Update() 
	{
		// Test purposes
		if (Input.GetKey(KeyCode.LeftArrow)) 
		{ 
			UpdateHandState("LeftHand", true);
		} 
		else 
		{
			UpdateHandState("LeftHand", false);
		}

		if (Input.GetKey(KeyCode.RightArrow)) 
		{ 
			UpdateHandState("RightHand", true);
		} 
		else 
		{
			UpdateHandState("RightHand", false);
		}
	}
		
	/// <summary>
	/// Registers an hand entry.
	/// </summary>
	public void RegisterHandEntry(GameObject insideObject, GameObject hand, GameObject axis, GameObject handle) 
	{
		Hand handAux = null;
	
		if (registeredHands.TryGetValue(hand.name, out handAux)) 
		{
			if (!handAux.IsEngaged) 
			{
				handAux.Axis = axis;
				handAux.Handle = handle;
				handAux.IsInside = true;
				handAux.InsideObject = insideObject;
			}
		}
		else 
		{
			Debug.LogError("RegisterHandEntry ERROR: Hand " + hand.name + " is not registered.");
		}
	}

	/// <summary>
	/// Unregisters the hand.
	/// </summary>
	public void RegisterHandExit(GameObject insideObject, GameObject hand, GameObject axis, GameObject handle) 
	{
		Hand handAux = null;

		if (registeredHands.TryGetValue(hand.name, out handAux))
		{
			if (!handAux.IsEngaged) 
			{
				if (handAux.IsInside && handAux.Axis.Equals(axis) && handAux.Handle.Equals(handle)) 
				{
					handAux.Axis = null;
					handAux.Handle = null;
					handAux.IsInside = false;
					handAux.IsEngaged = false;
					handAux.InsideObject = null;
				} 
			} 
			else 
			{
				handAux.IsInside = false;
			}
		}
		else 
		{
			Debug.LogError("RegisterHandExit ERROR: Hand " + hand.name + " is not registered.");
		}
	}

	/// <summary>
	/// Updates the hand open/close state and captures the initial hand position.
	/// </summary>
	public void UpdateHandState(string handName, bool isClosed)
	{
		Hand hand = null;

		if (registeredHands.TryGetValue(handName, out hand))
		{
			// Update only if the value has changed
			if (hand.IsClosed != isClosed) 
			{
				if (isClosed) 
				{
					closeHand(handName);
					hand.IsClosed = true;
					hand.InitialPosition = hand.Instance.transform.position;

					if (hand.IsInside) 
					{
						hand.IsEngaged = true;
						engagedHands.Add(hand);
					}
				} 
				else 
				{
					openHand(handName);
					hand.IsClosed = false;
					hand.InitialPosition = Vector3.zero;

					if (hand.IsEngaged) 
					{
						hand.IsEngaged = false;
						engagedHands.Remove(hand);

						if (!hand.IsInside) 
						{
							RegisterHandExit(hand.InsideObject, hand.Instance, hand.Axis, hand.Handle);
						}
					}
				}
			}
		}
		else 
		{
			Debug.LogError("UpdateHandState ERROR: Hand " + handName + " is not registered.");
		}
	}

	/// <summary>
	/// Returns a list of engaged hands inside a specific object. A hand is 
	/// considered engaged if its closed state and inside state is set to true. 
	/// </summary>
	public List<Hand> EngagedHandsFiltered(string insideObject)
	{
		List<Hand> engagedHandsFiltered = new List<Hand>();

		foreach (Hand hand in engagedHands) 
		{
			if (hand.InsideObject.name.Equals(insideObject)) 
			{
				engagedHandsFiltered.Add(hand);
			}
		}
		return engagedHandsFiltered;
	}

	/// <summary>
	/// Returns a list of active hands. A hand is considered 
	/// active if its closed state is set to true.
	/// </summary>
	public List<Hand> ActiveHands()
	{
		List<Hand> activeHands = new List<Hand>();

		foreach (Hand hand in registeredHands.Values) 
		{
			if (hand.IsClosed) 
			{
				activeHands.Add(hand);
			}
		}
		return activeHands;
	}

	/// <summary>
	/// Opens the hand.
	/// </summary>
	private void openHand(string handName)
	{
		if (handName.Equals("LeftHand")) 
		{
			openFinger(leftThumb);
			openFinger(leftIndex);
			openFinger(leftMiddle);
			openFinger(leftRing);
			openFinger(leftPinky);
		} 
		else 
		{	
			openFinger(rightThumb);
			openFinger(rightIndex);
			openFinger(rightMiddle);
			openFinger(rightRing);
			openFinger(rightPinky);
		}
	}

	/// <summary>
	/// Closes the hand.
	/// </summary>
	private void closeHand(string handName)
	{
		if (handName.Equals("LeftHand")) 
		{
            closeThumb(leftThumb, true);
			closeFinger(leftIndex, true);
			closeFinger(leftMiddle, true);
			closeFinger(leftRing, true);
			closeFinger(leftPinky, true);
		}
		else 
		{	
			closeThumb(rightThumb, false);
			closeFinger(rightIndex, false);
			closeFinger(rightMiddle, false);
			closeFinger(rightRing, false);
			closeFinger(rightPinky, false);
		}
	}
		
	/// <summary>
	/// Opens the finger.
	/// </summary>
	private void openFinger(Transform finger) 
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
	private void closeFinger(Transform finger, bool left) 
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
	private void closeThumb(Transform finger, bool left) 
	{
		finger.localRotation = Quaternion.identity;
		finger = finger.GetChild(0);
		finger.localRotation = Quaternion.Euler(60, 0, 315 * (left ? -1.0f : 1.0f));
		finger = finger.GetChild(0);
		finger.localRotation = Quaternion.Euler(90, 0, 0 * (left ? -1.0f : 1.0f));
	}
}