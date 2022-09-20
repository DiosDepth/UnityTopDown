using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class TDGUITransitionPage : TDGUIBase,EventListener<TDLevelEvent>
{
    public CanvasGroup canvasGroup;
    public Slider progressBar;
    public Image bgImage;
    public TextMeshProUGUI transitionText;

    private string defaultText;
    // Start is called before the first frame update
    void Start()
    {
        if(transitionText != null)
        {
            defaultText = transitionText.text;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Initialization()
    {
        base.Initialization();
        this.StartListening<TDLevelEvent>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Hidden()
    {
        base.Hidden();
        this.StopListening<TDLevelEvent>();
    }
    public void OnEvent(TDLevelEvent evt)
    {
        switch (evt.type)
        {
            case TDEnums.LevelState.None:
                break;
            case TDEnums.LevelState.LevelLoadRequest:
                break;
            case TDEnums.LevelState.LevelLoading:
                progressBar.value = evt.asy.progress;
                break;
            case TDEnums.LevelState.LevelLoaded:
                progressBar.value = 1;
                transitionText.text = "Press Any Key";
                break;
            case TDEnums.LevelState.levelActive:
                break;
            case TDEnums.LevelState.LevelPrepare:
                break;
            case TDEnums.LevelState.LevelStart:
                break;
            case TDEnums.LevelState.LevelPause:
                break;
            case TDEnums.LevelState.LevelEnd:
                break;
            case TDEnums.LevelState.LevelRestart:
                break;
        }
    }

    public void TransitionProgressUpdate(float m_progress)
    {
        progressBar.value = m_progress;
    }

    public void TransitionProgressCompleted(float m_progress = 1)
    {
        progressBar.value = m_progress;
        transitionText.text = "Press Any Key";
    }

    public void TransitionReset()
    {
        progressBar.value = 0;
        transitionText.text = defaultText;
        canvasGroup.alpha = 1;
    }
}
