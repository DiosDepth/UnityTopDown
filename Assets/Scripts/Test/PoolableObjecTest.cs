using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObjecTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        Invoke("DestroySelf", 2);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroySelf()
    {
        TDPoolManager.instance.PushObj(this.name, this.gameObject);
    }
}
