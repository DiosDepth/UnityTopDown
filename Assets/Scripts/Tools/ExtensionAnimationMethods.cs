using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionAnimationMethods
{

    public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
    {
        if (name == null || name == "") { return false; }
        AnimatorControllerParameter[] parameters = self.parameters;
        foreach (AnimatorControllerParameter currParam in parameters)
        {
            if (currParam.type == type && currParam.name == name)
            {
                return true;
            }
        }
        return false;
    }


    public static void UpdateAnimationParamFloat(this Animator self, string m_paramname, float m_value, List<string> m_paramlist)
    {
        if(m_paramlist.Contains(m_paramname))
        {
            self.SetFloat(m_paramname, m_value);
        }
    }

    public static void UpdateAnimationParamBool(this Animator self, string m_paramname, bool m_value, List<string> m_paramlist)
    {
        if (m_paramlist.Contains(m_paramname))
        {
            self.SetBool(m_paramname, m_value);
        }
    }

    public static void UpdateAnimationParamInt(this Animator self, string m_paramname, int m_value, List<string> m_paramlist)
    {
        if (m_paramlist.Contains(m_paramname))
        {
            self.SetInteger(m_paramname, m_value);
        }
    }

    public static void UpdateAnimationParamTrigger(this Animator self, string m_paramname, List<string> m_paramlist)
    {
        if (m_paramlist.Contains(m_paramname))
        {
            self.SetTrigger(m_paramname);
        }
    }

}
