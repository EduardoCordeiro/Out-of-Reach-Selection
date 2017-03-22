using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class FadedTeleport : MonoBehaviour {

    public GameObject checkPoint1;

    public GameObject checkPoint2;

    public GameObject checkPoint3;

    public GameObject checkPoint4;

    public GameObject checkPoint5;

    public GameObject checkPoint6;

    private int currentObjective;

    private bool fade;

    public GameObject city;

    /* Teleporter Variables */
    public GameObject teleporter;

    // Lerping Variables
    private float TravelTime = 1.5f;

    // Whether we are currently interpolating or not
    private bool _isTraveling;

    // The start and finish positions for the interpolation
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    // The Time.time value when we started the interpolation
    private float _timeStartedTraveling;

    private Vector3 teleporterOffset = new Vector3(0.0f, 3f, 0.0f);

    // Use this for initialization
    public void Start() {

        this.currentObjective = 0;

        this.teleporter.SetActive(true);
    }

    public void LateUpdate() {

        if (fade == true) {

            switch (this.currentObjective) {

                case 0: FadedTeleportToCheckpoint(checkPoint1);

                    this.currentObjective++;

                    break;

                case 1: FadedTeleportToCheckpoint(checkPoint2);

                    this.currentObjective++;

                    break;

                case 2: FadedTeleportToCheckpoint(checkPoint3);

                    this.currentObjective++;

                    break;

                case 3: FadedTeleportToCheckpoint(checkPoint4);

                    this.currentObjective++;

                    break;

                case 4: FadedTeleportToCheckpoint(checkPoint5);

                    this.currentObjective++;

                    break;

                case 5: FadedTeleportToCheckpoint(checkPoint6);

                    break;
            }
        }

        if (_isTraveling) {

            float timeSinceStarted = Time.time - _timeStartedTraveling;
            float percentageComplete = timeSinceStarted / TravelTime;

            this.teleporter.transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            if (percentageComplete >= 1.0f)
                _isTraveling = false;
        }
    }

    public void StartTraveling(Vector3 startPosition, Vector3 endPosition) {

        _isTraveling = true;
        _timeStartedTraveling = Time.time;

        _startPosition = startPosition;
        _endPosition = endPosition;
    }

    // Update is called once per frame
    public void Update() {

        /*if (fade == true) {

            switch (this.currentObjective) {

                case 0: FadedTeleportToCheckpoint(checkPoint1);

                    this.currentObjective++;

                    break;

                case 1: FadedTeleportToCheckpoint(checkPoint2);

                    this.currentObjective++;

                    break;

                case 2: FadedTeleportToCheckpoint(checkPoint3);

                    this.currentObjective++;

                    break;

                case 3: FadedTeleportToCheckpoint(checkPoint4);

                    this.currentObjective++;

                    break;

                case 4: FadedTeleportToCheckpoint(checkPoint5);

                    this.currentObjective++;

                    break;

                case 5: FadedTeleportToCheckpoint(checkPoint6);

                    break;
            }
        }*/
    }

    public void Faded() {

        StartTraveling(this.teleporter.transform.position, this.teleporter.transform.position + teleporterOffset);

        //Fade Out Scenes
        StartCoroutine("Fade"); 
    }

    void FadedTeleportToCheckpoint(GameObject checkpoint) {

        this.transform.position = checkpoint.transform.position;

        StartTraveling(this.teleporter.transform.position, this.teleporter.transform.position - teleporterOffset);

        DisableCheckpoint(checkpoint);

        this.fade = false;
    }

    public IEnumerator Fade() {

        yield return new WaitForSeconds(2f);

        this.fade = true;
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

        this.teleporter.SetActive(false);
    }
}