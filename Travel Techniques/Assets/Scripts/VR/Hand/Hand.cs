using UnityEngine;
using System.Collections;

public class Hand 
{
	// Attributes
	private GameObject instance;
	private GameObject insideObject;
	private Vector3 initialPosition;

	private bool isClosed;
	private bool isInside;
	private bool isEngaged;

	private GameObject axis;
	private GameObject handle;

	/// <summary>
	/// Initializes a new instance of the <see cref="Hand"/>class,
	/// </summary>
	public Hand(GameObject instance) 
	{
		this.instance = instance;
		this.insideObject = null;
		this.initialPosition = Vector3.zero;

		this.isClosed = false;
		this.isInside = false;
		this.isEngaged = false;

		this.axis = null;
		this.handle = null;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Hand"/>class,
	/// </summary>
	public Hand(GameObject instance, GameObject axis, GameObject handle) 
	{
		this.instance = instance;
		this.insideObject = null;
		this.initialPosition = Vector3.zero;

		this.isClosed = false;
		this.isInside = false;
		this.isEngaged = false;

		this.axis = axis;
		this.handle = handle;
	}

	/// <summary>
	/// Gets or sets the hand game object istance.
	/// </summary>
	public GameObject Instance 
	{
		get { return this.instance; }
		set { instance = value; }
	}

	/// <summary>
	/// Gets or sets the inside object.
	/// </summary>
	public GameObject InsideObject 
	{
		get { return this.insideObject; }
		set { insideObject = value; }
	}

	/// <summary>
	/// Gets or sets the initial hand position.
	/// </summary>
	public Vector3 InitialPosition 
	{
		get { return this.initialPosition; }
		set { initialPosition = value; }
	}

	/// <summary>
	/// Gets or sets a value indicating whether the hand is closed.
	/// </summary>
	public bool IsClosed 
	{
		get { return this.isClosed; }
		set { isClosed = value; }
	}

	/// <summary>
	/// Gets or sets a value indicating whether the hand is inside.
	/// </summary>
	public bool IsInside
	{
		get { return this.isInside; }
		set { isInside = value; }
	}
		
	/// <summary>
	/// Gets or sets a value indicating whether the hand is engaged.
	/// </summary>
	public bool IsEngaged
	{
		get { return this.isEngaged; }
		set { isEngaged = value; }
	}

	/// <summary>
	/// Gets or sets the axis.
	/// </summary>
	public GameObject Axis 
	{
		get { return this.axis; }
		set { axis = value; }
	}

	/// <summary>
	/// Gets or sets the handle.
	/// </summary>
	public GameObject Handle 
	{
		get { return this.handle; }
		set { handle = value; }
	}
}