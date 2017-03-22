using UnityEngine;
using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

public class ClientController : MonoBehaviour 
{
	public int port;
	private IPEndPoint ip;

	private UdpClient udpClient;
	private List<string> stringsToParse;

	public TestController testController;

	void Start() 
	{
		UDPRestart();
	}
	
	void Update() 
	{
		while (stringsToParse.Count > 0) 
		{
			string stringToParse = stringsToParse.First();
			stringsToParse.RemoveAt(0);

			string[] split = stringToParse.Split(MessageSeparators.L1);

			if (split.Length == 2) 
			{
				string head = split[0];
				string body = split[1];

				switch (head) 
				{
				case "sw":
					testController.StartStopwatch(body);
					break;
				case "ts":
					testController.TestSwitch(body);
					break;
				default:
					Debug.LogError("[ClientController] ERROR: Invalid test.");
					break;
				}
			}		
		}
	}

	public void UDPRestart() 
	{
		stringsToParse = new List<string>();
		ip = new IPEndPoint(IPAddress.Any, port);

		if (udpClient != null) 
		{
			udpClient.Close();
		}
		udpClient = new UdpClient(ip);
		udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);

		Debug.Log("[ClientController]: Receiving test data in port: " + port);
	}

	public void ReceiveCallback(IAsyncResult ar) 
	{
		Byte[] receiveBytes = udpClient.EndReceive(ar, ref ip);
		stringsToParse.Add(Encoding.ASCII.GetString(receiveBytes));
		udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);
	}

	void OnApplicationQuit() 
	{
		if (udpClient != null) 
		{
			udpClient.Close();
			udpClient = null;
		}
	}

	void OnQuit() 
	{
		OnApplicationQuit();
	}
}