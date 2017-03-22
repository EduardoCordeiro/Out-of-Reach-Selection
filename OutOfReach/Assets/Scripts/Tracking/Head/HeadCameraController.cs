using UnityEngine;
using System.Collections;

public class HeadCameraController : MonoBehaviour {

    public Transform cameraParent;

    void LateUpdate() {

        this.transform.position = cameraParent.transform.position;
    }
}