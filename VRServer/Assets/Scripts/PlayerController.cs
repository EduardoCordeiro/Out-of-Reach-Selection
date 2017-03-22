using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System;

public class PlayerController : MonoBehaviour {

    // Types of clicks
    // 0 -> nothing being pressed
    // 1 -> Move to next checkpoint [Space]
    // 2 -> Change technique [F]
    // 3 -> Reset the Player to the starting position and Reset the techniques [Q]

    private UdpBroadcast udpBroadcast;

    private int currentTechnique;

    private DataHooks dataHooks;

    public Text textTechnique;

    void Start() {

        currentTechnique = 0;

        dataHooks = this.GetComponent<DataHooks>();

        textTechnique = GameObject.Find("Text").GetComponent<Text>();

        textTechnique.text = "Teleport";

        dataHooks.WriteLine(textTechnique.text);

        udpBroadcast = new UdpBroadcast(TrackerProperties.Instance.broadcastPort);

        loadConfig();
    }

    void Update() {

        // udp broadcast
        string stringToSend = "";

        if (Input.GetKeyDown(KeyCode.F1)) {

            stringToSend += "F1";

            this.currentTechnique = 0;

            textTechnique.text = "Teleport";

            dataHooks.WriteLine(textTechnique.text);
        }
        else if (Input.GetKeyDown(KeyCode.F2)) {

            stringToSend += "F2";

            this.currentTechnique = 1;

            textTechnique.text = "FadedTeleport";

            dataHooks.WriteLine(textTechnique.text);
        }
        else if (Input.GetKeyDown(KeyCode.F3)) {

            stringToSend += "F3";

            this.currentTechnique = 2;

            textTechnique.text = "Travel";

            dataHooks.WriteLine(textTechnique.text);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {

            stringToSend += "Space";

            string now = DateTime.Now.ToString();

            dataHooks.WriteLine(now);
        }
        else if (Input.GetKeyDown(KeyCode.Q)) {

            stringToSend += "Q";

            this.currentTechnique = 0;
			
			textTechnique.text = "Teleport";

            dataHooks.WriteLine("Reseting....");
        }     

        udpBroadcast.send(stringToSend);
    }

    private void loadConfig() {
        
        resetBroadcast();
    }

    public void resetBroadcast() {

        udpBroadcast.reset(TrackerProperties.Instance.broadcastPort);
    }
}

