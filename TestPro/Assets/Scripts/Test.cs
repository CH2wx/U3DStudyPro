using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
	public GameObject cube;
	public GameObject sphere;

	[Range(1.0f, 10.0f)]
	public float additionSpeed = 5;
	private float baseSpeed = 1;
	public float speed;
	
	public Vector3 axis;
	
	public float maxEuler = 150;
	public float curMaxEuler;

	//private float rate = 1.5f;
	public float maxLevel = 5;
	private float _curLevel = 0;
	public float curLevel {
		set  {
			if (value > maxLevel) return;
			_curLevel = value;
			curMaxEuler = maxEuler / maxLevel * value;
		}
		get {
			return _curLevel;
		}
	}

	// Use this for initialization
	void Start () {
		axis = Vector3.forward;
		// for (float i = -180; i <= 180; i++)
		// {
		// 	print(Mathf.Cos(i / 180 * Mathf.PI));
		// }
	}

	public void AddLevel ()
	{
		curLevel = curLevel + 1;
	}

	public void ClearLevel ()
	{
		curLevel = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// print(sphere.transform.eulerAngles);
		if (curLevel > 0)
		{
			float eulerZ = sphere.transform.eulerAngles.z;
			eulerZ = eulerZ + Mathf.Floor(Mathf.Abs(eulerZ) / 360) * 360;
			eulerZ = eulerZ % 360;
			eulerZ = eulerZ > 180 ? eulerZ - 360 : eulerZ;
			print(string.Format("{0}\t{1}\t{2}\t{3}", eulerZ, eulerZ * 1 / 180 * Mathf.PI, Mathf.Cos(eulerZ * 1 / 180 * Mathf.PI), (additionSpeed) * Mathf.Cos(eulerZ * 1 / 180 * Mathf.PI) * curLevel));
			speed = baseSpeed + (additionSpeed) * Mathf.Max(0, Mathf.Cos(eulerZ / maxEuler / 2 * Mathf.PI) * curLevel);
			if(axis.z == Vector3.forward.z && eulerZ >= curMaxEuler)
				axis = Vector3.back;
			else if (axis.z == Vector3.back.z && eulerZ <= -curMaxEuler)
				axis = Vector3.forward;
			sphere.transform.RotateAround(cube.transform.position, axis, speed);
		}
	}
}
