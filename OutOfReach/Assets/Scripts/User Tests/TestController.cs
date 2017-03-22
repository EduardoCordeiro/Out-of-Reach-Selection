using UnityEngine;
using UnityEngine.VR;
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

public class TestController : MonoBehaviour 
{
	// Flags
	private bool isHeader;
	private bool isTrackingObject;

	// Stopwatches
	private Stopwatch taskDuration;

	private UserTestID currentTest;
	private UserTaskID currentTask;

	// Object data
    public int incorrectObjectSelection;

	public GameObject targetObject;
	private GameObject currentObject;

	public GameObject testObject;
	public List<Transform> testTransforms;

	// Managers/Handlers
	public HandManager handManager;

    // Character Client
    public GameObject characterClient;

    public ConeCasting coneCasting;

    public StretchGoGo stretchGoGo;

    public GameObject Cone;

    public GameObject virtualHand;

    public Flashlight flashlight;

    public GameObject flashLightCone;

	void Start() 
	{
		isHeader = false;
		isTrackingObject = false;

		taskDuration = new Stopwatch();
	}

	void Update() 
	{
		if (currentObject != null && isTrackingObject) 
		{
			WriteFrameDataByTask();

			if (isHeader) 
			{
				isHeader = false;
			}
		}
	}

	public void StartStopwatch(string pdu)
	{
		switch (pdu)
		{
		case "start":
			taskDuration.Start();
			isHeader = true;
			isTrackingObject = true;
			break;
		case "stop":
			isTrackingObject = false;

			taskDuration.Stop();

			WriteDataByTask();

			taskDuration.Reset();

			break;
		default:
			UnityEngine.Debug.LogError("[TestController] ERROR: Invalid stopwatch operation.");
			break;
		}
	}

	public void TestSwitch(string pdu)
	{
		string[] split = pdu.Split(MessageSeparators.SET);

		switch (split[0])
		{
		case "tsover":
			break;
		case "tstnext":
			currentTest = (UserTestID)int.Parse(split[1]);
			currentTask = (UserTaskID)int.Parse(split[2]);
			Setup(currentTest, currentTask);
			break;
		case "tsknext":
			currentTask = (UserTaskID)int.Parse(split[1]);
			Setup(currentTest, currentTask);
			break;
		default:
			UnityEngine.Debug.LogError("[TestController] ERROR: Invalid test switch.");
			break;
		}
	}
		
	private void Setup(UserTestID currentTest, UserTaskID currentTask)
	{
		// Deactivate past object
		if (currentObject != null) 
            currentObject.SetActive(false);

		targetObject.SetActive(true);

		try 
		{
			switch (currentTest)
			{
			case UserTestID.Precious:

                    SwitchTechnique("Precious");

                    ResetPlayerPosition();

				    currentObject = GetCurrentObject("Precious");

                    currentObject.SetActive(true);

                    incorrectObjectSelection = 0;

                    coneCasting.DeleteCorrectSelection();

				    break;
			case UserTestID.GoGo:

                    SwitchTechnique("GoGo");

                    ResetPlayerPosition();

				    currentObject = GetCurrentObject("GoGo");

                    currentObject.SetActive(true);

                    incorrectObjectSelection = 0;

                    stretchGoGo.DeleteCorrectSelection();

                    virtualHand.transform.position = handManager.RegisteredHands["RightHand"].transform.position;
                    stretchGoGo.ResetHandPosition();

				    break;

            case UserTestID.Flashlight:

                    SwitchTechnique("Flashlight");

                    ResetPlayerPosition();

                    currentObject = GetCurrentObject("Flashlight");

                    currentObject.SetActive(true);

                    incorrectObjectSelection = 0;

                    flashlight.DeleteCorrectSelection();

                    break;
			default:
				    UnityEngine.Debug.LogError("[TestController] ERROR: Invalid test.");
				    break;
			}

			// Setup task transform
			Transform initialTransform = testTransforms[(int)currentTask];

			currentObject.transform.position = initialTransform.position;
			currentObject.transform.rotation = initialTransform.rotation;

            targetObject.transform.GetChild(0).GetComponent<SelectionScript>().AdjustCactusPosition(initialTransform.position);
		}
		catch (Exception e)
		{
			UnityEngine.Debug.LogError("[TestController] ERROR: Invalid object.");
			UnityEngine.Debug.LogError("[TestController] ERROR: " + e.Message);
		}
	}

    private void SwitchTechnique(string technique) {

        if (technique == "Precious") {

            Cone.SetActive(true);

            characterClient.GetComponent<StretchGoGo>().enabled = false;

            virtualHand.SetActive(false);

            flashLightCone.SetActive(false);
        }
        else if (technique == "GoGo") {

            Cone.SetActive(false);

            characterClient.GetComponent<StretchGoGo>().enabled = true;

            virtualHand.SetActive(true);
            virtualHand.transform.position = handManager.RegisteredHands["RightHand"].transform.position;

            flashLightCone.SetActive(false);
        }
        else if (technique == "Flashlight") {

            Cone.SetActive(false);

            characterClient.GetComponent<StretchGoGo>().enabled = false;

            virtualHand.SetActive(false);

            flashLightCone.SetActive(true);
        }
    }

    private void ResetPlayerPosition() {

        this.characterClient.GetComponent<TrackerClient>().PositionChange = true;

        this.characterClient.GetComponent<TrackerClient>().TeleportPosition = new Vector3(0.0f, 0.0f, 0.0f);

        characterClient.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        characterClient.transform.rotation = Quaternion.identity;
    }

	private GameObject GetCurrentObject(string tag)
	{
        if (currentTask == UserTaskID.FreeRoam) {

            targetObject.SetActive(false);
            return testObject;
        }
        else {

            testObject.SetActive(false);

            return targetObject;
        }
	}

