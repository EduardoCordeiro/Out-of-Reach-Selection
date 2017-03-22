using UnityEngine;
using System.Collections;

public static class MessageSeparators 
{
	public const char L1 = '#';
	public const char L2 = '/';
	public const char SET = '=';
}

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

public enum UserTestPermutationsID : int
{
    FPG = 0,
    FGP = 1,

    PFG = 2,
    GFP = 3,

    PGF = 4,
    GPF = 5
}