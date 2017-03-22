using UnityEngine;
using System.Collections;

public class RightHand : MonoBehaviour 
{
	private float speed = 0.025f;

	// Update is called once per frame
	void FixedUpdate() 
	{
		if (Input.GetKey(KeyCode.U)) 
		{
			this.transform.position += transform.up * speed;
			//this.transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 2, transform.position.z);
		}
		else if(Input.GetKey(KeyCode.J))
		{
			this.transform.position -= transform.up * speed;
			//this.transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * 2, transform.position.z);
		}
		else if (Input.GetKey(KeyCode.K)) 
		{
			this.transform.position += transform.right * speed;
			//this.transform.position = new Vector3(transform.position.x + Time.deltaTime * 2, transform.position.y, transform.position.z);
		}
		else if (Input.GetKey(KeyCode.H))
		{
			this.transform.position -= transform.right * speed;
			//this.transform.position = new Vector3(transform.position.x - Time.deltaTime * 2, transform.position.y, transform.position.z);
		}
		else if (Input.GetKey(KeyCode.I)) 
		{
			this.transform.position += transform.forward * speed;
			//this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime * 2);
		}
		else if (Input.GetKey(KeyCode.M))
		{
			this.transform.position -= transform.forward * speed;
			//this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * 2);
		}
	}
}