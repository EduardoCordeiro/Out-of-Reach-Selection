using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Server : MonoBehaviour {

	// Server info
	private int defaultPort;

	private int connectionNumber = 1; // We only work with 1 client
	private List<NetworkPlayer> connectionList;

	// GUI
	public Text serverInfoText;
	public Text clientInfoText;
	public InputField serverPortInput;

	public Button initializeServerButton;
	public Button stopServerButton;

	// Getters
	public List<NetworkPlayer> GetConnectionList() {

		return connectionList;
	}

	// Use this for initialization
	void Start() { 

		connectionList = new List<NetworkPlayer>();
	}
	
	// Update is called once per frame
	void Update() { /***/}

	// Initialize server
	public void InitializeServer() {

		// Failed to parse port number 
		if (!int.TryParse (serverPortInput.text, out defaultPort)) {
			serverPortInput.text = "Invalid port format";
		} else {
			serverInfoText.text = "Initializing server...";
			bool useNat = !Network.HavePublicAddress();
			Network.InitializeServer(connectionNumber, defaultPort, useNat);
		}
	}

	// Disconnect server
	public void StopServer() {

		Network.Disconnect();
	}

	// Print connection list
	public void PrintConnectionList() {

		clientInfoText.text = "Clients connected " + "[" + connectionList.Count + "]:\n";

		foreach (NetworkPlayer player in connectionList)
			clientInfoText.text = clientInfoText.text + player.ipAddress + ":" + player.port + "\n";
	}
		
	// Callbacks
	public void OnServerInitialized() {
		
		serverInfoText.text = "Running...\n" + Network.player.ipAddress.ToString() + ":" + defaultPort;

		// Change button context
		initializeServerButton.interactable = false;
		stopServerButton.interactable = true;
	}
		
	public void OnDisconnectedFromServer(NetworkDisconnection info) {
		
		serverInfoText.text = "Disconnected...";
		connectionList.Clear();
		PrintConnectionList();

		// Change button context
		initializeServerButton.interactable = true;
		stopServerButton.interactable = false;
	}
		
	public void OnPlayerConnected(NetworkPlayer player) {
		
		connectionList.Add(player);
		PrintConnectionList();
	}

	public void OnPlayerDisconnected(NetworkPlayer player) {
		
		connectionList.Remove(player);
		PrintConnectionList();
	}
}