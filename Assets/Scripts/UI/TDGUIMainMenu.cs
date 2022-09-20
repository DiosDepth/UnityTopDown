using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using TDEnums;




public class TDGUIMainMenu : TDGUIBase
{
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
        base.Initialization();
        GetGUIComponent<Button>("GUI_Play_Btn").onClick.AddListener(OnPlayBtnClicked);
        GetGUIComponent<Button>("GUI_Load_Btn").onClick.AddListener(OnLoadBtnClicked);
        GetGUIComponent<Button>("GUI_Options_Btn").onClick.AddListener(OnOptionBtnClicked);
        GetGUIComponent<Button>("GUI_Quit_Btn").onClick.AddListener(OnQuitBtnClicked);
    }

    public void SceneLoading()
    {
        TDGameEvent.Trigger(GameState.SceneLoading);
    }

    public void OnPlayBtnClicked()
    {
        TDGameEvent.Trigger(GameState.SceneLoading);
    }

    public void OnLoadBtnClicked()
    {

    }


    public void OnOptionBtnClicked()
    {
        TDUIManager.instance.ShowGUI<TDGUIOptionsMenu>("TDGUIOptionsMenu", UICanvasLayer.Mid, (gui) => 
        {
            gui.Initialization();
            TDUIManager.instance.HiddenGUI("TDGUIMainMenu");
        });
    }

    public void OnQuitBtnClicked()
    {
        Debug.Log("GameQuit");
        Application.Quit();
    }


   /* IEnumerator LoadingScreen()
    {
        async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        async.completed += OnAsyncCompleted;
        async.allowSceneActivation = false;
        while(!async.isDone)
        {
            slider.value = async.progress;
            if (async.progress >= 0.9f && TDGameManager.instance.ManagerPrepared())
            {
                slider.value = 1;
                text.text = "Press Space Key";
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    async.allowSceneActivation = true;
                    TDGameEvent.Trigger(GameState.GamePrepare);
                }
            }
            yield return null;
        }
    }*/


    /*private void OnAsyncCompleted(AsyncOperation asy)
    {
        if(asy.progress == 1f)
        {
            Debug.Log("Async is Completed");
    
        }
 
    }*/

   /* public void OnEvent(TDManagerEvent evt)
    {
        switch(evt.type)
        {
            case ManagerEventType.InitialCompleted:
                if(evt.managerName == "DataManager")
                {
                    isDataManagerInitialCompleted = true;
                }
                if(evt.managerName == "UIManager")
                {
                    isUIManagerInitialCompleted = true;
                }
                if(evt.managerName == "InputManager")
                {
                    isInputManagerInitialCompleted = true;
                }
                break;
        }

        if(isDataManagerInitialCompleted && 
            isUIManagerInitialCompleted &&
            isInputManagerInitialCompleted)
        {
            isManagersAllcompleted = true;
        }
    }*/

   /* public void ManagerIntialFlagReset()
    {
        isManagersAllcompleted = false;
        isDataManagerInitialCompleted = false;
        isUIManagerInitialCompleted = false;
        isInputManagerInitialCompleted = false;
    }
    */

}
