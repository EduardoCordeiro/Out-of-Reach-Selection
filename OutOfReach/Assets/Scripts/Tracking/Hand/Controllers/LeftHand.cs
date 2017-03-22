using UnityEngine;
using System.Collections;

public class LeftHand : MonoBehaviour 
{
	private float speed = 0.2f; // meters per second

    public GameObject sceneObject;

    public GameObject cone;

	void Update() 
	{
		if (Input.GetKey(KeyCode.W)) 
		{
			this.transform.Translate( Vector3.up * speed * Time.deltaTime, Space.World);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			this.transform.Translate(-Vector3.up * speed * Time.deltaTime, Space.World);
		}
		else if (Input.GetKey(KeyCode.D)) 
		{
			this.transform.Translate( Vector3.right * speed * Time.deltaTime, Space.World);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			this.transform.Translate(-Vector3.right * speed * Time.deltaTime, Space.World);
		}
		else if (Input.GetKey(KeyCode.Q)) 
		{
			this.transform.Translate( Vector3.forward * speed * Time.deltaTime, Space.World);
		}
		else if (Input.GetKey(KeyCode.Z))
		{
			this.transform.Translate(-Vector3.forward * speed * Time.deltaTime, Space.World);
		}
	}
}