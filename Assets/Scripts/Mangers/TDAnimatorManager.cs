using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TDAnimatorManager 
{
    
    public static void AddAnimatorParamaterIfExists(Animator ani, string param_name, AnimatorControllerParameterType param_type, List<string> param_list)
    {
        if(param_name == null || param_name == "")
        {
            Debug.Log("TDAnimatorManager : " + ani.transform.name + " got a empty paramater name! please check animator paramater name settings");
            return;
        }
        if(ani.HasParameterOfType(param_name,param_type))
        {
            param_list.Add(param_name);
        }
    }

    public static void UpdateBoolParamater(Animator ani, string param_name, bool value, List<string> param_list)
    {
        if(param_list.Contains(param_name))
        {
            ani.SetBool(param_name, value);
        }
    }
}
