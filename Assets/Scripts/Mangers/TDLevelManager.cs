using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace TDEnums
{
    public enum LevelState
    {
        None,
        LevelLoadRequest,
        LevelLoading,
        LevelLoaded,
        levelActive,
        LevelPrepare,
        LevelStart,
        LevelPause,
        LevelEnd,
        LevelRestart,
    }


}

public struct TDLevelEvent
{
    public LevelState type;
    public string levelName;
    public int index;
    public AsyncOperation asy;
    public TDLevelEvent(LevelState m_type, string m_levelname, int m_index, AsyncOperation m_asy)
    {
        type = m_type;
        levelName = m_levelname;
        index = m_index;
        asy = m_asy;
    }

    static TDLevelEvent e;
    public static void Trigger(LevelState m_type, string m_levelname, int m_index, AsyncOperation m_asy)
    {
        e.type = m_type;
        e.levelName = m_levelname;
        e.index = m_index;
        e.asy = m_asy;
        TDEventManager.TriggerEvent<TDLevelEvent>(e);
    }

}

public class TDLevelManager : Singleton<TDLevelManager>,EventListener<TDLevelEvent>
{
    [HideInInspector]
    AsyncOperation async;
    public TDCharacter currentPlayer;
    public List<TDCharacter> AIList = new List<TDCharacter>();

    public GameDataABInfo AIDataInfo;
    public Dictionary<string, AIDataInfo> AIDataDic = new Dictionary<string, AIDataInfo>();
    public Transform startPoint;

    public TDStateMachine<LevelState> levelState;
    public GameObject playerPrefab;

