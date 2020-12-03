using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ColorRangeHSV
{
    [FloatRangeSlider(0f, 1f)]
    public FloatRange hue, saturation, value, alpha;

    public Color RandomInRange
    {
        get
        {
            return Random.ColorHSV(
                hue.min, hue.max,
                saturation.min, saturation.max,
                value.min, value.max,
                alpha.min, alpha.max);
        }
    }

    public void SetRange(float hueMin, float hueMax,
                         float saturationMin, float saturationMax,
                         float valueMin, float valueMax,
                         float alphaMin, float alphaMax)
    {
        hue.SetRange(hueMin, hueMax);
        saturation.SetRange(saturationMin, saturationMax);
        value.SetRange(valueMin, valueMax);
        alpha.SetRange(alphaMin, alphaMax);
    }
}
