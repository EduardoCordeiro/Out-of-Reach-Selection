using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerController : MonoBehaviour 
{
	// Server
	private bool isServerActive;
	private UDPUnicast udpUnicast;
	public UDPUnicast UdpUnicast { get { return this.udpUnicast; } }

	// GUI
	public Text txtServerInfo;
	public InputField inpServerIp;
	public InputField inpServerPort;

	public Button btnInitServer;

	public Button btnNextTask;
	public Button btnStartStopwatch;

	public Button btnTestPermutation;
	public Button btnTestPermutationCheck;

	// Use this for initialization
	void Start() 
	{
		isServerActive = false;
		udpUnicast = new UDPUnicast();
	}
	
	private void StartServer()
	{
		int defaultPort = 0;

		// Failed to parse port number 
		if (!int.TryParse(inpServerPort.text, out defaultPort)) 
		{
			inpServerPort.text = "Invalid";
		} 
		else 
		{
			isServerActive = true;
			udpUnicast.Reset(inpServerIp.text, defaultPort);

			// GUI
			txtServerInfo.text = "Sending data to " + inpServerIp.text + ":" + inpServerPort.text;

			inpServerIp.interactable = false;
			inpServerPort.interactable = false;

			btnTestPermutation.interactable = true;
			btnTestPermutationCheck.interactable = true;
			btnInitServer.GetComponentInChildren<Text>().text = "Stop";
		}
	}

	private void StopServer()
	{
		udpUnicast.Stop();
		isServerActive = false;

		// GUI
		txtServerInfo.text = "Disconnected...";

		inpServerIp.text = "";
		inpServerPort.text = "";

		inpServerIp.interactable = true;
		inpServerPort.interactable = true;

		btnNextTask.interactable = false;
		btnStartStopwatch.interactable = false;

		btnTestPermutation.interactable = false;
		btnTestPermutationCheck.interactable = false;

		btnInitServer.GetComponentInChildren<Text>().text = "Start";
	}

	public void ServerSwitch()
	{
		if (isServerActive) 
		{ 
			StopServer(); 
		}
		else 
		{ 
			StartServer(); 
		}
	}
}