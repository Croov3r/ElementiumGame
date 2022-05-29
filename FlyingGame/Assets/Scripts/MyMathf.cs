using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMathf
{
    public static float Arg(Vector3 vec)
    {
        float x = vec.x;
        float y = vec.y;
        if ((y >= 0) && (x >= 0))
        {
            return Mathf.Atan(Mathf.Abs(y) / Mathf.Abs(x)) * 180 / Mathf.PI;
        }
        else if ((y >= 0) && (x <= 0))
        {
            return 1 * 90 + Mathf.Atan(Mathf.Abs(x) / Mathf.Abs(y)) * 180 / Mathf.PI;
        }
        else if ((y <= 0) && (x <= 0))
        {
            return 2 * 90 + Mathf.Atan(Mathf.Abs(y) / Mathf.Abs(x)) * 180 / Mathf.PI;
        }
        return 3 * 90 + Mathf.Atan(Mathf.Abs(x) / Mathf.Abs(y)) * 180 / Mathf.PI;
    }

    public static Vector3 RotateVector(Vector3 vec, float ang)
    {
        float x = vec.x;
        float y = vec.y;
        return new Vector3(Mathf.Cos(ang) * x - Mathf.Sin(ang) * y, Mathf.Sin(ang) * x + Mathf.Cos(ang) * y, vec.z);
    }
}
