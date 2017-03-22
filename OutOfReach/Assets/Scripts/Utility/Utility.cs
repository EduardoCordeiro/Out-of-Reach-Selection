using UnityEngine;
using System.Collections;

public class Utility {

    public static Vector3 RandomCircle(Vector3 center, float radius, int angle) {

        Vector3 position;

        position.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        position.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        position.z = center.z;

        return position;
    }

    public static void ChangeObjectOpacity(GameObject objectInScene, float alphaValue) {

        Color newColor = objectInScene.GetComponent<Renderer>().material.color;
        newColor.a = alphaValue;

        objectInScene.GetComponent<Renderer>().material.color = newColor;
    }

    public static void colliderInteraction(GameObject objectInScene, bool value) {

        objectInScene.GetComponent<Collider>().enabled = value;
    }

    public Vector3 ConeMovement(Transform transform) {

        // Control of the cone
        Vector3 movementPosition = transform.position;

        // Right and Left movement
        if (Input.GetKey(KeyCode.A))
            movementPosition.x -= 0.1f;
        else if (Input.GetKey(KeyCode.D))
            movementPosition.x += 0.1f;

        // Up and Down movement
        if (Input.GetKey(KeyCode.W))
            movementPosition.y += 0.1f;
        else if (Input.GetKey(KeyCode.S))
            movementPosition.y -= 0.1f;

        // Front and back movement
        if (Input.GetKey(KeyCode.R))
            movementPosition.z += 0.1f;
        else if (Input.GetKey(KeyCode.F))
            movementPosition.z -= 0.1f;

        return movementPosition;
    }

    /*public void RayCasting() {

        // Simple Ray Casting
        // Bit shift the index of the layer (9, Scene) to get a bit mask
        int layerMask = 1 << 9;

        RaycastHit hit;
        // Does the ray intersect any objects in the Scene Layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)) {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }*/

    /*void multipleCasting() {

        // Bit shift the index of the layer (9, Scene) to get a bit mask
        int layerMask = 1 << Constants.sceneMask;

        // Center Ray
        Vector3 centerRayDirection = this.centerRay.transform.position - rayDirectionObject.transform.position;

        RaycastHit centerRayHit;

        if (Physics.Raycast(centerRay.transform.position, centerRayDirection, out centerRayHit, Mathf.Infinity, layerMask)) {

            if(this.DrawDebugRays)
                Debug.DrawRay(centerRay.transform.position, centerRayDirection * centerRayHit.distance, Color.yellow);

            if (!this.gameObjectList.Contains(centerRayHit.collider.gameObject))
                this.gameObjectList.Add(centerRayHit.collider.gameObject);
        }
        else {

            if(this.DrawDebugRays)
                Debug.DrawRay(centerRay.transform.position, centerRayDirection * 2000, Color.red);
        }

        // Outer Circle Rays
        RaycastHit[] outerCircleRayHits = new RaycastHit[numberOfRays];

        // Determine the rotation of the ray for Inner and Outer Circle
        Quaternion quaternion = Quaternion.AngleAxis(Constants.coneStartingAngle, activeHand.transform.TransformDirection(Vector3.forward));

        for (int i = 0; i < numberOfRays; i++) {

            // Rotation of the Outer rays
            outerCircleRays[i].transform.rotation = quaternion * outerCircleRays[i].transform.rotation;

            // Direction of the rays for the Outer Circle
            Vector3 outerCircleRayDirection = outerCircleRays[i].transform.position - rayDirectionObject.transform.position;
            
            // Does the ray intersect any objects in the Scene Layer
            if (Physics.Raycast(outerCircleRays[i].transform.position, outerCircleRayDirection, out outerCircleRayHits[i], Mathf.Infinity, layerMask)) {

                // This 2 here is a TODO [Rays it the object but the debug rays dont show the correct distance]
                if(this.DrawDebugRays)
                    Debug.DrawRay(outerCircleRays[i].transform.position, outerCircleRayDirection * outerCircleRayHits[i].distance * 1.5f, Color.green);

                if (!this.gameObjectList.Contains(outerCircleRayHits[i].collider.gameObject))
                    this.gameObjectList.Add(outerCircleRayHits[i].collider.gameObject);
            }
            else {
                if(this.DrawDebugRays)
                    Debug.DrawRay(outerCircleRays[i].transform.position, outerCircleRayDirection * 2000, Color.red);
            }

            // Inner Circle Rays
            RaycastHit[] innerCircleRayHits = new RaycastHit[numberOfRays];

            // Rotation of the Inner rays
            innerCircleRays[i].transform.rotation = quaternion * innerCircleRays[i].transform.rotation;

            // Direction of the rays for the Inner Circle
            Vector3 innerCircleRayDirection = innerCircleRays[i].transform.position - rayDirectionObject.transform.position;

            // Does the ray intersect any objects in the Scene Layer
            if (Physics.Raycast(innerCircleRays[i].transform.position, innerCircleRayDirection, out innerCircleRayHits[i], Mathf.Infinity, layerMask)) {

                if(this.DrawDebugRays)
                    Debug.DrawRay(innerCircleRays[i].transform.position, innerCircleRayDirection * innerCircleRayHits[i].distance, Color.green);

                if (!this.gameObjectList.Contains(innerCircleRayHits[i].collider.gameObject))
                    this.gameObjectList.Add(innerCircleRayHits[i].collider.gameObject);
            }
            else {
                if(this.DrawDebugRays)
                    Debug.DrawRay(innerCircleRays[i].transform.position, innerCircleRayDirection * 2000, Color.green);
            }
        }
    }*/

