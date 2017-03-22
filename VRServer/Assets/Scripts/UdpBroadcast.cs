﻿using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class UdpBroadcast {

    private string _address;
    private int _port;

    private IPEndPoint _remoteEndPoint;
    private UdpClient _udp;
    private int _sendRate;

    private DateTime _lastSent;

    private bool _streaming = false;

    public UdpBroadcast(int port, int sendRate = 100) {

        _lastSent = DateTime.Now;
        reset(port, sendRate);
    }

    public void reset(int port, int sendRate = 100) {

        _sendRate = sendRate;

        try {
            _port = port;

            _remoteEndPoint = new IPEndPoint(IPAddress.Parse("146.193.224.217"), _port);

            //_remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, _port);
            _udp = new UdpClient();
            _streaming = true;

            Debug.Log("[UDP Broadcast] Sending at port: " + _port);
        }
        catch (Exception e) {

            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
    }

    public void send(string line) {

        if (_streaming) {

            try {
                if (DateTime.Now > _lastSent) {

                    byte[] data = Encoding.UTF8.GetBytes(line);
                    _udp.Send(data, data.Length, _remoteEndPoint);
                    _lastSent = DateTime.Now;
                }
            }
            catch (Exception e) {

                Debug.LogError("[UDP Send] " + e.Message);
                Debug.LogError(e.StackTrace);
            }
        }
    }
}