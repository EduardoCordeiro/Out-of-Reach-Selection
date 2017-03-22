using UnityEngine;
using System.Collections;

public class TravelManager : MonoBehaviour {

    // 0
    public Teleport teleport;

    // 1
    public FadedTeleport fadedTeleport;

    // 2
    public Travel travel;

    private int currentTechnique;

    private Vector3 inicialPlayerPosition;

    private Quaternion inicialPlayerRotation;

	// Use this for initialization
	void Start () {

        this.setTechnique(0);

        this.inicialPlayerPosition = this.transform.position;

        this.inicialPlayerRotation = this.transform.rotation;
	}

    public void ChangeTechnique(int technique) {

        this.currentTechnique = technique;

        string stringTechnique = "";

        if (this.currentTechnique == 0)
            stringTechnique = "Teleport";
        else if (currentTechnique == 1)
            stringTechnique = "FadedTeleport";
        else if (currentTechnique == 2)
            stringTechnique = "Travel";

        Debug.Log("Active Technique " + stringTechnique);

        setTechnique(this.currentTechnique);
    }

    public void MoveToNextObjective() {

        if (this.currentTechnique == 0)
            this.teleport.Tele();
        else if (currentTechnique == 1)
            this.fadedTeleport.Faded();
        else if (currentTechnique == 2)
            this.travel.HowTravel();
    }

    void setTechnique(int technique) {
        
        if (technique == 0) {

            this.teleport.enabled = true;

            this.fadedTeleport.enabled = false;

            this.travel.enabled = false;
        }
        else if (technique == 1) {

            this.teleport.enabled = false;

            this.fadedTeleport.enabled = true;

            this.travel.enabled = false;
            
        }
        else if (technique == 2) {

            this.teleport.enabled = false;

            this.fadedTeleport.enabled = false;

            this.travel.enabled = true;
        }
    }

    public void ResetState() {

        this.setTechnique(0);

        this.transform.position = this.inicialPlayerPosition;

        this.transform.rotation = this.inicialPlayerRotation;

        this.GetComponent<Teleport>().ResetTechnique();

        this.GetComponent<FadedTeleport>().ResetTechnique();

        this.GetComponent<Travel>().ResetTechnique();
    }
}
