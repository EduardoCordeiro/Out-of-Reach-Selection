using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleSelectionHandler : MonoBehaviour {

    public HandManager handManager;

    private Hand hand;

    void Start() {

        hand = handManager.RegisteredHands["RightHand"];
    }

    void Update() {

    }

    public void DisperseObjects(List<GameObject> gameObjetList) {

        transform.forward = hand.transform.right;

        Vector3 newPosition = hand.transform.transform.position + hand.transform.transform.right * 2.5f;
        newPosition.y = 1.5f;

        transform.position = newPosition;

        // Use parent for alignment of pivots
        for (int i = 0; i < gameObjetList.Count; i++)
            gameObjetList[i].transform.parent.position = transform.GetChild(i).position;
    }
}
