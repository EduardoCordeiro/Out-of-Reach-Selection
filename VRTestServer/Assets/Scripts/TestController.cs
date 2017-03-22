using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

public class TestController : MonoBehaviour
{
	// Server
	public ServerController serverController;

	// Tests/Tasks
	private bool isTaskActive;
	private Stopwatch taskDuration;

	private List<UserTaskID> tasksToPerform;
	private Dictionary<UserTestPermutationsID, List<UserTestID>> testsToPerform;

	private UserTestID currentTest;
	private UserTaskID currentTask;

	// GUI
	public Text txtCurrentTest;
	public Text txtCurrentTask;

	public Button btnNextTask;
	public Button btnStartStopwatch;

	public Button btnTestPermutation;
	public Button btnTestPermutationCheck;

	private UserTestPermutationsID testPermutationButtonState;

	// Use this for initialization
	void Start() 
	{
		isTaskActive = false;
		taskDuration = new Stopwatch();

		tasksToPerform = new List<UserTaskID>();
		testsToPerform = new Dictionary<UserTestPermutationsID, List<UserTestID>>();

		Reset();
	}

	// Update is called once per frame
	void Update() 
	{ 
		if (isTaskActive)
		{
			btnStartStopwatch.GetComponentInChildren<Text>().text = taskDuration.Elapsed.ToString();
		}
	}

	public void SelectTestPermutation(bool confirmPermutation)
	{
		if (!confirmPermutation)
		{
			int currentButtonState = (int)testPermutationButtonState;
			currentButtonState++;
			testPermutationButtonState = (UserTestPermutationsID)currentButtonState;
	
			if (currentButtonState == Enum.GetNames(typeof(UserTestPermutationsID)).Length)
			{
				testPermutationButtonState = (UserTestPermutationsID)0;
			}
			btnTestPermutation.GetComponentInChildren<Text>().text = testPermutationButtonState.ToString();
		}
		else
		{	
			btnTestPermutation.interactable = false;
			btnTestPermutationCheck.interactable = false;

			btnNextTask.interactable = false;
			btnStartStopwatch.interactable = true;

			SetupTest();
			SetupTask();

			serverController.UdpUnicast.Send("ts" + MessageSeparators.L1 + "tstnext" + MessageSeparators.SET + (int)currentTest + MessageSeparators.SET + (int)currentTask);
		}
	}

	public void StartStopwatch()
	{
		if (!isTaskActive) 
		{
			isTaskActive = true;
			taskDuration.Start();

			serverController.UdpUnicast.Send("sw" + MessageSeparators.L1 + "start");
		} 
		else 
		{
			taskDuration.Stop();
			taskDuration.Reset();
			isTaskActive = false;

			btnNextTask.interactable = true;
			btnStartStopwatch.interactable = false;
			btnStartStopwatch.GetComponentInChildren<Text>().text = "Start Stopwatch";

			serverController.UdpUnicast.Send("sw" + MessageSeparators.L1 + "stop");

			if (testsToPerform[testPermutationButtonState].Count == 0 && tasksToPerform.Count == 0) // No more tests and tasks to perform
			{
				Reset();

				btnNextTask.interactable = false;
				btnStartStopwatch.interactable = false;

				btnTestPermutation.interactable = true;
				btnTestPermutationCheck.interactable = true;

				serverController.UdpUnicast.Send("ts" + MessageSeparators.L1 + "tstover");
			}
		}
	}

	public void TestSwitch()
	{
		if (tasksToPerform.Count == 0) // Finished tasks to perform in a certain test
		{
			SetupTest();
			SetupTask();

			btnNextTask.interactable = false;
			btnStartStopwatch.interactable = true;

			serverController.UdpUnicast.Send("ts" + MessageSeparators.L1 + "tstnext" + MessageSeparators.SET + (int)currentTest + MessageSeparators.SET + (int)currentTask);
		}
		else if (tasksToPerform.Count > 0) // Didn't finish tasks to perform in a certain test
		{
			SetupTask();

			btnNextTask.interactable = false;
			btnStartStopwatch.interactable = true;

			serverController.UdpUnicast.Send("ts" + MessageSeparators.L1 + "tsknext" + MessageSeparators.SET + (int)currentTask);
		}
	}

	private void SetupTest()
	{
		ResetTasks();

		// Gets a random test
		currentTest = testsToPerform[testPermutationButtonState][0];
		testsToPerform[testPermutationButtonState].Remove(currentTest);

		txtCurrentTest.text = "Current test:  " + currentTest.ToString();
	}

	private void SetupTask()
	{
		// Get next task
		currentTask = tasksToPerform[0];
		tasksToPerform.Remove(currentTask);

		txtCurrentTask.text = "Current task: " + currentTask.ToString();
	}

	private void ResetTests()
	{
		testsToPerform.Clear();

        testsToPerform[UserTestPermutationsID.FPG] = new List<UserTestID>() { UserTestID.Flashlight, UserTestID.Precious, UserTestID.GoGo };
        testsToPerform[UserTestPermutationsID.FGP] = new List<UserTestID>() { UserTestID.Flashlight, UserTestID.GoGo, UserTestID.Precious };
        testsToPerform[UserTestPermutationsID.PFG] = new List<UserTestID>() { UserTestID.Precious, UserTestID.Flashlight, UserTestID.GoGo };
        testsToPerform[UserTestPermutationsID.GFP] = new List<UserTestID>() { UserTestID.GoGo, UserTestID.Flashlight, UserTestID.Precious };
        testsToPerform[UserTestPermutationsID.PGF] = new List<UserTestID>() { UserTestID.Precious, UserTestID.GoGo, UserTestID.Flashlight };
        testsToPerform[UserTestPermutationsID.GPF] = new List<UserTestID>() { UserTestID.GoGo, UserTestID.Precious, UserTestID.Flashlight };

		testPermutationButtonState = (UserTestPermutationsID)0;
		btnTestPermutation.GetComponentInChildren<Text>().text = testPermutationButtonState.ToString();
	}

	private void ResetTasks()
	{
		tasksToPerform.Clear();
		tasksToPerform = Enum.GetValues(typeof(UserTaskID)).Cast<UserTaskID>().ToList();
	}

	public void Reset()
	{
		ResetTests();
		ResetTasks();

		txtCurrentTest.text = "Current test:  None";
		txtCurrentTask.text = "Current task: None";
	}
}