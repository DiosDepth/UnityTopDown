using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMathTools 
{
/// <summary>
///  gaven a transfrom(m_trs) and a facing direction (m_dir), rotate transfom forward to m_dir with m_rotSpeed
/// </summary>
/// <param name="m_trs"></param>
/// <param name="m_forward"></param>
/// <param name="m_rotSpeed"></param>
    public static void ApplyRotationWithDirectionXY(Transform m_trs, Vector3 m_forward, float m_rotSpeed)
    {
        Transform tempTrs;
        if (m_trs == null)
        {
            return;
        }
        else
        {
            tempTrs = m_trs;
        }
        Quaternion rot = Quaternion.FromToRotation(Vector3.right, m_forward);

        m_trs.rotation = Quaternion.RotateTowards(m_trs.rotation, rot, m_rotSpeed * Time.deltaTime);
    }
    public static List<Vector2> DivideAngleByCountXY(Vector2 m_dir, float m_angle, int m_dividecount )
    {
        List<Vector2> templist = new List<Vector2>();
        int anglecount = m_dividecount -1;
        float averageangle = m_angle / anglecount;
        Vector2 startdir = RotateVectorXY(m_dir, m_angle / 2 * -1);
       
        for (int i = 0; i <= anglecount; i++)
        {
            if(i == 0)
            {
                templist.Add(startdir);
            }
            else
            {
                templist.Add(RotateVectorXY(templist[i - 1], averageangle));
            }

        }
        return templist;
    }

    public static Vector2 RotateVectorXY(Vector2 m_dir, float m_angle)
    {
        float radian = m_angle * Mathf.Deg2Rad;
        float _x = m_dir.x * Mathf.Cos(radian) - m_dir.y * Mathf.Sin(radian);
        float _y = m_dir.x * Mathf.Sin(radian) + m_dir.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }


    public static Vector3 ToVector3(this Vector2 m_vec2)
    {
        return new Vector3(m_vec2.x, m_vec2.y, 0);
    }

   
}
