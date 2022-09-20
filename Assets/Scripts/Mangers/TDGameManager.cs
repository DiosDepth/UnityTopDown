using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TDEnums;
using System.IO;

namespace TDEnums
{
    public enum GameState
    {
        ManagerPrepare,
        DataLoading,
        MenuStart,
        SceneLoading,
        GamePrepare,
        GameStart,
        GamePaused,
        GameOver,
        GameReset,
        GameEnd,
    }

    public enum ManagerEventType
    {
        InitialCompleted,
        IsReady,
    }


}

public struct TDGameEvent
{
    public GameState type;
    public TDGameEvent(GameState m_type)
    {
        type = m_type;
    }
    static TDGameEvent e;
    public static void Trigger(GameState m_type)
    {
        e.type = m_type;
        TDEventManager.TriggerEvent<TDGameEvent>(e);
    }
}


public struct TDManagerEvent
{
    public ManagerEventType type;
    public string managerName;
    public TDManagerEvent (ManagerEventType m_type, string m_managername)
    {
        type = m_type;
        managerName = m_managername;
    }
    static TDManagerEvent e;
    public static void Trigger(ManagerEventType m_type, string m_managername)
    {
        e.type = m_type;
        e.managerName = m_managername;
        TDEventManager.TriggerEvent<TDManagerEvent>(e);
    }
}

[System.Serializable]
public struct ManagerInfo
{
    [SerializeField]
    public GameObject prefab;
    public string prefabName;
}

public class TDGameManager : Singleton<TDGameManager>, EventListener<TDGameEvent>
{

    public bool testMode = false;
    //public GameObject playerPrefab;
    public TDStateMachine<GameState> gameState;
    public string managerResourcePath = @"GamePrefab\Managers\";
    [Header("ManagerInfo")]
    [SerializeField]
    public ManagerInfo levelManager;
    [SerializeField]
    public ManagerInfo dataManager;
    [SerializeField]
    public ManagerInfo inputSystemManager;
    [SerializeField]
    public ManagerInfo uiManager;
    [SerializeField]
    public ManagerInfo objectPoolManager;
    [SerializeField]
    public ManagerInfo cameraManager;
    [SerializeField]
    public ManagerInfo inventoryManager;
    [SerializeField]
    public ManagerInfo saveLoadManager;



    private long _totalmanagerLength;
    private long _curLength;
    //public GameObject levelManagerPrefab;
    //public GameObject inputManagerPrefab;
    //public GameObject uiManagerPrefab;
    //public GameObject objectPoolManagerPrefab;
    //public GameObject cameraManagerPrefab;
    //public GameObject dataManagerPrefab;



