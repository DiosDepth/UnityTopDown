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
    public static void ApplyRotationWithDirection(Transform m_trs, Vector3 m_forward,Vector3 m_up, float m_rotSpeed)
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
        Quaternion lookRot = Quaternion.LookRotation(m_forward, m_up);
        m_trs.rotation = Quaternion.RotateTowards(m_trs.rotation, lookRot, m_rotSpeed * Time.fixedDeltaTime);
    }

}
