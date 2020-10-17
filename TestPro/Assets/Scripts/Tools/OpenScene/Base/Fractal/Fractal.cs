using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
	public Mesh[] meshs;
	public Material material;

	public int maxColorLen = 3;
	public int maxDepth = 4;
	private int curDepth = 0;

	public float childScale = 0.5f;

	[Range(0, 1)]
	public float spawnProbability = 1;

	public Material[,] materials = null;

	public float maxTwist;

	public float maxRotateSpeed;
	private float rotateSpeed;

	public class Direction{
		public Vector3 direction;
		public Quaternion rotation;
		public Direction(Vector3 vct, Vector3 euler)
		{
			this.direction = vct;
			this.rotation = Quaternion.Euler(euler);
		}
	}
	public Direction[] directions = {
		new Direction(Vector3.up, new Vector3(0, 0, 0)),
		new Direction(Vector3.left, new Vector3(0, 0, 90)),
		new Direction(Vector3.right, new Vector3(0, 0, -90)),
		new Direction(Vector3.forward, new Vector3(90, 0, 0)),
		new Direction(Vector3.back, new Vector3(-90, 0, 0)),
	};

	void Start () {
		int length = maxDepth + 1;
		int maxColor = maxColorLen + 1;
		if (materials == null || materials.Length < length)
		{
			InitMaterials(length, maxColor);
		}

		transform.Rotate(Random.Range(-maxTwist, maxTwist), 0, 0);
		rotateSpeed = Random.Range(-maxRotateSpeed, maxRotateSpeed);

		// print(this.name + "\tStart");
		gameObject.AddComponent<MeshFilter>().mesh = meshs[Random.Range(0, meshs.Length)];
		gameObject.AddComponent<MeshRenderer>().material = materials[curDepth, Random.Range(0, maxColor)];

		if (curDepth < maxDepth)
		{
			StartCoroutine(CreateChildren());
		}
	}

	void Update()
	{
		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
	}

	private IEnumerator CreateChildren()
	{
		for (int i = 0; i < directions.Length; i++)
		{
			if (IsDrawChildren()){
				yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));
				new GameObject("Fractal Child" + curDepth).
					AddComponent<Fractal>().Initialize(this, directions[i]);	
			}
		}
	}
	
	private void Initialize(Fractal parent, Direction direction)
	{
		// print("Init\t" + this.name + "\t" + parent.name);
		meshs = parent.meshs;
		material = parent.material;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		curDepth = parent.curDepth + 1;
		childScale = parent.childScale;
		spawnProbability = parent.spawnProbability;
		maxRotateSpeed = parent.maxRotateSpeed;
		transform.SetParent(parent.transform);
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = direction.direction * (parent.childScale + childScale * 0.5f);
		transform.localRotation = direction.rotation;
	}

	private void InitMaterials(int length1, int length2)
	{
		materials = new Material[length1, length2];
		for (int i = 0; i < length1; i++)
		{
			float temp = i / (maxDepth - 1);
			temp = Mathf.Pow(temp, 2);
			for (int j = 0; j < length2; j++)
			{
				materials[i, j] = new Material(material);
				materials[i, j].name = string.Format("Materials_{0}_{1}", i, j);
				materials[i, j].color = Color.Lerp(Color.white, new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)), temp);
			}
		}
	}

	private bool IsDrawChildren()
	{
		return Random.value < spawnProbability;
	}
}
