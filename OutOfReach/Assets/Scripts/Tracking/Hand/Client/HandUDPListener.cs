using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class HandUDPListener : MonoBehaviour {

    public bool debugging;

	// Hand
	private Hand hand;
	public string handName;

	// Delay/Offset
	private Quaternion yawOffset;
	public float offsetAngleTolerance;

    // Cone Casting
    public bool coneCastingMultipleSelection;

    // Managers
    public HandManager handManager;
    private TouchManager touchManager;
    private ClickManager clickManager;
    
    public float activationDelay;

	// Network
	public int port;
	private IPEndPoint ip;

	private UdpClient udpClient;
	private List<string> stringsToParse;

	void Start() {

        UDPRestart();

        yawOffset = Quaternion.identity;

        // Choose the hand
        hand = handManager.RegisteredHands[handName];

        coneCastingMultipleSelection = false;

        touchManager = new TouchManager(activationDelay);
        clickManager = new ClickManager(activationDelay);
	}

    void Update() {

        if(debugging)
            KeyboardMouseUpdate();
        else
            HandTrackingUpdate();
    }

    private void KeyboardMouseUpdate() {

        clickManager.Reset();

        if (hand.joint.name.Equals("RightHand")) {

            if (Input.GetMouseButtonDown(0)) {

                coneCastingMultipleSelection = true;

                clickManager.ClickDown();
            }
            else if (Input.GetMouseButtonUp(0)) {

                coneCastingMultipleSelection = false;

                clickManager.ClickUp();
            }

            // Debug Button Press
            if (Input.GetKeyDown(KeyCode.C)) {

                hand.CloseHand();

                clickManager.ClickDown();
            }
            else if(Input.GetKeyUp(KeyCode.C)) {

                hand.OpenHand();

                clickManager.ClickUp();
            }
        }

        // Process click presses
        if (clickManager.GetSingleClickDown)
            hand.UpdateHandState(true);
        else if (clickManager.GetSingleClickUp)
            hand.UpdateHandState(false);
        else if (clickManager.GetDoubleClickDown) {

            print("Double Click Down");

            // Do stuff with coneCasting.
            // Select objects
            //coneCasting.imuduinoClick = true;
        }
        else if (clickManager.GetDoubleClickUp) {

            hand.UpdateHandState(false);

            print("Double Click Up");

            // Do stuff with coneCasting.
            // Select objects
            //coneCasting.imuduinoClick = false;
        }
    }

	private void HandTrackingUpdate() {

		while (stringsToParse.Count > 0) {

            touchManager.Reset();
		
            string stringToParse = stringsToParse.First();
            stringsToParse.RemoveAt(0);

            if (stringToParse != null) {

                string[] splitted = stringToParse.Split('|');

                float x = float.Parse(splitted[0]);
                float y = float.Parse(splitted[1]);
                float z = float.Parse(splitted[2]);
                float w = float.Parse(splitted[3]);
                float p = float.Parse(splitted[4]);

				ApplyYawOffset(new Quaternion(x, -z, y, w));
			
				// Pressure level 1
                // User Activates the Cone and can move/scale it around
				if (p > 800 / 1023.0f) {

                    touchManager.TouchDown();
				}
                // No pressure
                else if (p < 400 / 1023.0f) {

                    touchManager.TouchUp();
				}
				// Pressure level 2
                // User goes from Level 1 -> Level 2
				/*if (p > 1021.0f / 1023.0f) {

                    //touchManager.DoubleTouchDown();
				}*/

                // Processing the touches
                if (touchManager.GetSingleTouchDown) {

                    hand.UpdateHandState(true);

                    hand.CloseHand();
                }
                else if (touchManager.GetSingleTouchUp) {
 
                    hand.UpdateHandState(false);

                    hand.OpenHand();
                }
                /*else if (touchManager.GetDoubleTouchDown) {

                    //print("double touch down");

                    //coneCastingMultipleSelection = true;

                    //hand.UpdateHandState(false);

                    //hand.CloseHand();
                }
                else if (touchManager.GetDoubleTouchUp) {

                    //coneCastingMultipleSelection = false;

                    //hand.UpdateHandState(false);

                    //hand.OpenHand();
                }*/
			}
        }
	}

	private void ApplyYawOffset(Quaternion currentRotation) {

		// Arm direction
		Vector3 armDirection = Utility.GetBoneDirection(hand.joint.transform.position, hand.elbow.transform.position);
		Vector3 armDirectionXZ = armDirection;
		armDirectionXZ.y = 0;

		// Hand direction
		Vector3 handDirection = Vector3.zero;
		hand.joint.transform.rotation = currentRotation;

		// Hand direction adjustment
		if (handName.Equals("RightHand")) 
		    handDirection =  hand.joint.transform.right;
		else if (handName.Equals("LeftHand"))
			handDirection = -hand.joint.transform.right;
		
		Vector3 handDirectionXZ = handDirection;
		handDirectionXZ.y = 0;

		if (Vector3.Angle(armDirection, armDirectionXZ) < offsetAngleTolerance && Vector3.Angle(handDirection, handDirectionXZ) < offsetAngleTolerance) 
			yawOffset = Quaternion.FromToRotation(handDirectionXZ, armDirectionXZ);
		
		hand.joint.transform.rotation = yawOffset * hand.joint.transform.rotation;
	}

    public void UDPRestart() {

		stringsToParse = new List<string>();
		ip = new IPEndPoint(IPAddress.Any, port);

		if (udpClient != null)
			udpClient.Close();
		
		udpClient = new UdpClient(ip);
		udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);

		//Debug.Log("[HandUDPListener]: Receiving " + handName + " data in port: " + port);
	}

	public void ReceiveCallback(IAsyncResult ar) {

		Byte[] receiveBytes = udpClient.EndReceive(ar, ref ip);

		float w = System.BitConverter.ToSingle(receiveBytes, 0);
		float x = System.BitConverter.ToSingle(receiveBytes, 4);
		float y = System.BitConverter.ToSingle(receiveBytes, 8);
		float z = System.BitConverter.ToSingle(receiveBytes, 12);
		float p = System.BitConverter.ToSingle(receiveBytes, 16);

		stringsToParse.Add(x + "|" + y + "|" + z + "|" + w + "|" + p);
		udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);
	}

	void OnApplicationQuit() {

		if (udpClient != null) {

			udpClient.Close();
			udpClient = null;
		}
	}

	void OnQuit() {

		OnApplicationQuit();
	}
}