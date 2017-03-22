using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

    public GameObject checkPoint1;

    public GameObject checkPoint2;

    public GameObject checkPoint3;

    public GameObject checkPoint4;

    public GameObject checkPoint5;

    public GameObject checkPoint6;

    private int currentObjective;

    public GameObject teleporter;

	// Use this for initialization
    public void Start() {

        this.currentObjective = 0;

        this.teleporter.SetActive(false);
	}

    public void Tele() {

        switch (this.currentObjective) {

            case 0: TeleportToCheckpoint(checkPoint1);

                this.currentObjective++;

                break;

            case 1: TeleportToCheckpoint(checkPoint2);

                this.currentObjective++;

                break;

            case 2: TeleportToCheckpoint(checkPoint3);

                this.currentObjective++;

                break;

            case 3: TeleportToCheckpoint(checkPoint4);

                this.currentObjective++;

                break;

            case 4: TeleportToCheckpoint(checkPoint5);

                this.currentObjective++;

                break;

            case 5: TeleportToCheckpoint(checkPoint6);

                break;
        }
    }

    void TeleportToCheckpoint(GameObject checkpoint) {

        this.transform.position = checkpoint.transform.position;

        //this.transform.position -= Utility.heightAdjustment;

        DisableCheckpoint(checkpoint);
    }

    void DisableCheckpoint(GameObject checkpoint) {

        checkpoint.SetActive(false);
    }

    public void ResetTechnique() {

        this.currentObjective = 0;

        this.checkPoint1.SetActive(true);

        this.checkPoint2.SetActive(true);

        this.checkPoint3.SetActive(true);

        this.checkPoint4.SetActive(true);

        this.checkPoint5.SetActive(true);

        this.checkPoint6.SetActive(true);
    }
}
