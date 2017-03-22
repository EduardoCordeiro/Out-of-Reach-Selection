using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System;

public class StretchGoGo : MonoBehaviour {

    // GoGo Algorithm

    // Origin = this.transform.position
    // Rr = length of vector |Rr pointing from origin to user hand
    // Virtual hand position = {Rv, Φ, θ}
    // Rv = length of the virtual arm
    // |Rv = vector from origin to virtual hand

    // Scene Transform
    private GameObject scene;

    private GameObject[] sceneObjects;

    // The Object that is selected [Final object]
    private GameObject chosenObject;

    // List of Objects that are currently being selected
    private List<GameObject> gameObjectList;

    // The Virtual Hand
    public GameObject virtualHand;

    // Hand Related objects
    private Hand hand;

    public string activeHandName;

    public HandManager handManager;

    // Carl's Origin
    private GameObject origin;

    public Transform armOrigin;

    // Virtual Hand Coordinates {Rv, Φ, θ}
    private Vector3 virtualHandPosition;

    private float virtualHandDistance;

    // Three distances for the concentric regions
    public float innerRegion;

    public float middleRegion;

    public float outerRegion;

    // Desired Object
    public GameObject desiredObject;

    // Audio source
    private AudioSource audioSource;

    private bool handActive;

    public TestController tests;

    public string activeRegion;

    // Use this for initialization
    void Start() {

        scene = GameObject.Find("Scene");

        sceneObjects = new GameObject[this.scene.transform.childCount];

        // Set the Selected Material on the objects
        for (int i = 0; i < scene.transform.childCount; i++)
            sceneObjects[i] = scene.transform.GetChild(i).gameObject;

        chosenObject = null;

        gameObjectList = new List<GameObject>();

        // Choose here the active hand
        hand = handManager.RegisteredHands[activeHandName];

        //this.origin = GameObject.Find("Spine1").transform.position;
        origin = GameObject.Find("Origin");

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        float distanceToOrigin = Vector3.Distance(hand.transform.position, origin.transform.position);

        if (hand.IsEngaged) {

            //Debug.Log("DISTANCE " + distanceToOrigin);

            // Inner Region
            if (distanceToOrigin >= innerRegion && distanceToOrigin < middleRegion) {

                virtualHandDistance -= 10.0f * Time.deltaTime; //Change this value

                if (virtualHandDistance < 0.0f) 
                    virtualHandDistance = 0.0f;

                activeRegion = "inner";
            }
            // Middle Region
            else if (distanceToOrigin >= middleRegion && distanceToOrigin < outerRegion) {

                activeRegion = "middle";
            }
            // Outer Region
            else if (distanceToOrigin > outerRegion) {

                virtualHandDistance += 5.0f * Time.deltaTime;

                activeRegion = "outer";
            }

            // exponential smoothing filter
            virtualHandPosition = 
				0.5f * virtualHandPosition + 
				0.5f * (hand.transform.position + (hand.transform.position - origin.transform.position).normalized * virtualHandDistance);

            virtualHand.transform.position = virtualHandPosition;

            if(!handActive)
                handActive = true;
        }
        else if(!hand.IsEngaged && handActive) {

            SingleObjectSelection();

            handActive = false;
        }
    }

    public void ResetHandPosition() {

        virtualHandPosition = virtualHand.transform.position;

        virtualHandDistance = 0.0f;
    }

    void SingleObjectSelection() {

        if (gameObjectList.Count == 1) {

            chosenObject = gameObjectList[0];

            if (gameObjectList[0].name == desiredObject.name) {

                chosenObject.GetComponent<SelectionScript>().DeactivateSelectionObject();

                CorrectSelection(chosenObject);

                gameObjectList.Clear();
            }
            else {

                audioSource.Play();

                // Inform the reader that an incorect object was selected
                tests.incorrectObjectSelection++;
            }
        }
    }

    private void CorrectSelection(GameObject correctObject) {

        GameObject selectionObject = Instantiate(Resources.Load("Prefabs/Cactus/CorrectSelection", typeof(GameObject))) as GameObject;

        selectionObject.transform.position = correctObject.GetComponent<Collider>().bounds.center;
        selectionObject.transform.localScale = correctObject.GetComponent<Collider>().bounds.size * 1.1f;
    }

    public void DeleteCorrectSelection() {

        GameObject correctSelection = GameObject.Find("CorrectSelection(Clone)");

        if (correctSelection != null)
            Destroy(correctSelection);
    }

   /* void ControlInnerRegion() {

        print("Inner");

        if (Vector3.Distance(virtualHand.transform.position, hand.transform.position) > 0.2f) {

            Vector3 virtualToReal = virtualHand.transform.position - hand.transform.position;
            virtualToReal.Normalize();

            virtualHandPosition.Value = virtualToReal * Time.deltaTime * 2.0f;

            virtualHand.transform.position -= virtualHandPosition.Value;

            Debug.DrawLine(hand.transform.position, virtualHand.transform.position, Color.green);

        }
        else {

            virtualHand.transform.position = hand.transform.position;
        }
    }

    void ControlMiddleRegion() {

        print("Middle");

        // when hand is here stop virtual hand movement

        Debug.DrawLine(hand.transform.position, virtualHand.transform.position, Color.yellow);
    }

    void ControlOuterRegion(float distanceToOrigin) {

        print("Outer");

        // keep increasing position of virtual hand over time
        virtualHandDistance = distanceToOrigin + k * Mathf.Pow((distanceToOrigin - middleRegion), 2);

        virtualHandPosition.Value = (hand.transform.position - origin.transform.position).normalized * virtualHandDistance / 24.0f;

        virtualHand.transform.position += virtualHandPosition.Value;

        Debug.DrawLine(hand.transform.position, virtualHand.transform.position, Color.black);
    }*/

    public void AddObjectToList(GameObject collider) {

        print(collider.name);

        gameObjectList.Add(collider);
    }

    public void RemoveObjectFromList(GameObject collider) {

        gameObjectList.Remove(collider);
    }
}
