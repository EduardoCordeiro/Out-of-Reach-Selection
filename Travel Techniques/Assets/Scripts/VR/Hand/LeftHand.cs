using UnityEngine;
using System.Collections;

public class LeftHand : MonoBehaviour 
{
	private float speed = 0.025f;

	// Update is called once per frame
	void FixedUpdate() 
	{
		if (Input.GetKey(KeyCode.W)) 
		{
			this.transform.position += transform.up * speed;
			//this.transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 2, transform.position.z);
		}
		else if(Input.GetKey(KeyCode.S))
		{
			this.transform.position -= transform.up * speed;
			//this.transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * 2, transform.position.z);
		}
		else if (Input.GetKey(KeyCode.D)) 
		{
			this.transform.position += transform.right * speed;
			//this.transform.position = new Vector3(transform.position.x + Time.deltaTime * 2, transform.position.y, transform.position.z);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			this.transform.position -= transform.right * speed;
			//this.transform.position = new Vector3(transform.position.x - Time.deltaTime * 2, transform.position.y, transform.position.z);
		}
		else if (Input.GetKey(KeyCode.Q)) 
		{
			this.transform.position += transform.forward * speed;
			//this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime * 2);
		}
		else if (Input.GetKey(KeyCode.Z))
		{
			this.transform.position -= transform.forward * speed;
			//this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * 2);
		}
	}
}