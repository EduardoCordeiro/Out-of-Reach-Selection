using UnityEngine;
using System.Collections;

public enum UserTaskID : int
{
	// First task
	FreeRoam = 0,

    // Scattered Objects
    ScatteredObjectSelectionNear = 1,
    ScatteredObjectSelectionFar = 2,

    // Objects very close to each other
    CloseObjectSelectionNear = 3,
    CloseObjectSelectionFar = 4    
}

public enum UserTestID : int
{
    Precious = 0,
    GoGo = 1,
    Flashlight = 2
}