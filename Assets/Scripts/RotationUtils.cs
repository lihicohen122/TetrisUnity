using UnityEngine;

public static class RotationUtils
{
    public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
    public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);

    public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };
}
