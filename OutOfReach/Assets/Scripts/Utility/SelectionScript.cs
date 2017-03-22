using UnityEngine;
using System.Collections;

public class SelectionScript : MonoBehaviour {

    private GameObject selectionObject;

    private Vector3 originalPosition;

    void Awake() {

        // parent
        originalPosition = transform.parent.position;

        selectionObject = Instantiate(Resources.Load("Prefabs/Selection", typeof(GameObject))) as GameObject;

        selectionObject.name = "Selection" + this.name;
        selectionObject.transform.position = GetComponent<Collider>().bounds.center;
        selectionObject.transform.localScale = GetComponent<Collider>().bounds.size * 1.1f;

        selectionObject.SetActive(false);
    }

    void Update() {

        selectionObject.transform.position = GetComponent<Collider>().bounds.center;
    }

    public void ActivateSelectionObject() {

        selectionObject.SetActive(true);
    }

    public void DeactivateSelectionObject() {

        if(selectionObject.activeInHierarchy == true)
            selectionObject.SetActive(false);
    }

    public void ResetPosition() {

        // check to use parent of child
        transform.parent.transform.position = originalPosition;
    }

    public void AdjustCactusPosition(Vector3 position) {

        originalPosition = position;
    }
}
