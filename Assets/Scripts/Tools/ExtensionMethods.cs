using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{


    public static bool Contains(this LayerMask mask, int layer)
    {
        return ((mask.value & (1 << layer)) > 0);
    }

    public static  bool HorizontalEqula(this Vector3 self, Vector3 target, float threshold = 0.1f)
    {
        Vector3 a = new Vector3(self.x, 0, self.z);
        Vector3 b = new Vector3(target.x, 0, target.z);

        if ((b-a).sqrMagnitude <= Mathf.Pow(threshold,2))
        {
            return true;
        }
        return false;
    }

    public static Vector3 HorizontalDirctionTo(this Vector3 self, Vector3 target)
    {
        Vector3 a = new Vector3(self.x, 0, self.z);
        Vector3 b = new Vector3(target.x, 0, target.z);

        return (b - a).normalized;
    }

    public static float HorizontalDistance(this Vector3 self, Vector3 target)
    {
        Vector3 a = new Vector3(self.x, 0, self.z);
        Vector3 b = new Vector3(target.x, 0, target.z);

        return Vector3.Distance(a, b);
    }

    public static float HorizontalSqrDistance(this Vector3 self, Vector3 target)
    {
        Vector3 a = new Vector3(self.x, 0, self.z);
        Vector3 b = new Vector3(target.x, 0, target.z);

        return (b - a).sqrMagnitude;
    }


}
