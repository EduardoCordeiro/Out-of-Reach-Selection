using UnityEngine;
using UnityEngine.UI;

using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class UDPUnicast 
{
	private bool streaming = false;

	private UdpClient udp;
	private IPEndPoint remoteEndPoint;

	public UDPUnicast() { /***/ }

	public void Reset(string ipAddress, int port) 
	{
		try 
		{
			remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
			udp = new UdpClient();
			streaming = true;

			Debug.Log("[UDP Unicast]: Sending data to " + ipAddress + ":" + port);
		}
		catch (Exception e) 
		{
			Debug.LogError("[UDP Reset] ERROR: " + e.StackTrace);
		}
	}

	public void Send(string line) 
	{
		if (streaming) 
		{
			try 
			{
				byte[] data = Encoding.UTF8.GetBytes(line);
				udp.Send(data, data.Length, remoteEndPoint);
			}
			catch (Exception e) {

				Debug.LogError("[UDP Send] ERROR: " + e.StackTrace);
			}
		}
	}

	public void Stop()
	{
		udp.Close();
	}

	void OnApplicationQuit() 
	{
		if (udp != null) 
		{
			udp.Close();
			udp = null;
		}
	}

	void OnQuit() 
	{
		OnApplicationQuit();
	}
}