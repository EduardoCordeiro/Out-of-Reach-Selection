using UnityEngine;
using System.Collections;

public class Travel : MonoBehaviour {

    public GameObject checkPoint1;

    public GameObject checkPoint2;

    public GameObject checkPoint3;

    public GameObject checkPoint4;

    public GameObject checkPoint5;

    public GameObject checkPoint6;

    private int currentObjective;

    // Lerping Variables
    public float TravelTime = 2f;

    // Whether we are currently interpolating or not
    private bool _isTraveling;

    // The start and finish positions for the interpolation
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    // The Time.time value when we started the interpolation
    private float _timeStartedTraveling;

    public GameObject teleporter;

    // Use this for initialization
    public void Start() {

        this.currentObjective = 0;

        this.teleporter.SetActive(false);
    }

    public void FixedUpdate() {

        if (_isTraveling) {

            float timeSinceStarted = Time.time - _timeStartedTraveling;
            float percentageComplete = timeSinceStarted / TravelTime;

            transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

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

    public void HowTravel() {

        switch (this.currentObjective) {

            case 0: TravelToCheckpoint(checkPoint1);

                this.currentObjective++;

                break;

            case 1: TravelToCheckpoint(checkPoint2);

                this.currentObjective++;

                break;

            case 2: TravelToCheckpoint(checkPoint3);

                this.currentObjective++;

                break;

            case 3: TravelToCheckpoint(checkPoint4);

                this.currentObjective++;

                break;

            case 4: TravelToCheckpoint(checkPoint5);

                this.currentObjective++;

                break;

            case 5: TravelToCheckpoint(checkPoint6);

                break;
        }
    }

    void TravelToCheckpoint(GameObject checkpoint) {

        //Vector3 endPosition = checkpoint.transform.position - Utility.heightAdjustment;

        StartTraveling(this.transform.position, checkpoint.transform.position);

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
