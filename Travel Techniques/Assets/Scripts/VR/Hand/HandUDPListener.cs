using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class HandUDPListener : MonoBehaviour 
{
	// Hand
	public GameObject hand;
    public float handTolerance;

	public TrackerClient trackerClient;
	public HandStateManager handStateManager;

	public int port;
	private IPEndPoint _anyIP;

	private UdpClient _udpClient;
	private List<string> _stringsToParse;

	private float yawOffset = 0.0f;

	// Timer
	public float pressureTimeLimit;
	private float pressureTimer = 0.0f;

	// Use this for initialization
	void Start() 
	{
		UDPRestart();
	}

	// Update is called once per frame
	void Update() 
	{
		while (_stringsToParse.Count > 0)
        {
            string stringToParse = _stringsToParse.First();
            _stringsToParse.RemoveAt(0);

            if (stringToParse != null)
            {
                string[] splitted = stringToParse.Split('|');

                float x = float.Parse(splitted[0]);
                float y = float.Parse(splitted[1]);
                float z = float.Parse(splitted[2]);
                float w = float.Parse(splitted[3]);
                float p = float.Parse(splitted[4]);

                Quaternion currentRotation = new Quaternion(x, -z, y, w);

				ResetYawOffset(currentRotation, hand.transform.localRotation);
				hand.transform.localRotation = currentRotation;

				//  Pressure level 1
                if (p > 800 / 1023.0f)
                {
					pressureTimer += Time.deltaTime;

					// Time limit exceeded, apply pressure level 1
					if (pressureTimer > pressureTimeLimit) 
					{
						handStateManager.UpdateHandState(hand.name, true);
					}
                }
                else if (p < 400 / 1023.0f) // No pressure
                {
					handStateManager.UpdateHandState(hand.name, false);
                }

				// Pressure level 2
				/*if (p > 1022 / 1023.0f)
				{
					// Quick press allows for pressure level 2
					if (pressureTimer < pressureTimeLimit) 
					{
						pressureTimer = 0.0f;

						if (handStateManager.EngagedHands.Count >= 1) 
						{
							sceneObjectsManager.AssignWidgetToObject(handStateManager.EngagedHands[0].InsideObject);	
						}
						//obj.localScale = new Vector3(3, 3, 3);
					}
				}
				else if (p < 975 / 1023.0f) // No pressure
				{
					//obj.localScale = new Vector3(1, 1, 1);
				}*/
            }
        }
    }

	private void ResetYawOffset(Quaternion currentRotation, Quaternion handRotation)
    {
        Body currentTrackedBody = null;

		// Check human existence
        if (trackerClient.TrackedHuman != null)
        {
			Vector3 armDirection = Vector3.zero;
			currentTrackedBody = trackerClient.TrackedHuman.body;

			// Get arm direction
            if (hand.name.Equals("LeftHand"))
            {
				armDirection = Utility.GetBoneDirection(currentTrackedBody.Joints[BodyJointType.leftHand], currentTrackedBody.Joints[BodyJointType.leftElbow]);
            }
            else if (hand.name.Equals("RightHand"))
            {
                armDirection = Utility.GetBoneDirection(currentTrackedBody.Joints[BodyJointType.rightHand], currentTrackedBody.Joints[BodyJointType.rightElbow]);
			}

			// Calculate the angle between the world forward and the arm direction 2D
			armDirection.y = 0.0f;
			yawOffset = Vector3.Angle(armDirection, Vector3.forward);
        }
    }

    public void UDPRestart()
	{
		_stringsToParse = new List<string>();
		_anyIP = new IPEndPoint(IPAddress.Any, port);

		if (_udpClient != null) 
		{
			_udpClient.Close();
		}
		_udpClient = new UdpClient(_anyIP);
		_udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);

		Debug.Log("[HandUDPListener] Receiving " + hand.name + " data in port: " + port);
	}

	public void ReceiveCallback(IAsyncResult ar)
	{
		Byte[] receiveBytes = _udpClient.EndReceive(ar, ref _anyIP);

		float w = System.BitConverter.ToSingle(receiveBytes, 0);
		float x = System.BitConverter.ToSingle(receiveBytes, 4);
		float y = System.BitConverter.ToSingle(receiveBytes, 8);
		float z = System.BitConverter.ToSingle(receiveBytes, 12);
		float p = System.BitConverter.ToSingle(receiveBytes, 16);

		_stringsToParse.Add(x + "|" + y + "|" + z + "|" + w + "|" + p);
		_udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);
	}

	void OnApplicationQuit()
	{
		if (_udpClient != null)
		{
			_udpClient.Close();
			_udpClient = null;
		}
	}

	void OnQuit()
	{
		OnApplicationQuit();
	}
}