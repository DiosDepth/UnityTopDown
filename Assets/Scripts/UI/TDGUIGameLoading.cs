using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[System.Serializable]
public class ShowWorkShopCompletedEvent : UnityEvent
{

}
[System.Serializable]
public class LoadingCompletedEvent : UnityEvent
{

}

public class TDGUIGameLoading : TDGUIBase ,EventListener<TDDataManagerEvent>
{
    [Header("---LoadingSettings---")]
    public float loadingProgress;

    [SerializeField]
    public LoadingCompletedEvent OnLoadingCompleted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }
    public override void Initialization()
    {
        this.StartListening<TDDataManagerEvent>();
    }

    public void OnEvent(TDDataManagerEvent evt)
    {
        switch (evt.evtType)
        {
            case TDEnums.TDDataManagerEventType.DataLoadingRequest:

                break;
            case TDEnums.TDDataManagerEventType.DataLoading:
                GetGUIComponent<Slider>("GUI_GameLoading_Slider").value = evt.progress;
                break;
            case TDEnums.TDDataManagerEventType.DataLoaded:
                
                GetGUIComponent<Slider>("GUI_GameLoading_Slider").value = evt.progress;
                GetGUIComponent<TMP_Text>("GUI_GameLoading_Text").text = "Please waitting";
                OnLoadingCompleted.Invoke();
                break;
        }
    }
    public override void Hidden()
    {
        base.Hidden();
        this.StopListening<TDDataManagerEvent>();
    }

}