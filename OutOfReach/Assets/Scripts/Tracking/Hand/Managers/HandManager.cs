using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandManager : MonoBehaviour {

	private List<Hand> engagedHands;

	/// <summary>
	/// Gets the engaged hands.
	/// </summary>
	public List<Hand> EngagedHands { 
		get { return this.engagedHands; } 
	}

	private Dictionary<string, Hand> registeredHands;

	/// <summary>
	/// Gets the registered hands.
	/// </summary>
	public Dictionary<string, Hand> RegisteredHands {
		get { return this.registeredHands; } 
	}

	void Awake() {

		engagedHands = new List<Hand>(2);
		registeredHands = new Dictionary<string, Hand>(2);

		registeredHands["LeftHand"] = GameObject.Find("LeftHand").GetComponent<Hand>();
		registeredHands["RightHand"] = GameObject.Find("RightHand").GetComponent<Hand>();
	}

	/// <summary>
	/// Returns a list of active hands. A hand is considered 
	/// active if its closed state is set to true.
	/// </summary>
	public List<Hand> ActiveHands()	{

		List<Hand> activeHands = new List<Hand>();

		foreach (Hand hand in registeredHands.Values) 
			if (hand.IsClosed)
				activeHands.Add(hand);

		return activeHands;
	}
}