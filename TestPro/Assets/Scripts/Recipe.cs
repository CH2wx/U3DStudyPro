using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum IngredientUnit { Spoon, Cup, Bowl, Piece }

[Serializable]
public class Ingredient
{
	public string name;
	public int amount = 1;
    public IngredientUnit unit;
}

public class Recipe : MonoBehaviour {
	public Ingredient potionResult;
    public Ingredient[] potionIngredients;

	// 在 Inspector 中将此浮点数显示为 0 到 10 之间的滑动条
	[Range(0f, 10f)]
	public float myFloat = 0f;

	public MyRangeAttribute myRangeAttribute;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
