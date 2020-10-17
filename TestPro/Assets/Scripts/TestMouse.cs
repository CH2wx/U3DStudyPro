using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouse : MonoBehaviour {
	public GameObject cube;
	public GameObject c1,c2;
	// Use this for initialization
	void Start () {
		Debug.Log(Time.time + "\t" + Time.deltaTime);
		Vector3 vct3 = c1.transform.position - c2.transform.position;
		Debug.Log(vct3);
	}
	
	// Update is called once per frame
	void Update () {
		
		// if (Input.GetMouseButtonDown(0))
		// {
		// 	int x = Random.Range(-10, 10);
		// 	int y = Random.Range(-10, 10);
		// 	int z = Random.Range(-10, 10);
			
		// 	gameObject.transform.position = new Vector3(x, y, z);
		// 	Debug.Log(this.gameObject.transform.position);
		// }
	}

	void OnMouseUpAsButton()
	{
		// this.gameObject.SetActive(false);
		Debug.Log(Time.time + "\t" + Time.deltaTime);
		// Debug.Log(this.gameObject.name);
	}

	void OnMouseDown()
	{
		// Debug.Log("OnMouseDown");
	}

	void OnMouseUp()
	{
		// Debug.Log("OnMouseUp");
	}

	void OnMouseDrag()
	{
		// Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// Debug.Log(Input.mousePosition);
		// Vector3 vct3 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		// Vector3 v2 = Camera.main.WorldToScreenPoint(vct3);
		// gameObject.transform.position = v2;
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log(collision.gameObject.name);
	}
}
