﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(Application.dataPath);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
        {
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Res/Test/ScreenCapture/screencapture.png");
        }
	}
}
