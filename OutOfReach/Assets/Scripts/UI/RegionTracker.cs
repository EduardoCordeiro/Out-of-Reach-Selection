using UnityEngine;
using System.Collections;

public class RegionTracker : MonoBehaviour {

    public GameObject stretchUI;

    public GameObject cone;

    public StretchGoGo stretchGoGo;

    public ConeCasting coneCasting;

    private static float outerRegionPosition = 0.3f;

    private static float middleRegionPosition = 0.0f;

    private static float innerRegionPosition = -0.3f;
	
	// Update is called once per frame
	void Update () {

        if (this.gameObject.activeInHierarchy) {

            float newY = 0.0f;

            // change the Y position based on cenas da regiao
            if (stretchGoGo.enabled) {

                newY = RefreshPosition("GoGo"); //print("gogo");
            }
            else if (cone.GetComponent<MeshRenderer>().enabled) {

                newY = RefreshPosition("Precious"); //print("precious");
            }
            else {

                ToggleRenderers(false);
            }

            ArrowPosition(newY);
        } 
	}

    private float RefreshPosition(string technique) {

        ToggleRenderers(true);

        string activeRegion = "";

        if (technique == "GoGo")
            activeRegion = stretchGoGo.activeRegion;
        else if (technique == "Precious")
            activeRegion = coneCasting.activeRegion;

        switch (activeRegion) {

            case "inner":

                return innerRegionPosition;

            case "middle":

                return middleRegionPosition;

            case "outer":

                return outerRegionPosition;

            default:

                Debug.Log("String is null");
                break;
        }

        return 0.0f;
    }

    private void ArrowPosition(float newY) {

        if (!float.IsNaN(transform.position.x) && !float.IsNaN(transform.position.y) && !float.IsNaN(transform.position.z)) {

            Vector3 newPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

            transform.localPosition = newPosition;
        }
    }

    private void ToggleRenderers(bool value) {

        stretchUI.GetComponent<MeshRenderer>().enabled = value;

        this.GetComponent<MeshRenderer>().enabled = value;
    }
}
