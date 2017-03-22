using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;

public class ClicksUdpListener : MonoBehaviour {

    public int port;
    private IPEndPoint _anyIP;

    private UdpClient _udpClient;
    private List<string> _stringsToParse;

    private TravelManager travelManager;

    // Use this for initialization
    void Start() {

        travelManager = this.GetComponent<TravelManager>();

        UDPRestart();
    }

    // Update is called once per frame
    void LateUpdate() {

        while (_stringsToParse.Count > 0) {

            string stringToParse = _stringsToParse.First();
            _stringsToParse.RemoveAt(0);

            if (stringToParse != null) {

                switch (stringToParse) {

                    case "F1":      this.travelManager.ChangeTechnique(0);
                                    break;

                    case "F2":      this.travelManager.ChangeTechnique(1);
                                    break;

                    case "F3":      this.travelManager.ChangeTechnique(2);
                                    break;

                    case "Space":   this.travelManager.MoveToNextObjective();
                                    break;

                    case "Q":       this.travelManager.ResetState();
                                    break;
                }
            }
        }
    }

    public void UDPRestart() {

        _stringsToParse = new List<string>();
        _anyIP = new IPEndPoint(IPAddress.Any, port);

        if (_udpClient != null)
            _udpClient.Close();
        
        _udpClient = new UdpClient(_anyIP);
        _udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);

        Debug.Log("[HandUDPListener] Receiving click data in port: " + port);
    }

    public void ReceiveCallback(IAsyncResult ar) {

        Byte[] receiveBytes = _udpClient.EndReceive(ar, ref _anyIP);
        _stringsToParse.Add(Encoding.ASCII.GetString(receiveBytes));

        _udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);
    }

    void OnApplicationQuit() {

        if (_udpClient != null) {

            _udpClient.Close();
            _udpClient = null;
        }
    }

    void OnQuit() {

        OnApplicationQuit();
    }
}