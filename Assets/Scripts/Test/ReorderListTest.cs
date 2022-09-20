using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malee.List;

[System.Serializable]
public class ReorderListTest : MonoBehaviour
{
    [Reorderable(null, "Action", null)]
    public TDAIActionsList TestAIList;

    /*[SerializeField]
    public List<TDAIState> StateList;*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
