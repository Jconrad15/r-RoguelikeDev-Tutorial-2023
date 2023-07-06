using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Utility : MonoBehaviour
{
    public static Color32 DarkenColor(Color32 color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

        v -= 10f / 100f;
        s -= 5f / 100f;
        Color32 newColor = Color.HSVToRGB(h, s, v);

        return newColor;
    }

    public static Color32 HideColor(Color32 color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

        v -= 10f / 100f;
        s -= 5f / 100f;
        Color32 newColor = Color.HSVToRGB(h, s, v);
        newColor.a = 50;
        return newColor;
    }

    public static Color32 LightenColor(Color32 color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

        v += 10f / 100f;
        s += 5f / 100f;
        Color32 newColor = Color.HSVToRGB(h, s, v);

        return newColor;
    }

    public static Color32 HueShift(Color32 color, float amount)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

        h = (h + amount) % 360f;
        Color32 newColor = Color.HSVToRGB(h, s, v);

        return newColor;
    }

    /// <summary>
    /// Returns a random color.
    /// </summary>
    /// <returns></returns>
    public static Color32 RandomColor()
    {
        return new Color32(
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
            255);
    }

    /// <summary>
    /// Returns a random Enum of given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetRandomEnum<T>()
    {
        Array enumArray = Enum.GetValues(typeof(T));
        T selectedEnum = (T)enumArray.GetValue(UnityEngine.Random.Range(0, enumArray.Length));
        return selectedEnum;
    }
}