    public bool isPaused = false;
    private AssetBundle ABData;
    private float defaultFixedDeltaTime;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        
    }
   public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void Initialization()
    {
        base.Initialization();
        Debug.Log("TDLevelManager : " + instance.gameObject.name);
        //startPoint = GameObject.Find("startPoint").GetComponent<Transform>();

        levelState = new TDStateMachine<LevelState>(this.gameObject);
        levelState.currentState = LevelState.None;
        //pooler.InitialPooler();
        this.StartListening<TDLevelEvent>();

        TDManagerEvent.Trigger(ManagerEventType.InitialCompleted, this.gameObject.name);
    }

    public void LevelInfoCollection()
    {
        Debug.Log("LevelInfoCollection");
        startPoint = GameObject.Find("StartPoint").GetComponent<Transform>();
    }

    public IEnumerator LoadingScene(int m_index, UnityAction<AsyncOperation> callback = null)
    {
        bool isLoadingCompleted = false;
        async = SceneManager.LoadSceneAsync(m_index);
        async.allowSceneActivation = false;
        while(!isLoadingCompleted)
        {
            //在scene加载过程中触发LevelLoading事件，并且传入Async数据
            TDLevelEvent.Trigger(LevelState.LevelLoading, SceneManager.GetSceneByBuildIndex(m_index).name, m_index, async);
            if (!async.isDone && async.progress >= 0.9f)
            {
                isLoadingCompleted = true;
                
                yield return null;
            }
            yield return null;
        }
       
        if (callback != null)
        {
            callback(async);
        }
    }

    public IEnumerator WaitForActiveScene(int m_index, AsyncOperation async,UnityAction<AsyncOperation> callback = null)
    {
        bool isactive = false;
        while(!isactive)
        {
            if(Keyboard.current.anyKey.wasPressedThisFrame)
            {
                isactive = true;
                
                async.allowSceneActivation = true;
                yield return null;
            }
            yield return null;
        }
       
        if (callback != null)
        {
            callback(async);
        }
    }


    public void OnEvent(TDLevelEvent evt)
    {
        switch(evt.type)
        {
            case LevelState.LevelLoadRequest:
                levelState.ChangeState(LevelState.LevelLoadRequest);
                //开始加载Scene
                StartCoroutine(LoadingScene(evt.index, (async) =>
                {
                    TDLevelEvent.Trigger(LevelState.LevelLoaded, evt.levelName, evt.index, async);
                }));
               // TDLevelEvent.Trigger(LevelState.LevelLoading, evt.levelName, evt.index, null);
                break;
            case LevelState.LevelLoading:
                levelState.ChangeState(LevelState.LevelLoading);


                break;
            case LevelState.LevelLoaded:
                levelState.ChangeState(LevelState.LevelLoaded);
                StartCoroutine(WaitForActiveScene(evt.index, evt.asy, (async) =>
                {
                    TDLevelEvent.Trigger(LevelState.levelActive, evt.levelName, evt.index, async);
                }));
                break;
            case LevelState.levelActive:
                levelState.ChangeState(LevelState.levelActive);
                TDGameEvent.Trigger(GameState.GamePrepare);
                
                break;
            case LevelState.LevelPrepare:
                levelState.ChangeState(LevelState.LevelPrepare);
                LevelInfoCollection();
                SpawnPlayer();
                TDCameraManager.instance.ProcessCamera();
                ProcessCharacter();
                TDGameEvent.Trigger(GameState.GameStart);
                
                
                break;
            case LevelState.LevelStart:
                levelState.ChangeState(LevelState.LevelStart);
                
                
                // ProcessAI();
                break;
            case LevelState.LevelPause:
                levelState.ChangeState(LevelState.LevelPause);
                ToggleLevelPause();
                break;
            case LevelState.LevelEnd:
                levelState.ChangeState(LevelState.LevelEnd);

                break;
            case LevelState.LevelRestart:
                levelState.ChangeState(LevelState.LevelRestart);
                break;
        }
    }

    public void ToggleLevelPause()
    {
        if(isPaused)
        {
            //Unpause Level
            isPaused = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = defaultFixedDeltaTime;

            //Active Game UI
            //TDUIManager.instance.SwitchUIMode(UIMode.GameUIMode);
            Debug.Log("TDLevelManager : " + "Unpaused " + Time.timeScale);
        }
        else
        {
            //Pause Level
            isPaused = true;
            Time.timeScale = 0f;
            defaultFixedDeltaTime = Time.fixedDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * Time.fixedDeltaTime;

            //Active Pause UI
            //TDUIManager.instance.SwitchUIMode(UIMode.PausedUIMode);

            Debug.Log("TDLevelManager : "+ "Paused " + Time.timeScale);


            
        }
    }

    private void PreparePlayer()
    {
        SpawnPlayer();

    }

    public void LevelPrepare()
    {

    }

    public void LevelStart()
    {

    }

    public void LevelEnd()
    {

    }

    public void LevelRestart()
    {

    }

    private void GetAllCharacter()
    {
        
        TDCharacter[] array;
        array = GameObject.FindObjectsOfType<TDCharacter>();
        for (int i = 0; i < array.Length; i++)
        {
            if(array[i].characterType == TDEnums.CharacterType.AI)
            {
                AIList.Add(array[i]);
            }
            else
            {
                currentPlayer = array[i];
            }
        }
    }

    //spanw and initial player instance;
    public void SpawnPlayer()
    {
        if(playerPrefab == null)
        {
            Debug.Log("TDLevelManager : " + "can't find player prefab!");
            return;
        }
        if (currentPlayer == null)
        {
            if (startPoint == null)
            {
                startPoint = this.GetComponent<Transform>();
                currentPlayer = Instantiate(playerPrefab).GetComponent<TDCharacter>();
                currentPlayer.gameObject.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
          
            }
            else
            {
                currentPlayer = Instantiate(playerPrefab).GetComponent<TDCharacter>();
                currentPlayer.gameObject.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);

            }
        }
        else
        {
            
            Debug.Log("TDLevelManager : " + "already has a player in the scene!");
        }

    }

    public void SpawnAI(GameObject prefab, Transform trs)
    {
           
    }

    public void ProcessCharacter()
    {
        if(currentPlayer == null)
        {
            Debug.Log("TDLevelManager : " + "can't find any player instance in the scene!");
            return;
        }
        currentPlayer.Initialization();
        currentPlayer.isUpdateAbility = true;
        

    }

    public void UnProcessCharacter()
    {
        if(currentPlayer == null)
        {
            Debug.Log("TDLevelManager : " + "can't find any player instance in the scene!");
            return;
        }
        currentPlayer.isUpdateAbility = false;
        TDCameraManager.instance.UnProcessCamera();
    }


    private void ProcessAI()
    {
        for (int i = 0; i < AIList.Count; i++)
        {
            AIList[i].Initialization();
            AIList[i].isUpdateAbility = true;
        }
        //throw new NotImplementedException();
    }

    public void UnProcessAI()
    {

    }
}
