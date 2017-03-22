using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;

public class BodyUDPListener : MonoBehaviour {

	public static string NoneMessage = "0";

	public int port;
	private IPEndPoint _anyIP;

    private UdpClient _udpClient = null;
    private List<string> _stringsToParse;

    void Start() {

		UDPRestart();
	}

	public void UDPRestart() {

        if (_udpClient != null)
            _udpClient.Close();
        
        _stringsToParse = new List<string>();
		_anyIP = new IPEndPoint(IPAddress.Any, port);

		_udpClient = new UdpClient(_anyIP);
        _udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);

		Debug.Log("[BodyUDPListener] Receiving body data in port: " + port);
    }

    public void ReceiveCallback(IAsyncResult ar) {

        Byte[] receiveBytes = _udpClient.EndReceive(ar, ref _anyIP);
        _stringsToParse.Add(Encoding.ASCII.GetString(receiveBytes));

        _udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);
    }

    void Update() {

        while (_stringsToParse.Count > 0) {

            string stringToParse = _stringsToParse[_stringsToParse.Count - 1];
            _stringsToParse.Clear();

            List<Body> bodies = new List<Body>();

            if (stringToParse != null && stringToParse.Length != 1) {

                int n = 0;

                foreach (string b in stringToParse.Split(MessageSeparators.L1)) {

                    if (n++ == 0) 
                        continue;

                    if (b != NoneMessage) 
                        bodies.Add(new Body(b));
                }
            }
            
            gameObject.GetComponent<TrackerClient>().SetNewFrame(bodies.ToArray());
        }
    }

    void OnApplicationQuit() {

        if (_udpClient != null) 
            _udpClient.Close();
    }

    void OnQuit() {

        OnApplicationQuit();
    }
}