using UnityEngine;
using UnityEngine.VR;
using System.Collections;

/// <summary>
/// This class is responsible for updating the caracter head 
/// rotation according to the HMD (Head Mounted Display) rotation.
/// </summary>
public class HeadCameraController : MonoBehaviour 
{
	public Transform cameraParent;
	public Transform characterHead;

    public float offset = 0.3f;

    void Start() {

        // Follow camera
		transform.parent = cameraParent.transform;
    }

    // Update is called once per frame
    void Update() {

		// Rotate head according to HMD rotation
		//characterHead.transform.localEulerAngles = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles;

        //this.transform.position = characterHead.transform.position + (transform.up * offset);

       // this.transform.position = characterHead.transform.position;
	}
}