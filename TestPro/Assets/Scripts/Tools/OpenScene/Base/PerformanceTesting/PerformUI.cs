using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSCounter))]
public class PerformUI : MonoBehaviour
{
    [System.Serializable]
    private struct FPSColor
    {
        public Color color;
        public int minimunFPS;  // 最小FPS
    };

    // 储存FPS帧率的字符串，避免在更新text的时候生成临时字符串导致的性能浪费
    private string[] stringsFrom0To99 = new string[100];

    public Text averageFPSLabel, highestFPSLabel, lowestFPSLabel;
    [SerializeField]
    private FPSColor[] fpsColors;

    private FPSCounter fpsCounter;

    private void Awake()
    {
        InitString();
        InitColor();

        fpsCounter = GetComponent<FPSCounter>();
    }

    private void Update()
    {
        ShowFPSText(averageFPSLabel, fpsCounter.fps.averageFPS);
        ShowFPSText(highestFPSLabel, fpsCounter.fps.highestFPS);
        ShowFPSText(lowestFPSLabel, fpsCounter.fps.lowestFPS);
    }

    private void ShowFPSText(Text text, int fps)
    {
        text.text = stringsFrom0To99[Mathf.Clamp(fps, 0, 99)];
        for (int i = 0; i < fpsColors.Length; i++)
        {
            if (fps >= fpsColors[i].minimunFPS)
            {
                text.color = fpsColors[i].color;
                break;
            }
        }
    }

    private void InitString()
    {
        for (int i = 0; i < 100; i++)
        {
            stringsFrom0To99[i] = i.ToString();
        }
    }

    private void InitColor()
    {
        fpsColors = new FPSColor[5];

        // 蓝色，60帧
        fpsColors[0].color = Color.cyan;
        fpsColors[0].minimunFPS = 60;

        // 绿色，45帧
        fpsColors[1].color = Color.green;
        fpsColors[1].minimunFPS = 45;

        // 黄色，30帧
        fpsColors[2].color = Color.yellow;
        fpsColors[2].minimunFPS = 30;

        // 橘色，15帧
        fpsColors[3].color = new Color(1f, 0.5f, 0);
        fpsColors[3].minimunFPS = 15;

        // 红色，0帧
        fpsColors[4].color = Color.red;
        fpsColors[4].minimunFPS = 0;
    }
}
