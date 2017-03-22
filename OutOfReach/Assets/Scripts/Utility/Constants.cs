using UnityEngine;
using System.Collections;

public static class Constants {

    // No objects selected and user can move the cone around
    public static int MOVEMENT_PHASE = 0;
    // Mouse0 or Imuduino has been pressed and one or more objects are selected
    public static int REFINEMENT_PHASE = 1;
    // If only 1 object is Selected [Final Phase]
    public static int FINAL_PHASE = 3;

    // Camera's default FoV
    public static float defaultFoV = 60.0f;

    // Cone Starting Angle
    public static float coneStartingAngle = 43.45f;

    // Number of Rays being cast from the hand ; angles per ray: 10 -> 36 rays ;  20 -> 18 rays;
    public static int defaultNumberOfRays = 36;

    // 
    public static string outerCircle = "OuterCicle";

    public static string innerCircle = "InnerCicle";

    public static float outerCircleRadius = 0.045f;

    public static float innerCircleRadius = 0.025f;

    // Default Z for the Cone.
    public static float defaultConeApperture = 0.4f;

    public static int raysPerAngle = 10;

    // Cone range Limits and Ratio
    public static float coneRangeLowerLimit = 1.0f;

    public static float coneRangeUpperLimit = 60.0f;

    public static float coneRangeRatio = 0.3f; //0.3f

    public static Vector3 startingConeScale = new Vector3(coneRangeRatio, 15.0f, coneRangeRatio);

    // Cone apperture Limit
    public static float coneApertureUpperLimit = 2.5f;

    public static float coneApertureLowerLimit = 0.4f;

    // Scenes layerMask
    public static int sceneMask = 9;

    public static int sphereColliderMark = 10;

    //
    public static float separationThreshold = 2.0f;

    public static Vector3 playerInitialPosition = new Vector3(0.0f, 0.0f, 0.0f);
}
