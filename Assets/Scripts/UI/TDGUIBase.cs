using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TDGUIBase : MonoBehaviour
{
    public GameObject owner;
    private Dictionary<string, List<UIBehaviour>> GUIComponentDic = new Dictionary<string, List<UIBehaviour>>();
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        FindChildGUIComponents<Button>();
        FindChildGUIComponents<Image>();
        FindChildGUIComponents<TMP_Text>();
        FindChildGUIComponents<Slider>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Initialization()
    {
        
    }

    protected virtual void UpdateFiller(Image m_filler, float m_filler_amount)
    {
        m_filler.fillAmount = m_filler_amount;
    }

    public virtual void Show()
    { }

    public virtual void Hidden()
    {
        //在UI被隐藏或者销毁前执行
        //比如StopListening
    }

    private void FindChildGUIComponents<T>() where T: UIBehaviour
    {
        T[] comps = this.GetComponentsInChildren<T>();
        string objname;
        for (int i = 0; i < comps.Length; i++)
        {
            objname = comps[i].gameObject.name;
            if (GUIComponentDic.ContainsKey(objname))
            {
                GUIComponentDic[objname].Add(comps[i]);
            }
            else
            {
                GUIComponentDic.Add(objname, new List<UIBehaviour>() { comps[i] });
            }
            
        }
    }

    protected T GetGUIComponent<T>(string compName) where T: UIBehaviour
    {
        if(GUIComponentDic.ContainsKey(compName))
        {
            for (int i = 0; i < GUIComponentDic[compName].Count; i++)
            {
                if(GUIComponentDic[compName][i] is T)
                {
                    return GUIComponentDic[compName][i] as T;
                }
            }
        }

        return null;
    }
}
