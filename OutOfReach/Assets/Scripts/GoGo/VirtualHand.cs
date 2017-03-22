using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class VirtualHand : MonoBehaviour {

    public StretchGoGo stretchGoGo;

    public GameObject hand;

    public void Update() {

        transform.forward = -hand.transform.right;
    }

    void OnTriggerEnter(Collider collider) {

        if (collider.gameObject.layer == LayerMask.NameToLayer("Scene")) {

            collider.GetComponent<SelectionScript>().ActivateSelectionObject();

            stretchGoGo.AddObjectToList(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider) {

        if (collider.gameObject.layer == LayerMask.NameToLayer("Scene")) {

            collider.GetComponent<SelectionScript>().DeactivateSelectionObject();

            stretchGoGo.RemoveObjectFromList(collider.gameObject);
        }
    }
}
