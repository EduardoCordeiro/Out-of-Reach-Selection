using UnityEngine;
using System.Collections;

public class RightHand : MonoBehaviour {

    public GameObject sceneObject;

    public GameObject cone;

    public float speed = 0.5f; // meters per second

    private float rotationalSpeed = 10.0f; // degrees per second

	// Update is called once per frame
	void FixedUpdate() {

        // Speed increase/decrease
        if (Input.GetKey(KeyCode.UpArrow)) {
            speed += 0.01f;
            rotationalSpeed += 2.0f;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {

            speed -= 0.01f;
            rotationalSpeed -= 2.0f;

            if (speed < 0.0f) {
                speed = 0.0f;
            }

            if (rotationalSpeed < 0.0f) {
                rotationalSpeed = 0.0f;
            }
        }

        // Hand rotation
        if (Input.GetKey(KeyCode.RightArrow)) {
            this.transform.Rotate(Vector3.forward, -rotationalSpeed * Time.deltaTime, Space.Self);
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.Rotate(Vector3.forward, rotationalSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.W)) {
            this.transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.S)) {
            this.transform.Translate(-Vector3.up * speed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.D)) {
            this.transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.A)) {
            this.transform.Translate(-Vector3.right * speed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.Q)) {
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.Z)) {
            this.transform.Translate(-Vector3.forward * speed * Time.deltaTime, Space.World);
        }
	}
}