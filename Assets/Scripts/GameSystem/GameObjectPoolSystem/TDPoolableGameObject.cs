using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDPoolableGameObject : MonoBehaviour
{
   
    public string holderObjName;
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
    public virtual void Destroy()
    {
        TDPoolManager.instance.PushObj(this.name, this.gameObject);
    }

}
