using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

    // Player Camera
    public Camera playerCamera;

    // Global variable for the start of Time
    private float startTime;

    // Duration of the Zoom
    private float duration = 2.0f;

	// Use this for initialization
	void Start () {

        playerCamera = this.transform.GetComponentInChildren<Camera>();

        playerCamera.fieldOfView = Constants.defaultFoV;

        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        // Control of the player [Debug purposes only]
        Vector3 newPosition = this.transform.position;

        // Right and Left movement
        if (Input.GetKey(KeyCode.LeftArrow))
            newPosition.x -= 0.05f;
        else if (Input.GetKey(KeyCode.RightArrow))
            newPosition.x += 0.05f;

        // Front and back movement
        if (Input.GetKey(KeyCode.UpArrow))
            newPosition.z += 0.05f;
        else if (Input.GetKey(KeyCode.DownArrow))
            newPosition.z -= 0.05f;

        this.transform.position = newPosition;
	}

    public void ZoomIn(Vector3 closestObjectPosition) {

        StartCoroutine("DecreaseFoV");
    }

    public void ZoomOut() {

        StartCoroutine("IncreaseFoV");
    }

    IEnumerator DecreaseFoV(){

        float i = 0.0f;
        float t = (Time.time - startTime) / duration;

        float currentFoV = playerCamera.fieldOfView;

        float newFoV = 25.0f;

        while (i < 1.0f) {

            i += Time.deltaTime * t;

            playerCamera.fieldOfView = Mathf.SmoothStep(currentFoV, newFoV, i);

            yield return null; 
        }
    }

    IEnumerator IncreaseFoV() {

        float i = 0.0f;
        float t = (Time.time - startTime) / duration;

        float currentFoV = playerCamera.fieldOfView;

        while (i < 1.0f) {

            i += Time.deltaTime * t;

            playerCamera.fieldOfView = Mathf.SmoothStep(currentFoV, Constants.defaultFoV, i);

            yield return null;
        }
    }
}
