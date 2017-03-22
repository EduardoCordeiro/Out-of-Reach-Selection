using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackerProperties : MonoBehaviour {

    private static TrackerProperties _singleton;

    public int broadcastPort = 56600;

    private TrackerProperties() {

        _singleton = this;
    }

    public static TrackerProperties Instance {

        get {
            return _singleton;
        }
    }

    void Start() {
        //_singleton = this;
    }
}
