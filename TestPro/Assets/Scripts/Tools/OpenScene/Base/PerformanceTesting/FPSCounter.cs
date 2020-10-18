using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour {
    public FPS fps;
    public int FPSRange = 60;

	// Use this for initialization
	void Start () {
        fps = new FPS();
    }
	
	// Update is called once per frame
	void Update () {
        //fps = (int)(1 / Time.unscaledDeltaTime);
        if (fps.fpsRange != FPSRange)
        {
            fps.InitFPSData(FPSRange);
        }
        fps.UpdateFPS();
	}

    public class FPS
    {
        public int fpsRange;
        private int fpsBufferIndex;
        private int[] fpsBuffer;
        private bool isFullBuffer;

        public int averageFPS, highestFPS, lowestFPS;

        public void InitFPSData(int FPSRange)
        {
            if (FPSRange <= 0)
            {
                FPSRange = 60;
            }
            fpsRange = FPSRange;
            fpsBufferIndex = 0;
            isFullBuffer = false;
            fpsBuffer = new int[fpsRange];
        }

        public void UpdateFPS()
        {
            fpsBuffer[fpsBufferIndex] = (int)(1 / Time.unscaledDeltaTime);

            if (++fpsBufferIndex >= fpsRange)
            {
                isFullBuffer = true;
                fpsBufferIndex = 0;
            }
            if (isFullBuffer)
            {
                CalculateFPS();
            }
        }

        private void CalculateFPS()
        {
            int sum = 0;
            highestFPS = int.MinValue;
            lowestFPS = int.MaxValue;
            for (int i = 0; i < fpsRange; i++)
            {
                int curFPS = fpsBuffer[i];
                sum += curFPS;
                if (curFPS > highestFPS)
                {
                    highestFPS = curFPS;
                }
                else if (curFPS < lowestFPS)
                {
                    lowestFPS = curFPS;
                }
            }
            averageFPS = (int)(sum / fpsRange);
        }
    }
}
