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
    public static List<float> DivideAngleByCount(Vector3 m_dir, float m_angle, int m_dividecount )
    {
        List<float> templist = new List<float>();
        return templist;
    }
}