	private void WriteDataByTask()
	{
		if (currentObject != null) 
		{
			FileStream fs = File.Open(Application.persistentDataPath + "/" + currentTest.ToString () + "_" + currentTask.ToString () + "_global.csv", FileMode.Create);
			StreamWriter sw = new StreamWriter (fs);

			string dataDescription = "";
			string data = "";

			// Task duration
			dataDescription += "Task Duration,";
			data += taskDuration.Elapsed.ToString() + ",";

			// Target position
			dataDescription += "Target Position,";
			data += "\"(" + targetObject.transform.position.x + "," + targetObject.transform.position.y + "," + targetObject.transform.position.z + ")\",";

			// Reached position
			dataDescription += "Reached Position,";
			data += "\"(" + currentObject.transform.position.x + "," + currentObject.transform.position.y + "," + currentObject.transform.position.z + ")\",";

			// Position error (Magnitude)
			dataDescription += "Position error (Magnitude),";
			data += Vector3.Distance(currentObject.transform.position, targetObject.transform.position).ToString() + ",";

			// Position error (Vector)
			dataDescription += "Position error (Vector),";
			Vector3 vectorDistance = (currentObject.transform.position - targetObject.transform.position);
			data += "\"(" + vectorDistance.x + "," + vectorDistance.y + "," + vectorDistance.z + ")\",";

			// Target rotation
			dataDescription += "Target Rotation,";
			data += "\"(" + targetObject.transform.rotation.x + "," + targetObject.transform.rotation.y + "," + targetObject.transform.rotation.z + "," + targetObject.transform.rotation.w + ")\",";

			// Reached rotation
			dataDescription += "Reached Rotation,";
			data += "\"(" + currentObject.transform.rotation.x + "," + currentObject.transform.rotation.y + "," + currentObject.transform.rotation.z + "," + currentObject.transform.rotation.w + ")\",";

			// Rotation error (Angle)
			dataDescription += "Rotation Error (Angle),";
			data += Quaternion.Angle(currentObject.transform.rotation, targetObject.transform.rotation).ToString() + ",";

            // Incorrect number of Objects
            dataDescription += "Incorrect Selections (Int)";
            data += incorrectObjectSelection.ToString();
		
			sw.WriteLine(dataDescription);
			sw.WriteLine(data);
			sw.Flush();
		 
			fs.Close();
		}
	}

	private void WriteFrameDataByTask()
	{
		FileStream fs = File.Open(Application.persistentDataPath + "/" + currentTest.ToString() + "_" + currentTask.ToString() + "_frame.csv", FileMode.Append);
		StreamWriter sw = new StreamWriter(fs);

		string dataDescription = "";
		string data = "";

		// Timestamp
		dataDescription += "Timestamp,";
		data += taskDuration.Elapsed.ToString() + ",";

		GameObject leftHandCenter = handManager.RegisteredHands["LeftHand"].center;

		// Left hand position
		dataDescription += "Left Hand Position,";
		data += "\"(" + leftHandCenter.transform.position.x + "," + leftHandCenter.transform.position.y + "," + leftHandCenter.transform.position.z + ")\",";

		// Left hand rotation
		dataDescription += "Left Hand Rotation,";
		data += "\"(" + leftHandCenter.transform.rotation.x + "," + leftHandCenter.transform.rotation.y + "," + leftHandCenter.transform.rotation.z + "," + leftHandCenter.transform.rotation.w + ")\",";

		GameObject rightHandCenter = handManager.RegisteredHands["RightHand"].center;

		// Right hand position
		dataDescription += "Right Hand Position,";
		data += "\"(" + rightHandCenter.transform.position.x + "," + rightHandCenter.transform.position.y + "," + rightHandCenter.transform.position.z + ")\",";

		// Right hand rotation
		dataDescription += "Right Hand Rotation,";
		data += "\"(" + rightHandCenter.transform.rotation.x + "," + rightHandCenter.transform.rotation.y + "," + rightHandCenter.transform.rotation.z + "," + rightHandCenter.transform.rotation.w + ")\",";

		// Current object position
		dataDescription += "Object Position,";
		data += "\"(" + currentObject.transform.position.x + "," + currentObject.transform.position.y + "," + currentObject.transform.position.z + ")\",";

		// Current object rotation
		dataDescription += "Object Rotation,";
		data += "\"(" + currentObject.transform.rotation.x + "," + currentObject.transform.rotation.y + "," + currentObject.transform.rotation.z + "," + currentObject.transform.rotation.w + ")\",";

		// Left hand engaged/disengaged
		dataDescription += "Left Hand Engaged,";
		data += handManager.RegisteredHands["LeftHand"].IsEngaged.ToString() + ",";

		// Right hand engaged/disengaged
		dataDescription += "Right Hand Engaged,";
		data += handManager.RegisteredHands["RightHand"].IsEngaged.ToString() + ",";

		// Head position
		dataDescription += "Head Position,";
		Vector3 headPosition = InputTracking.GetLocalPosition(VRNode.Head);
		data += "\"(" + headPosition.x + "," + headPosition.y + "," + headPosition.z + ")\",";

		// Head rotation
		dataDescription += "Head Rotation";
		Quaternion headRotation = InputTracking.GetLocalRotation(VRNode.Head);
		data += "\"(" + headRotation.x + "," + headRotation.y + "," + headRotation.z + "," + headRotation.w + ")\"";

		if (isHeader) 
		{
			sw.WriteLine(dataDescription);
		}
		sw.WriteLine(data);
		sw.Flush();

		fs.Close();
	}
}