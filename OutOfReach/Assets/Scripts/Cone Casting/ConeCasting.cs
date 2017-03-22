using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeCasting : MonoBehaviour {

    private enum Phase {

        Movement,
        DoubleSelection,
        Refinement,
        Final
    }

    private Phase phase;

    // Phase of the Progressive Refinement
    //private int phase;

    // Scene Transform
    private GameObject scene;

    // Objects in the scene
    private GameObject[] sceneObjects;

    // The Object that is selected [Final object]
    private GameObject chosenObject;

    private List<GameObject> multipleSelectionObjects;

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

    // Test Purposes Only
    public GameObject testHand;

    // Cone Objects
    private GameObject cone;

    // GameObject used for the scaling
    private GameObject scalingCone;

    // Carl's Origin
    private GameObject origin;

    // Torso origin
    private GameObject torsoOrigin;

    // Stretch Go-Go Related Atributes
    // Virtual Hand Distance
    private float virtualHandDistance;

    // Three distances for the concentric regions
    public float innerRegion;

    public float middleRegion;

    public float outerRegion;

    // Starting angle between the hand up vectors
    private float startingAngle;

    public MultipleSelectionHandler multipleSelectionHandler;

    // Desired Object
    public GameObject desiredObject;

    // Audio source
    private AudioSource audioSource;

    private bool handActive;

    public TestController tests;

    public string activeRegion;

	// Use this for initialization
    void Start() {

        this.phase = Phase.Movement;

        //this.phase = Constants.MOVEMENT_PHASE;

        this.scene = GameObject.Find("Scene");

        this.sceneObjects = new GameObject[this.scene.transform.childCount];

        // get the Scene GameObjects
        for (int i = 0; i < scene.transform.childCount; i++)
            this.sceneObjects[i] = scene.transform.GetChild(i).gameObject;

        this.chosenObject = null;

        this.multipleSelectionObjects = new List<GameObject>();

        this.gameObjectList = new List<GameObject>();

        this.carl = GameObject.Find("Character");

        this.characterClient = GameObject.Find("Character Client");

        // Choose here the active hand
        this.hand = handManager.RegisteredHands[activeHandName];

        this.cone = GameObject.Find("Cone");

        this.scalingCone = GameObject.Find("ScalingCone");

        this.scalingCone.transform.localScale = Constants.startingConeScale;

        // cone starting position
        this.cone.transform.position = hand.transform.position;

        // Cone starts Disabled
        this.DisableCone();

        //CHANGE THIS TO ORIGIN FROM STRECTH GOGO
        //this.origin = GameObject.Find("Origin");
        this.origin = GameObject.Find("RightShoulder");

        this.torsoOrigin = GameObject.Find("TorsoOrigin");

        this.startingAngle = hand.transform.localEulerAngles.x;

        audioSource = GetComponent<AudioSource>();

        handActive = false;
    }
	
	void Update () {

        cone.transform.position = hand.transform.position;

        // This is here because The cone needs to face where the hand is facing
        // change this incase the transform.right doesnt work on imuduino
        //cone.transform.forward = hand.transform.right;
        //cone.transform.forward = hand.transform.position - foreArm.transform.position;

        cone.transform.forward = hand.transform.right;

        //Debug.Log("Object List Count" + gameObjectList.Count);

        // Cone Movement
        if (phase == Phase.Movement || phase == Phase.Refinement || phase == Phase.DoubleSelection) {

            if (hand.IsEngaged) {

                EnableCone();

                ControlHandPosition();

                ControlConeAperture();

                // Multiple Selection [Actual multiple Objects]
                /*if (handListener.coneCastingMultipleSelection) {

                    print("Performing Multiple Selection!");

                    MultipleObjectSelection();
                }*/

                if (!handActive)
                    handActive = true;
            }
            // Release
            else if(!hand.IsEngaged && handActive) {

                DisableCone();

                if (phase == Phase.DoubleSelection) {

                    RestoreObjectsToScene();

                    characterClient.GetComponent<TrackerClient>().TeleportPosition = new Vector3(0.0f, 0.0f, 0.0f);

                    characterClient.GetComponent<TrackerClient>().PositionChange = true;

                    characterClient.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

                    phase = Phase.Movement;
                }

                ObjectSelection();
                
                handActive = false;
            }
        }
        
        // If more than 1 object is selected
        if (phase == Phase.Refinement)
            UpdateSelectionPhaseTeleport();
    }

    private void RestoreObjectsToScene() {

        // restore objects
        foreach (GameObject sceneObject in sceneObjects) {

            // check here again soon for child
            sceneObject.transform.GetChild(0).gameObject.SetActive(true);

            sceneObject.transform.GetChild(0).GetComponent<SelectionScript>().ResetPosition();
        }
    }

    // Stretch Go-Go -> increase or decrease the reach
    void ControlHandPosition() {

        //Vector3 originToHand = hand.transform.position - origin.transform.position;
        Vector3 originToHand = hand.transform.position - torsoOrigin.transform.position;

        float originMagnitude = originToHand.magnitude;

        Debug.Log("Distance" + originMagnitude);

        // Inner Region
        if (originMagnitude >= innerRegion && originMagnitude < middleRegion) {

            virtualHandDistance -= 5.0f * Time.deltaTime;

            if (virtualHandDistance < 0.0f)
                virtualHandDistance = 0.0f;

            activeRegion = "inner";
        }
        // Middle Region
        else if (originMagnitude >= middleRegion && originMagnitude < outerRegion) {

            activeRegion = "middle";
        }
        // Outer Region
        else if (originMagnitude > outerRegion) {

            if (scalingCone.transform.localScale.y < Constants.coneRangeUpperLimit)
                virtualHandDistance += 5.0f * Time.deltaTime;

            activeRegion = "outer";
        }

        // Change the scale based on where the arm is
        Vector3 gogoScale = scalingCone.transform.localScale;

        if (virtualHandDistance == 0.0f) {

            gogoScale = new Vector3(Constants.coneRangeRatio, 1, Constants.coneRangeRatio);
        }
        else
            gogoScale = virtualHandDistance * 5 * new Vector3(Constants.coneRangeRatio, 1, Constants.coneRangeRatio);

        scalingCone.transform.localScale = gogoScale;
    }

    void ControlConeAperture() {

        float currentHandRotation = hand.transform.localEulerAngles.x;

        //Corrections for the cases where the hand is at 0º or 360º
        if (Mathf.Abs(currentHandRotation - startingAngle) > Mathf.Abs((currentHandRotation - 360.0f) - startingAngle))
            currentHandRotation -= 360.0f;
        else if (Mathf.Abs(currentHandRotation - startingAngle) > Mathf.Abs((currentHandRotation + 360.0f) - startingAngle))
            currentHandRotation += 360.0f;

        // The amount of movement (angle) that the hand travelled
        float handRotationDelta = currentHandRotation - this.startingAngle;

        this.startingAngle = currentHandRotation;

        // Calculate the scale based on the rotation
        float scaleAngle = handRotationDelta * Constants.coneApertureUpperLimit / 220.0f; //test value

        Vector3 apertureScale = cone.transform.localScale;

        apertureScale += new Vector3(-scaleAngle, -scaleAngle, 0);

        apertureScale = new Vector3(Mathf.Clamp(apertureScale.x, Constants.coneApertureLowerLimit, Constants.coneApertureUpperLimit),
                                    Mathf.Clamp(apertureScale.y, Constants.coneApertureLowerLimit, Constants.coneApertureUpperLimit),
                                    apertureScale.z);

        cone.transform.localScale = apertureScale;
    }

    void ObjectSelection() {

        if (gameObjectList.Count == 1) {

            chosenObject = gameObjectList[0];

            if (chosenObject.name == desiredObject.name) {

                CorrectSelection(chosenObject);

                gameObjectList.Clear();

                // Attaching the object to the cone for now
                //this.chosenObject.transform.parent = this.origin.transform;
                phase = Phase.Movement;
            }
            else {

                // wrong object
                audioSource.Play();

                // Inform the reader that an incorect object was selected
                tests.incorrectObjectSelection++;

                chosenObject = null;

                // Carl Y = 0 Adjustment
                ResetPlayerPosition();

                gameObjectList.Clear();

                phase = Phase.Movement;
            }
        }
        // More than 1 Object Selected
        else if (gameObjectList.Count > 1) {

            if (gameObjectList.Count == 2) {

                multipleSelectionHandler.DisperseObjects(gameObjectList);

                DisableSceneObjects(gameObjectList);

                DisableSelectedObjects(gameObjectList);

                gameObjectList.Clear();

                phase = Phase.DoubleSelection;
            }
            else {

                phase = Phase.Refinement;
            }
        }
    }

    private void ResetPlayerPosition() {

        characterClient.GetComponent<TrackerClient>().TeleportPosition = new Vector3(0.0f, 0.0f, 0.0f);

        characterClient.GetComponent<TrackerClient>().PositionChange = true;

        characterClient.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
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

    private void DisableSceneObjects(List<GameObject> gameObjectList) {

        // disable objects not in gameObjectList
        foreach (GameObject sceneObject in sceneObjects) {

            if (sceneObject.transform.GetChild(0).name == gameObjectList[0].name || sceneObject.transform.GetChild(0).name == gameObjectList[1].name) {

            }
            else {

                sceneObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    void DisableSelectedObjects(List<GameObject> gameObjectList) {

        foreach (GameObject gameObject in gameObjectList)
            gameObject.GetComponent<SelectionScript>().DeactivateSelectionObject();
    }

    // Actual Multiple Objects [not being tested, only to see if it's cool]
    void MultipleObjectSelection() {

        multipleSelectionObjects = gameObjectList;

        foreach (GameObject sceneObject in multipleSelectionObjects)
            sceneObject.transform.parent = this.transform;
    }

    void UpdateSelectionPhaseTeleport() {

        // Distances Dictionary
        Dictionary<GameObject, float> selectedObjects = new Dictionary<GameObject, float>();

        foreach (GameObject actualObject in gameObjectList) {

            selectedObjects.Add(actualObject, Vector3.Distance(hand.transform.position, actualObject.transform.position));

        }

        GameObject closestObject = null;
        float minDistance = Mathf.Infinity;

        // Create a Bounds to calculate the optimal point for the new Position
        Bounds bounds = new Bounds(gameObjectList[0].transform.position, new Vector3(1f, 1f, 1f));

        // Find out which object is closest to the user
        foreach (KeyValuePair<GameObject, float> selection in selectedObjects) {

            if (selection.Value < minDistance) {

                minDistance = selection.Value;
                closestObject = selection.Key;
            }

            bounds.Encapsulate(selection.Key.GetComponent<Collider>().bounds);
        }

        // Calculate the Closest Point to the objects
        Vector3 directionVector = bounds.center - hand.transform.position;

        float sphereRadius = Vector3.Distance(bounds.center, bounds.max);

        print("Count = " + selectedObjects.Count + "  " + bounds.center);

        GameObject sphere = Instantiate(Resources.Load("Prefabs/SphereCollider", typeof(GameObject))) as GameObject;
        sphere.GetComponent<SphereCollider>().center = bounds.center;
        sphere.GetComponent<SphereCollider>().radius = sphereRadius * 1.5f;

        // Ray Cast vs the Sphere
        RaycastHit rayHit;

        Physics.Raycast(new Ray(hand.transform.position, directionVector), out rayHit, Mathf.Infinity, 1 << Constants.sphereColliderMark);

        print("ray" + rayHit.point + rayHit.distance);

        if (rayHit.distance == 0.0f) {

            // put the player haflway between his position and the center
            Vector3 newPosition = hand.transform.position + (bounds.center - hand.transform.position) * 0.5f;

            characterClient.GetComponent<TrackerClient>().TeleportPosition = new Vector3(newPosition.x, 0.0f, newPosition.z);

            characterClient.GetComponent<TrackerClient>().PositionChange = true;

            characterClient.transform.position = new Vector3(newPosition.x, 0.0f, newPosition.z);
        }
        else {

            characterClient.GetComponent<TrackerClient>().TeleportPosition = new Vector3(rayHit.point.x, 0.0f, rayHit.point.z);

            characterClient.GetComponent<TrackerClient>().PositionChange = true;

            characterClient.transform.position = new Vector3(rayHit.point.x, 0.0f, rayHit.point.z);
        }       

        print("what" + characterClient.transform.position);

        DestroyObject(sphere);

        gameObjectList.Clear();

        // Return to the Movement Phase
        phase = Phase.Movement;
    }

    void OnTriggerEnter(Collider collider) {

        if (phase == Phase.Movement || phase == Phase.Refinement || phase == Phase.DoubleSelection) {
            
            //Debug.Log("Enter :" + collider.name);

            if(collider.GetComponent<SelectionScript>() != null)
                collider.GetComponent<SelectionScript>().ActivateSelectionObject();

            if (collider.gameObject.layer == LayerMask.NameToLayer("Scene")) {

                // Add the GameOject to the list of Objects
                if (!gameObjectList.Contains(collider.gameObject))
                    gameObjectList.Add(collider.gameObject);
            }            
        }
    }

    void OnTriggerExit(Collider collider) {

        if (phase == Phase.Movement || phase == Phase.Refinement || phase == Phase.DoubleSelection) {

            //Debug.Log("Exit :" + collider.name);

            if (collider.GetComponent<SelectionScript>() != null)
                collider.GetComponent<SelectionScript>().DeactivateSelectionObject();

            if (collider.gameObject.layer == LayerMask.NameToLayer("Scene")) {

                // Remove the GameOject from the list of Objects
                gameObjectList.Remove(collider.gameObject);
            }
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
