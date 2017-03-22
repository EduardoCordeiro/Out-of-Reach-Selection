using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class TrackerClient : MonoBehaviour {
	// Spine
	public Transform spineShoulder;
	public Transform spineMid;
	public Transform spineBase;

	// Left Arm
	public Transform leftShoulder;
	public Transform leftElbow;
	public Transform leftArm;

	// Left Leg
	public Transform leftHip;
	public Transform leftKnee;

	// Right Arm
	public Transform rightShoulder;
	public Transform rightElbow;
	public Transform rightArm;

	// Right Leg
	public Transform rightHip;
	public Transform rightKnee;

	private Human trackedHuman;
    public Human TrackedHuman {
        get { return this.trackedHuman; }
    }

	private Dictionary<string, Human> _humans;

	void Start() {

		trackedHuman = null;
		_humans = new Dictionary<string, Human>();
	}

	void Update() {

		string currentHumanId = GetHumanIdWithHandUp();

		if (_humans.ContainsKey(currentHumanId)) 
	        trackedHuman = _humans[currentHumanId];
		
		UpdateCharacterBody(trackedHuman);

		// Finally
		CleanDeadHumans();
	}

	private void UpdateCharacterBody(Human human) {

		if (human != null) {
			// Spine
			Vector3 spineUp = Utility.GetBoneDirection(human.body.Joints[BodyJointType.spineShoulder], human.body.Joints[BodyJointType.spineBase]);
			Vector3 spineRight = Utility.GetBoneDirection(human.body.Joints[BodyJointType.rightShoulder], human.body.Joints[BodyJointType.leftShoulder]);
			Vector3 spineForward = Vector3.Cross(spineRight, spineUp);

			//comment this for no kinect
			spineBase.position = human.body.Joints[BodyJointType.spineBase];
			spineBase.rotation = Quaternion.LookRotation(spineForward, spineUp);

			// Left Arm
			leftShoulder.rotation = Utility.GetQuaternionFromRightUp(-Utility.GetBoneDirection(human.body.Joints[BodyJointType.leftShoulder], human.body.Joints[BodyJointType.spineShoulder]), spineUp);
			leftElbow.rotation = Utility.GetQuaternionFromRightUp(-Utility.GetBoneDirection(human.body.Joints[BodyJointType.leftWrist], human.body.Joints[BodyJointType.leftElbow]), spineUp);
			leftArm.rotation = Utility.GetQuaternionFromRightUp(-Utility.GetBoneDirection(human.body.Joints[BodyJointType.leftElbow], human.body.Joints[BodyJointType.leftShoulder]), spineUp);

			// Left Leg
			leftHip.rotation = Utility.GetQuaternionFromUpRight(-Utility.GetBoneDirection(human.body.Joints[BodyJointType.leftKnee], human.body.Joints[BodyJointType.leftHip]), spineRight);
			leftKnee.rotation = Utility.GetQuaternionFromUpRight(-Utility.GetBoneDirection(human.body.Joints[BodyJointType.leftAnkle], human.body.Joints[BodyJointType.leftKnee]), spineRight);

			// Right Arm
			rightShoulder.rotation = Utility.GetQuaternionFromRightUp(Utility.GetBoneDirection(human.body.Joints[BodyJointType.rightShoulder], human.body.Joints[BodyJointType.spineShoulder]), spineUp);
			rightElbow.rotation = Utility.GetQuaternionFromRightUp(Utility.GetBoneDirection(human.body.Joints[BodyJointType.rightWrist], human.body.Joints[BodyJointType.rightElbow]), spineUp);
			rightArm.rotation = Utility.GetQuaternionFromRightUp(Utility.GetBoneDirection(human.body.Joints[BodyJointType.rightElbow], human.body.Joints[BodyJointType.rightShoulder]), spineUp);

			// Right Leg
			rightHip.rotation = Utility.GetQuaternionFromUpRight(-Utility.GetBoneDirection(human.body.Joints[BodyJointType.rightKnee], human.body.Joints[BodyJointType.rightHip]), spineRight);
			rightKnee.rotation = Utility.GetQuaternionFromUpRight(-Utility.GetBoneDirection(human.body.Joints[BodyJointType.rightAnkle], human.body.Joints[BodyJointType.rightKnee]), spineRight);
		}
	}

	/// <summary>
	/// Gets the first human identifier with the hand above the head.
	/// </summary>
	private string GetHumanIdWithHandUp() {

		foreach (Human h in _humans.Values)
			if (h.body.Joints[BodyJointType.leftHand].y  > h.body.Joints[BodyJointType.head].y ||
                h.body.Joints[BodyJointType.rightHand].y > h.body.Joints[BodyJointType.head].y)                    
                    return h.id;

		return string.Empty;
	}

	public void SetNewFrame(Body[] bodies) {

        foreach (Body b in bodies) {

            try {  
				string bodyID = b.Properties[BodyPropertiesType.UID];

				if (!_humans.Keys.Contains(bodyID))
					_humans.Add(bodyID, new Human());
				
				_humans[bodyID].Update(b);
			} 
			catch (Exception e) {

				Debug.LogError("[TrackerClient] ERROR:" + e.StackTrace);
			}
		}
	}

	void CleanDeadHumans()	{

		List<Human> deadhumans = new List<Human>();

		foreach (Human h in _humans.Values)
			if (DateTime.Now > h.lastUpdated.AddMilliseconds(1000))
                deadhumans.Add(h);
			
		foreach (Human h in deadhumans) 
		    _humans.Remove(h.id);
	}
}