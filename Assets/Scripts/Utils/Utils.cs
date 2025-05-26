using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    #region ENUMS

    public enum EPlanToUse
    {
        XY,
        XZ
    }

    #endregion ENUMS


    #region PLATFORM

    public static bool IsComputer()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor
         || Application.platform == RuntimePlatform.WindowsPlayer
         || Application.platform == RuntimePlatform.OSXEditor
         || Application.platform == RuntimePlatform.OSXPlayer
         || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsMobile()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer
         || Application.platform == RuntimePlatform.Android)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion PLATFORM


    #region MATHS

    public static bool IsAngleWithinRange(float angle, Vector2 range, bool debug = false)
    {
        //New: normalize angle
        angle = angle % 360f;
        //New: normalize angle

        bool isInRange;

        float a1 = range[0] != 360f ? range[0] % 360f : 360f;
        float a2 = range[1] != 360f ? range[1] % 360f : 360f;

        float min = Mathf.Min(a1, a2);
        float max = Mathf.Max(a1, a2);

        if (debug)
        {
            Debug.Log("--- IsAngleWithinRange(angle: " + angle + ", range: " + range + " ---");
            Debug.Log(" -> a1: " + a1 + ", a2: " + a2 + ", min: " + min + ", max: " + max);
        }

        if (min >= 0f && max >= 0f)
        {
            isInRange = angle >= min && angle <= max;

            if (debug)
                Debug.Log("  => A -> isInRange: " + isInRange);

            return isInRange;
        }
        else if (min <= 0f && max >= 0f)
        {
            //min <= angle <= 0f
            isInRange = angle >= min && angle <= 0f;
            // - or -
            //0f <= angle <= max
            isInRange = isInRange || (angle >= 0f && angle <= max);

            if (debug)
                Debug.Log("  => B -> isInRange: " + isInRange);

            return isInRange;
        }
        else if (min <= 0f && max <= 0f)
        {
            isInRange = angle >= min && angle <= max;

            if (debug)
                Debug.Log("  => C -> isInRange: " + isInRange);

            return isInRange;
        }
        else
        {
            Debug.Log("WTF");

            return false;
        }
    }

    public static float ScreenDeltaPosToAngleInDegrees(Vector2 screenStartPos, Vector2 screenEndPos)
    {
        Vector2 diff = screenEndPos - screenStartPos;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (angle < 0f)
        {
            angle += 360f;
        }
        return angle;
    }

    public static Vector3 ComputeRotationShift(Vector3 _v1, float _xRot, float _yRot, float _zRot)
    {
        Vector3 v2 = _v1;
        // ---
        v2 = ComputeXRotationShift(v2, _xRot);
        v2 = ComputeYRotationShift(v2, _yRot);
        v2 = ComputeZRotationShift(v2, _zRot);
        // ---
        return v2;
    }

    public static Vector3 ComputeXRotationShift(Vector3 _v1, float _xRot)
    {
        float x1, y1, z1, x2, y2, z2;
        x1 = _v1.x;
        y1 = _v1.y;
        z1 = _v1.z;
        x2 = x1;
        y2 = y1 * Cos(_xRot) + z1 * (-Sin(_xRot));
        z2 = y1 * Sin(_xRot) + z1 * (Cos(_xRot));
        return new Vector3(x2, y2, z2);
    }

    public static Vector3 ComputeYRotationShift(Vector3 _v1, float _yRot)
    {
        float x1, y1, z1, x2, y2, z2;
        x1 = _v1.x;
        y1 = _v1.y;
        z1 = _v1.z;
        x2 = x1 * Cos(_yRot) + z1 * Sin(_yRot);
        y2 = y1;
        z2 = x1 * (-Sin(_yRot)) + z1 * Cos(_yRot);
        return new Vector3(x2, y2, z2);
    }

    public static Vector3 ComputeZRotationShift(Vector3 _v1, float _zRot)
    {
        float x1, y1, z1, x2, y2, z2;
        x1 = _v1.x;
        y1 = _v1.y;
        z1 = _v1.z;
        x2 = x1 * Cos(_zRot) + y1 * (-Sin(_zRot));
        y2 = x1 * Sin(_zRot) + y1 * Cos(_zRot);
        z2 = z1;
        return new Vector3(x2, y2, z2);
    }

    public static float Cos(float _degrees)
    {
        return Mathf.Cos(_degrees * Mathf.Deg2Rad);
    }

    public static float Sin(float _degrees)
    {
        return Mathf.Sin(_degrees * Mathf.Deg2Rad);
    }

    /// <summary>
    /// Nullify the y coordinate.
    /// Useful to compare distance between units, using a bird top view.
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static Vector3 Pos3DTo2D(Vector3 worldPos)
    {
        worldPos.y = 0f;
        return worldPos;
    }

    public static float UnityToTrigoAngle(float unityAngle)
    {
        return -1f * unityAngle + 90f;
    }

    public static float TrigoToUnityAngle(float trigoAngle)
    {
        return -1f * trigoAngle + 90f;
    }

    public static Vector3 TrigoAngleToDir3(float trigoAngle)
    {
        float x = Mathf.Cos(trigoAngle * Mathf.Deg2Rad);
        float y = 0f;
        float z = Mathf.Sin(trigoAngle * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }

    public static Vector3 UnityAngleToDir3(float unityAngle)
    {
        return TrigoAngleToDir3(UnityToTrigoAngle(unityAngle));
    }

    #endregion MATHS


    #region FLOAT CORRECTIONS

    public static float ChangeDecimals(float _f, int _decimals)
    {
        if (_decimals < 0)
        {
            _decimals = 0;
        }
        _f *= Mathf.Pow(10, _decimals);
        _f = Mathf.RoundToInt(_f);
        _f /= Mathf.Pow(10, _decimals);

        return _f;
    }

    #endregion FLOAT CORRECTIONS


    #region GEOMETRY

    /// <summary>
    /// Used to make Rect.Overlap work!
    /// </summary>
    /// <param name="rt"></param>
    /// <returns></returns>
    public static Rect GetWorldSpaceRect(RectTransform rt)
    {
        Rect r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = rt.TransformVector(r.size);
        return r;
    }

    public static bool IsWithin(Vector2 xRange1, Vector2 yRange1,
        Vector2 xRange2, Vector2 yRange2)
    {
        //Rectangle 1 -> Rectangle 2
        foreach (Vector2 corner1 in GetCorners(xRange1, yRange1))
        {
            if (IsPointWithinRange(corner1, xRange2, yRange2))
            {
                return true;
            }
        }
        //Rectangle 2 -> Rectangle 1
        foreach (Vector2 corner2 in GetCorners(xRange2, yRange2))
        {
            if (IsPointWithinRange(corner2, xRange1, yRange1))
            {
                return true;
            }
        }
        return false;
    }

    public static List<Vector2> GetCorners(Vector2 xRange, Vector2 yRange)
    {
        List<Vector2> corners = new List<Vector2>();
        for (int j = 0; j < 1; j++)
        {
            for (int i = 0; i < 1; i++)
            {
                Debug.Log("i: " + i + ", j: " + j);
                corners.Add(new Vector2(xRange[i], yRange[j]));
            }
        }
        return corners;
    }

    public static bool IsPointWithinRange(Vector2 point, Vector2 xRange, Vector2 yRange)
    {
        return point.x >= xRange[0] && point.x <= xRange[1] &&
            point.y >= yRange[0] && point.y <= yRange[1];
    }

    #endregion GEOMETRY


    #region EXTENSION METHODS

    //https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
    /// <summary>
    /// Extension method to check if a layer is in a layermask
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    #endregion EXTENSION METHODS


    #region DEBUG

    public static void DrawPoint(Vector3 _pos, float _length = 0.5f, float _duration = 1f, bool useGizmos = false)
    {
        Vector3 x1 = _pos - _length * new Vector3(1f, 0f, 0f);
        Vector3 x2 = _pos + _length * new Vector3(1f, 0f, 0f);
        Vector3 y1 = _pos - _length * new Vector3(0f, 1f, 0f);
        Vector3 y2 = _pos + _length * new Vector3(0f, 1f, 0f);
        Vector3 z1 = _pos - _length * new Vector3(0f, 0f, 1f);
        Vector3 z2 = _pos + _length * new Vector3(0f, 0f, 1f);

        if (!useGizmos)
        {
            Debug.DrawLine(x1, x2, Color.red,   _duration);
            Debug.DrawLine(y1, y2, Color.green, _duration);
            Debug.DrawLine(z1, z2, Color.blue,  _duration);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(x1, x2);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(y1, y2);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(z1, z2);
        }
    }

    public static void DrawCircle(Vector3 center, float radius, EPlanToUse planToUse, Color? c = null, int amountOfSides = 36)
    {
        if (amountOfSides < 3)
            return;

        Color color = c ?? Color.white;

        float angle, x, y, z, yz;
        Vector3 p1, p2;

        for (int i = 0; i < amountOfSides; i++)
        {
            angle = i * 360f / amountOfSides;
            x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            yz = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            y = planToUse == EPlanToUse.XY ? yz : 0f;
            z = planToUse == EPlanToUse.XZ ? yz : 0f;
            p1 = center + new Vector3(x, y, z);

            angle = (i + 1) * 360f / amountOfSides;
            x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            yz = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            y = planToUse == EPlanToUse.XY ? yz : 0f;
            z = planToUse == EPlanToUse.XZ ? yz : 0f;
            p2 = center + new Vector3(x, y, z);

            Debug.DrawLine(p1, p2, color);
        }
    }

    public static void DrawRectangle(Vector2 xRange, float y, Vector2 zRange, Color color, bool useGizmos = false)
    {
        float xMin = xRange[0];
        float xMax = xRange[1];
        float zMin = zRange[0];
        float zMax = zRange[1];

        Vector3 botLeft = new Vector3(xMin, y, zMin);
        Vector3 botRight = new Vector3(xMax, y, zMin);
        Vector3 topLeft = new Vector3(xMin, y, zMax);
        Vector3 topRight = new Vector3(xMax, y, zMax);

        if (!useGizmos)
        {
            Debug.DrawLine(botLeft, botRight, color);
            Debug.DrawLine(botRight, topRight, color);
            Debug.DrawLine(topRight, topLeft, color);
            Debug.DrawLine(topLeft, botLeft, color);
        }
        else
        {
            Gizmos.color = color;
            Gizmos.DrawLine(botLeft, botRight);
            Gizmos.DrawLine(botRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, botLeft);
        }
    }

    #endregion DEBUG
}