    /*GameObject[] createStartingRays(Vector3 startingPosition, int numberOfRays, float radius, string circle, int raysPerAngle) {

        // Define the Angle of Aperture of the Cone
        rayDirectionObject.transform.position = startingPosition - new Vector3(0.0f, 0.0f, Constants.defaultConeApperture);
        rayDirectionObject.transform.parent = activeHand.transform;

        GameObject[] startingRays = new GameObject[numberOfRays];

        for (int i = 0; i < numberOfRays; i++) {

            int angle = i * raysPerAngle;

            Vector3 rayPosition = Utility.RandomCircle(startingPosition, radius, angle);

            startingRays[i] = new GameObject(circle + " Ray " + i.ToString());
            startingRays[i].transform.position = rayPosition;
            startingRays[i].transform.parent = activeHand.transform;
        }

        return startingRays;
    }*/

    /*void UpdateSelectionPhaseZoom() {

        // Distances Dictionary
        Dictionary<GameObject, float> distances = new Dictionary<GameObject, float>();

        foreach (GameObject actualObject in this.gameObjectList)
            distances.Add(actualObject, Vector3.Distance(carl.transform.position, actualObject.transform.position));

        GameObject closestObject = null;
        float minDistance = Mathf.Infinity;

        // Find out which object is closest to the user
        foreach (KeyValuePair<GameObject, float> distance in distances) {

            if (distance.Value < minDistance) {

                minDistance = distance.Value;
                closestObject = distance.Key;
            }

            // Revert the color change back to the normal color
            distance.Key.GetComponent<Renderer>().material = distance.Key.GetComponent<ObjectScript>().startingMaterial;
        }

        // Perform calculations regarding the size of the object [TODO]


        // Adjusment made so Carl isn't on top of the object [check above comment]
        Vector3 closestObjectPosition = closestObject.transform.position - new Vector3(0.0f, closestObject.transform.position.y, 2.0f);

        // Change the opacity of all objects in the scene except the Chosen Objects
        foreach (GameObject sceneObject in sceneObjects)
            if (!distances.ContainsKey(sceneObject))
                Utility.ChangeObjectOpacity(sceneObject, 0.3f);        

        // Return to the Movement Phase
        this.phase = Constants.MOVEMENT_PHASE;
    }*/
	
	// Human body functions
    public static Vector3 GetBoneDirection(Vector3 joint, Vector3 parentJoint) {

        return (joint - parentJoint).normalized;
    }

    public static Quaternion GetQuaternionFromRightUp(Vector3 right, Vector3 up) {

        Vector3 forward = Vector3.Cross(right, up);
        return Quaternion.LookRotation(forward, Vector3.Cross(forward, right));
    }

    public static Quaternion GetQuaternionFromUpRight(Vector3 up, Vector3 right) {

        Vector3 forward = Vector3.Cross(right, up);
        return Quaternion.LookRotation(forward, up);
    }

    public static Quaternion GetQuaternionFromForwardUp(Vector3 forward, Vector3 up) {

        Vector3 right = Vector3.Cross(up, forward);
        return Quaternion.LookRotation(forward, Vector3.Cross(forward, right));
    }

    public static Quaternion GetQuaternionFromRightForward(Vector3 right, Vector3 forward) {

        Vector3 up = Vector3.Cross(forward, right);
        return Quaternion.LookRotation(Vector3.Cross(right, up), up);
    }
}
