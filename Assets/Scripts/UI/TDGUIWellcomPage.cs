using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDGUIWellcomPage : TDGUIBase
{
    public CanvasGroup group;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Show()
    {
        base.Show();
        if (group == null)
        {
            group = GetComponent<CanvasGroup>();
        }
        StartCoroutine(TDUIManager.instance.FadeCanvasGroupByTime(group, 0, 1, 0.25f,1f, () =>
            {
                TDGameEvent.Trigger(TDEnums.GameState.DataLoading);
            }));
    }


    public override void Initialization()
    {
        base.Initialization();
        if(group ==null)
        {
            group = GetComponent<CanvasGroup>();
        }
       
    }
}
