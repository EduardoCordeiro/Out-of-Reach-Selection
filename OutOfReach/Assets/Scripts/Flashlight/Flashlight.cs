using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flashlight : MonoBehaviour {

    // Scene Transform
    private GameObject scene;

    // Objects in the scene
    private GameObject[] sceneObjects;

    // The Object that is selected [Final object]
    private GameObject chosenObject;

    // List of Objects that are currently being selected
    private List<GameObject> gameObjectList;

    // Carl GameObject
    private GameObject carl;

    // Character Cliente
    private GameObject characterClient;

    // Hand Related objects
    public HandManager handManager;

    private Hand hand;

    public string activeHandName;

    // Cone Objects
    private GameObject flashlightCone;

    // Torso origin
    private GameObject torsoOrigin;

    // Desired Object
    public GameObject desiredObject;

    // Audio source
    private AudioSource audioSource;

    private bool handActive;

    public TestController tests;

	// Use this for initialization
	void Start () {

        this.scene = GameObject.Find("Scene");

        this.sceneObjects = new GameObject[this.scene.transform.childCount];

        // get the Scene GameObjects
        for (int i = 0; i < scene.transform.childCount; i++)
            this.sceneObjects[i] = scene.transform.GetChild(i).gameObject;

        this.chosenObject = null;

        this.gameObjectList = new List<GameObject>();

        this.carl = GameObject.Find("Character");

        this.characterClient = GameObject.Find("Character Client");

        // Choose here the active hand
        this.hand = handManager.RegisteredHands[activeHandName];

        this.flashlightCone = GameObject.Find("Flashlight");

        // cone starting position
        this.flashlightCone.transform.position = hand.transform.position;

        // Cone starts Disabled
        this.DisableCone();

        this.torsoOrigin = GameObject.Find("TorsoOrigin");

        audioSource = this.GetComponent<AudioSource>();

        handActive = false;
	}
	
	// Update is called once per frame
	void Update () {

        flashlightCone.transform.position = hand.transform.position;

        // This is here because The cone needs to face where the hand is facing
        // change this incase the transform.right doesnt work on imuduino
        //cone.transform.forward = hand.transform.right;
        //cone.transform.forward = hand.transform.position - foreArm.transform.position;

        flashlightCone.transform.forward = hand.transform.right;

        print("flash");

        if (hand.IsEngaged) {

            EnableCone();

            print("hand active");

            if (chosenObject != null) {

                chosenObject.GetComponent<SelectionScript>().DeactivateSelectionObject();

                chosenObject = null;
            }

            chosenObject = RayCasting();

            if (chosenObject == null) {

                // see whats inside cone
                if (gameObjectList.Count > 1) {

                    GameObject closestObject = null;
                    float minDistance = Mathf.Infinity;

                    foreach (GameObject go in gameObjectList) {

                        float distance = DistanceToLine(hand.transform.position, hand.transform.right, go.GetComponent<Collider>().bounds.center);

                        if (distance < minDistance) {
                            closestObject = go;
                            minDistance = distance;
                        }
                    }

                    chosenObject = closestObject;
                }
                else if(gameObjectList.Count == 1)
                    chosenObject = gameObjectList[0];
            }

            if (chosenObject != null) {
                // do the highlight stuff
                chosenObject.GetComponent<SelectionScript>().ActivateSelectionObject();
            }

            if (!handActive)
                handActive = true;
        }
        else if (!hand.IsEngaged && handActive) {

            DisableCone();

            if (chosenObject != null) {

                if (chosenObject.name == desiredObject.name) {

                    chosenObject.GetComponent<SelectionScript>().DeactivateSelectionObject();
                    CorrectSelection(chosenObject);
                }
                else {

                    audioSource.Play();

                    // Inform the reader that an incorect object was selected
                    tests.incorrectObjectSelection++;
                    chosenObject = null;
                }
            }

            gameObjectList.Clear();
            handActive = false;
        }
	}

    public static float DistanceToLine(Vector3 rayOrigin, Vector3 rayDirection, Vector3 point) {

        return Vector3.Cross(rayDirection, point - rayOrigin).magnitude;
    }

    GameObject RayCasting() {

        // Simple Ray Casting
        // Bit shift the index of the layer (9, Scene) to get a bit mask
        int layerMask = 1 << LayerMask.NameToLayer("Scene");

        RaycastHit hit;

        // Does the ray intersect any objects in the Scene Layer
        if (Physics.Raycast(hand.transform.position, hand.transform.right, out hit, Mathf.Infinity, layerMask)) {

            Debug.DrawRay(hand.transform.position, hand.transform.right, Color.gray);
            Debug.Log("Did Hit");

            return hit.collider.gameObject;
        }
        else {

            Debug.DrawRay(hand.transform.position, hand.transform.right * 1000, Color.gray);
            Debug.Log("Did not Hit");

            return null;
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

    void OnTriggerEnter(Collider collider) {

        //Debug.Log("Enter :" + collider.name);

        //if (collider.GetComponent<SelectionScript>() != null)
        //    collider.GetComponent<SelectionScript>().ActivateSelectionObject();

        if (collider.gameObject.layer == LayerMask.NameToLayer("Scene")) {

            // Add the GameOject to the list of Objects
            if (!gameObjectList.Contains(collider.gameObject))
                gameObjectList.Add(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider) {

        //Debug.Log("Exit :" + collider.name);

        //if (collider.GetComponent<SelectionScript>() != null)
        //    collider.GetComponent<SelectionScript>().DeactivateSelectionObject();

        if (collider.gameObject.layer == LayerMask.NameToLayer("Scene")) {

            // Remove the GameOject from the list of Objects
            gameObjectList.Remove(collider.gameObject);
        }
    }

    void EnableCone() {

        // Activate on the Cone
        this.GetComponent<MeshRenderer>().enabled = true;

        this.GetComponent<MeshCollider>().enabled = true;
    }

    void DisableCone() {

        // Deactivate the cone
        this.GetComponent<MeshRenderer>().enabled = false;

        this.GetComponent<MeshCollider>().enabled = false;

        // Ensure that all objects' SelectionScript is disabled
        foreach (GameObject sceneObject in sceneObjects)
            if (sceneObject.transform.GetChild(0).GetComponent<SelectionScript>() != null)
                sceneObject.transform.GetChild(0).GetComponent<SelectionScript>().DeactivateSelectionObject();
    }
}
