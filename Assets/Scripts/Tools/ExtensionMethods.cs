using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    //---LayerMaskOperation---
    public static bool Contains(this LayerMask mask, int layer)
    {
        return ((mask.value & (1 << layer)) > 0);
    }

    public static List<int> GetMaskValue(LayerMask mask)
    {
        List<int> tempvalue = new List<int>();
        for (int i = 0; i < 32; i++)
        {
            if ((mask.value & (1 << i)) != 0)
            {
                Debug.Log("HearLayerMaskValue : " + i);
                tempvalue.Add(i);
            }
        }

        return tempvalue;
    }


    //---2DVectorOperation---
    public static  bool EqualXY(this Vector3 self, Vector3 target, float threshold = 0.1f)
    {
        Vector3 a = new Vector3(self.x, self.y, 0);
        Vector3 b = new Vector3(target.x, target.y, 0);

        if ((b-a).sqrMagnitude <= Mathf.Pow(threshold,2))
        {
            return true;
        }
        return false;
    }

    public static Vector3 DirectionToXY(this Vector3 self, Vector3 target)
    {
        Vector3 a = new Vector3(self.x, self.y, 0);
        Vector3 b = new Vector3(target.x, target.y, 0);

        return (b - a).normalized;
    }

    public static float DistanceXY(this Vector3 self, Vector3 target)
    {
        Vector3 a = new Vector3(self.x, self.y, 0);
        Vector3 b = new Vector3(target.x, target.y, 0);

        return Vector3.Distance(a, b);
    }

    public static float SqrDistanceXY(this Vector3 self, Vector3 target)
    {
        Vector3 a = new Vector3(self.x, self.y, 0);
        Vector3 b = new Vector3(target.x, target.y, 0);

        return (b - a).sqrMagnitude;
    }


}