    //[Header("ManagerInstance")]
    // public TDLevelManager levelManager;
    //public TDInputManager inputManager;
    //public TDUIManager uiManager;
    //public TDGameObjectPoolManager objectPoolManager;
    //public TDCameraManager cameraManager;
    //public TDDataManager dataManager;


    
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
   
    }
    public override void  Start()
    {
        Debug.Log("TDGameManager : " + instance.gameObject.name);
        this.StartListening<TDGameEvent>();
        gameState = new TDStateMachine<GameState>(instance.gameObject, false);
        
        TDGameEvent.Trigger(GameState.ManagerPrepare);
        if (testMode)
        {
            InitiateManagers();
        }
    }

    public string GetManagerResPath(ManagerInfo m_manager)
    {
        return managerResourcePath + m_manager.prefabName;
    }


    public void OnEvent(TDGameEvent evt)
    {
        Debug.Log("TDGameManager.gameState : " + evt.type);
        switch (evt.type)
        {
            case GameState.ManagerPrepare:
                //初始化各种manager
                gameState.ChangeState(GameState.ManagerPrepare);
                if(!TDDataManager.InstanceExsist())
                {
                    dataManager.prefab = Resources.Load<GameObject>(GetManagerResPath(dataManager));
                    Instantiate(dataManager.prefab);
                    TDDataManager.instance.Initialization();
                }
                if(!TDUIManager.InstanceExsist())
                {
                    uiManager.prefab = Resources.Load<GameObject>(GetManagerResPath(uiManager));
                    Instantiate(uiManager.prefab);
                    TDUIManager.instance.Initialization();
                }
                if (!TDLevelManager.InstanceExsist())
                {
                    levelManager.prefab = Resources.Load<GameObject>(GetManagerResPath(levelManager));
                    Instantiate(levelManager.prefab);
                    TDLevelManager.instance.Initialization();
                }
                TDUIManager.instance.ShowGUI<TDGUIBackGround>("TDGUIBackGround", UICanvasLayer.Bot, (gui) =>
                {
                    gui.Initialization();
                });

                TDUIManager.instance.ShowGUI<TDGUIWellcomPage>("TDGUIWellcomPage", UICanvasLayer.Mid, (gui) =>
                {
                    gui.Initialization();
                });

                //TDGameEvent.Trigger(GameState.GameLoading);
                break;
            case GameState.DataLoading:
                //读取各种CSVData
                gameState.ChangeState(GameState.DataLoading);
                
                TDUIManager.instance.ShowGUI<TDGUIGameLoading>("TDGUIGameLoading", UICanvasLayer.Mid, (gui) => 
                {
                    gui.Initialization();
                    TDUIManager.instance.HiddenGUI("TDGUIWellcomPage");
                    //完成后回调，转换GameState状态到MenuStart
                    TDDataManager.instance.CollecteCSVDataInfo(()=>
                    {
                        TDGameEvent.Trigger(TDEnums.GameState.MenuStart);
                    });
                    
                });
                

                break;
            case GameState.MenuStart:
                //显示了主菜单。等待玩家选择
                gameState.ChangeState(GameState.MenuStart);
                TDUIManager.instance.HiddenGUI("TDGUIGameLoading");
                TDUIManager.instance.FadeInGUI<TDGUIMainMenu>("TDGUIMainMenu", UICanvasLayer.Mid,0,1,0.5f,0,(gui) => 
                {
                    gui.Initialization();
                });

                break;
            case GameState.SceneLoading:
                //玩家选择开始游戏，进行Scene的载入
                gameState.ChangeState(GameState.SceneLoading);
         
                TDUIManager.instance.ShowGUI<TDGUITransitionPage>("TDGUITransitionPage", UICanvasLayer.Top, (gui) =>
                {
                    gui.Initialization();
                    TDUIManager.instance.HiddenGUI("TDGUIMainMenu");
                    TDLevelEvent.Trigger(LevelState.LevelLoadRequest, SceneManager.GetSceneByBuildIndex(1).name, 1, null);
                    //此时游戏流程控制交给了LeveManager， 等待关卡载入结束，并且玩家激活的时候。转到GamePrepare
                });

                break;
            case GameState.GamePrepare:
                gameState.ChangeState(GameState.GamePrepare);
                InitiateManagers();
                //提前加载PlayerHUD，在关卡准备阶段需要使用这个。
                TDUIManager.instance.ShowGUI<TDGUIPlayerHUD>("TDGUIPlayerHUD", UICanvasLayer.Mid, (gui) =>
                {
                    gui.Initialization();
                    //通知关卡管理器准备各种物件初始化，比如玩家出生，玩家位置，摄影机，UI等。
                    TDLevelEvent.Trigger(LevelState.LevelPrepare, SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().buildIndex, null);
                });

                break;
            case GameState.GameStart:
                //所有准备完毕，开始游戏，开始关卡。
                gameState.ChangeState(GameState.GameStart);
                TDUIManager.instance.FadeOutGUI("TDGUITransitionPage");
                TDUIManager.instance.FadeOutGUI("TDGUIBackGround");
                TDLevelEvent.Trigger(LevelState.LevelStart, SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().buildIndex, null);
                break;
            case GameState.GameOver:
                gameState.ChangeState(GameState.GameOver);
                break;
            case GameState.GameReset:
                gameState.ChangeState(GameState.GameReset);
                break;
            case GameState.GameEnd:
                gameState.ChangeState(GameState.GameEnd);
                break;
        }
    }
    public void InitiateManagers()
    {
        if (!TDPoolManager.InstanceExsist())
        {
            objectPoolManager.prefab = Resources.Load<GameObject>(GetManagerResPath(objectPoolManager));
            Instantiate(objectPoolManager.prefab);
            TDPoolManager.instance.Initialization();
        }

        if (!TDInventoryManager.InstanceExsist())
        {
            inventoryManager.prefab = Resources.Load<GameObject>(GetManagerResPath(inventoryManager));
            Instantiate(inventoryManager.prefab);
            TDInventoryManager.instance.Initialization();
        }
        //if (!TDInputManager.InstanceExsist())
        //{
        //    inputManager.prefab = Resources.Load<GameObject>(GetManagerResPath(inputManager));
        //    Instantiate(inputManager.prefab);
        //    TDInputManager.instance.Initialization();
        //}
        if (!InputSystemManager.InstanceExsist())
        {
            inputSystemManager.prefab = Resources.Load<GameObject>(GetManagerResPath(inputSystemManager));
            Instantiate(inputSystemManager.prefab);
            InputSystemManager.instance.Initialization();
        }

        if (!TDCameraManager.InstanceExsist())
        {
            cameraManager.prefab = Resources.Load<GameObject>(GetManagerResPath(cameraManager));
            Instantiate(cameraManager.prefab);
            TDCameraManager.instance.Initialization();
        }
        if(!SaveLoadManager.InstanceExsist())
        {
            saveLoadManager.prefab = Resources.Load<GameObject>(GetManagerResPath(saveLoadManager));
            Instantiate(saveLoadManager.prefab);
            SaveLoadManager.instance.Initialization();
        }
    }




    public bool ManagerPrepared()
    {
        if(TDGameManager.InstanceExsist()&&
            TDUIManager.InstanceExsist()&&
            TDDataManager.InstanceExsist()&&
            InputSystemManager.InstanceExsist()&&
            TDLevelManager.InstanceExsist()&&
            TDPoolManager.InstanceExsist()&&
            TDCameraManager.InstanceExsist()&&
            SaveLoadManager.InstanceExsist())
        {
            return true;
        }
        else
        {
            return false;
        }
       
    }


    // Update is called once per frame
    public override void Update()
    {
        
    }

   
}
