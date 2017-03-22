using UnityEngine;
using System.Collections;

public class Utility {

    public static Vector3 heightAdjustment = new Vector3(0.0f, 2.5f, 0.0f);

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
