using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDPoolableGameObject : MonoBehaviour
{
   
    public string holderObjName;
    protected bool isUpdate = false;
    // Start is called before the first frame update
    private void OnEnable()
    {
        //Invoke("Destroy", 1f);

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
    public virtual void Destroy()
    {
        SetUpdate(false);
        TDPoolManager.instance.PushObj(this.name, this.gameObject);
    }

    public virtual void Active()
    {
    
        this.gameObject.SetActive(true);
    }

    public virtual void SetUpdate(bool m_value)
    {
        isUpdate = m_value;
    }

}
