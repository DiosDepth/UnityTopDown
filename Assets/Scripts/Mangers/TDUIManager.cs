using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TDEnums;
using UnityEngine.Events;


namespace TDEnums
{
    public enum UIMode
    {
        StartMenuUIMode,
        TransitionUIMode,
        GameUIMode,
        PausedUIMode,
    }

    public enum UICanvasLayer
    {
        Bot,
        Mid,
        Top,
        System,
    }
}

[System.Serializable]
public struct CanvasInfo
{
    [SerializeField]
    public GameObject prefab;
    public string prefabName;
    public Canvas canvas;
    public TDGUIBase info;
}




public class TDUIManager : Singleton<TDUIManager>, EventListener<TDGameEvent>
{
    public string canvasResPath;
    public AsyncOperation asy;
    public Dictionary<string, TDGUIBase> GUIDic = new Dictionary<string, TDGUIBase>();
    public RectTransform canvas;

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;


    //public CanvasInfo[] HUDCanvasInfo;
    //public CanvasInfo gameStartHudCanvas;
    //public CanvasInfo transitionHUDCanvas;
    //public CanvasInfo playerHUDCanvas;
    //public CanvasInfo inGameHUDCanvas;
    //public CanvasInfo pauseHUDCanvas;

    public Dictionary<string, Canvas> enemyInfoDic;
    public List<GameObject> enemyInfoCanvasList;
   


    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        
    }
    public override void Start()
    {

    }
    public override void Initialization()
    {
        base.Initialization();
        Debug.Log("TDUIManager : " + instance.gameObject.name);
        this.StartListening<TDGameEvent>();
        //创建对应的canvas和EventSystem
        GameObject obj = TDDataManager.instance.LoadRes<GameObject>(GetCanvasResPath("Core/Canvas"));
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);
        //找到对应的canvas层级
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        obj = TDDataManager.instance.LoadRes<GameObject>(GetCanvasResPath("Core/EventSystem"));
        GameObject.DontDestroyOnLoad(obj);
        /*
         InitialGameStartHUDCanvas();
         InitialTransitionHUDCanvas();
         InitialPlayerHUDCanvas();
         InitialInGameHUDCanvas();
         InitialPauseHUDCanvas();
         */

        //HUDCanvas = FindObjectOfType<Canvas>();
        /* if(playerHUDCanvas == null)
         {
             InitialPlayerHUDCanvas();
         }
         enemyHPBarList = new List<GameObject>();
         uiPoolerHolder = GetComponent<TDGameObjectPoolerHolder>();
         TDManagerEvent.Trigger(ManagerEventType.InitialCompleted,this.gameObject.name );
         Debug.Log(playerHUDCanvas.transform.name);*/
    }
    public string GetCanvasResPath(CanvasInfo m_canvasinfo)
    {
        return canvasResPath + m_canvasinfo.prefabName;
    }

    public string GetCanvasResPath(string m_prefabname)
    {
        return canvasResPath + m_prefabname;
    }

    public void OnEvent(TDGameEvent evt)
    {
        switch (evt.type)
        {
            case GameState.ManagerPrepare:
                break;
            case GameState.DataLoading:

                break;
            case GameState.MenuStart:
            
                break;
            case GameState.SceneLoading:

                break;
            case GameState.GamePrepare:

                break;
            case GameState.GameStart:


                break;
            case GameState.GameOver:
      
                break;
            case GameState.GameReset:
    
                break;
            case GameState.GameEnd:
    
                break;
        }
    }

    public void SetGUIParent(UICanvasLayer layer, GameObject obj)
    {
        switch (layer)
        {
            case UICanvasLayer.Bot:
                obj.transform.SetParent(bot);
                break;
            case UICanvasLayer.Mid:
                obj.transform.SetParent(mid);
                break;
            case UICanvasLayer.Top:
                obj.transform.SetParent(top);
                break;
            case UICanvasLayer.System:
                obj.transform.SetParent(system);
                break;
        }
    }

        public void ShowGUI<T>(string guiname, UICanvasLayer layer = UICanvasLayer.Mid, UnityAction<T> callback = null) where T:TDGUIBase
    {
        if(GUIDic.ContainsKey(guiname))
        {
            GUIDic[guiname].Show();
            if(callback != null)
            {
                callback(GUIDic[guiname] as T);
            }
            return;
        }
        TDDataManager.instance.LoadResAsync<GameObject>(GetCanvasResPath(guiname), (obj) => 
        {
            Transform father = bot;
            switch (layer)
            {
                case UICanvasLayer.Bot:
                    father = bot;
                    break;
                case UICanvasLayer.Mid:
                    father = mid;
                    break;
                case UICanvasLayer.Top:
                    father = top;
                    break;
                case UICanvasLayer.System:
                    father = system;
                    break;
            }
            obj.transform.SetParent(father);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            T gui = obj.GetComponent<T>();

            gui.Show();
            GUIDic.Add(guiname, gui);
            if (callback != null)
            {
                callback(gui);
            }
        });
    }

    public void FadeInGUI<T>(string guiname, UICanvasLayer layer = UICanvasLayer.Mid, float start = 0, float end = 1, float lerptime = 0.3f, float enddelay = 0, UnityAction<T> callback = null) where T : TDGUIBase
    {
        if (GUIDic.ContainsKey(guiname))
        {
            GUIDic[guiname].Show();
            if (callback != null)
            {
                callback(GUIDic[guiname] as T);
            }
            return;
        }
        TDDataManager.instance.LoadResAsync<GameObject>(GetCanvasResPath(guiname), (obj) =>
        {
            Transform father = bot;
            switch (layer)
            {
                case UICanvasLayer.Bot:
                    father = bot;
                    break;
                case UICanvasLayer.Mid:
                    father = mid;
                    break;
                case UICanvasLayer.Top:
                    father = top;
                    break;
                case UICanvasLayer.System:
                    father = system;
                    break;
            }
            obj.transform.SetParent(father);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            T gui = obj.GetComponent<T>();
            CanvasGroup cg = gui.GetComponent<CanvasGroup>();
            if(cg != null)
            {
              StartCoroutine(FadeCanvasGroupByTime(cg, start, end, lerptime, enddelay, () =>
                {
                    gui.Show();
                    GUIDic.Add(guiname, gui);
                    if (callback != null)
                    {
                        callback(gui);
                    }
                }));
            }
            else
            {
                gui.Show();
                GUIDic.Add(guiname, gui);
                if (callback != null)
                {
                    callback(gui);
                }
            }


        });
    }

    public void HiddenGUI(string guiname)
    {
        if(GUIDic.ContainsKey(guiname))
        {
            GUIDic[guiname].Hidden();
            GameObject.Destroy(GUIDic[guiname].gameObject);
            GUIDic.Remove(guiname);
        }
    }

    public void FadeOutGUI(string guiname, float start = 1, float end = 0, float lerptime = 0.3f, float enddelay = 0, UnityAction callback = null)
    {
        if (GUIDic.ContainsKey(guiname))
        {
            
            CanvasGroup cg = GUIDic[guiname].GetComponent<CanvasGroup>();
            if(cg != null)
            {
               StartCoroutine( FadeCanvasGroupByTime(cg, start, end, lerptime, enddelay, () => 
                {
                    GUIDic[guiname].Hidden();
                    GameObject.Destroy(GUIDic[guiname].gameObject);
                    GUIDic.Remove(guiname);
                }));
            }
            else
            {
                GUIDic[guiname].Hidden();
                GameObject.Destroy(GUIDic[guiname].gameObject);
                GUIDic.Remove(guiname);
            }

        }
    }



    public T GetGUI<T>(string guiname) where T: TDGUIBase
    {
        if(GUIDic.ContainsKey(guiname))
        {
            return GUIDic[guiname] as T;
        }
        return null;
    
    }



    // Update is called once per frame
    public override void Update()
    {
        if(startUpdate)
        {
        
        }

    }
    /*
    public void InitialGameStartHUDCanvas()
    {
        TDDataManager.instance.LoadAsync<GameObject>(GetCanvasResPath(gameStartHudCanvas), (obj) => 
        {
            gameStartHudCanvas.canvas = obj.GetComponent<Canvas>();
            gameStartHudCanvas.info = null;
            DontDestroyOnLoad(obj);
        });
    }
    public void InitialTransitionHUDCanvas()
    {
        TDDataManager.instance.LoadAsync<GameObject>(GetCanvasResPath(transitionHUDCanvas), (obj) =>
        {
            transitionHUDCanvas.canvas = obj.GetComponent<Canvas>();
            transitionHUDCanvas.info = obj.GetComponent<TDGUITransition>();
            DontDestroyOnLoad(obj);
            transitionHUDCanvas.canvas.gameObject.SetActive(false);
        });
    }
    public void InitialPlayerHUDCanvas()
    {
        TDDataManager.instance.LoadAsync<GameObject>(GetCanvasResPath(playerHUDCanvas), (obj) =>
        {
            playerHUDCanvas.canvas = obj.GetComponent<Canvas>();
            playerHUDCanvas.info = obj.GetComponent<TDGUIPlayerHUD>();
            DontDestroyOnLoad(obj);
            playerHUDCanvas.canvas.gameObject.SetActive(false);

        });
        //playerHUDCanvas = Instantiate(Resources.Load<GameObject>(canvasResPath));
        
    }
    public void InitialInGameHUDCanvas()
    {
        TDDataManager.instance.LoadAsync<GameObject>(GetCanvasResPath(inGameHUDCanvas), (obj) =>
        {
            inGameHUDCanvas.canvas = obj.GetComponent<Canvas>();
            inGameHUDCanvas.info = null; 
            DontDestroyOnLoad(obj);
            inGameHUDCanvas.canvas.gameObject.SetActive(false);
        });
    }
    public void InitialPauseHUDCanvas()
    {
        TDDataManager.instance.LoadAsync<GameObject>(GetCanvasResPath(pauseHUDCanvas), (obj) =>
        {
            pauseHUDCanvas.canvas = obj.GetComponent<Canvas>();
            pauseHUDCanvas.info = null;
            DontDestroyOnLoad(obj);
            pauseHUDCanvas.canvas.gameObject.SetActive(false);
        });
    }
    public void SwitchUIMode(UIMode m_mode)
    {
        switch (m_mode)
        {
            case UIMode.StartMenuUIMode:
                gameStartHudCanvas.canvas.gameObject.SetActive(true);
                transitionHUDCanvas.canvas.gameObject.SetActive(false);
                playerHUDCanvas.canvas.gameObject.SetActive(false);
                inGameHUDCanvas.canvas.gameObject.SetActive(false);
                pauseHUDCanvas.canvas.gameObject.SetActive(false);
                break;
            case UIMode.TransitionUIMode:
                gameStartHudCanvas.canvas.gameObject.SetActive(false);
                transitionHUDCanvas.canvas.gameObject.SetActive(true);
                playerHUDCanvas.canvas.gameObject.SetActive(false);
                inGameHUDCanvas.canvas.gameObject.SetActive(false);
                pauseHUDCanvas.canvas.gameObject.SetActive(false);
                break;
            case UIMode.GameUIMode:
                gameStartHudCanvas.canvas.gameObject.SetActive(false);
                transitionHUDCanvas.canvas.gameObject.SetActive(false);
                playerHUDCanvas.canvas.gameObject.SetActive(true);
                inGameHUDCanvas.canvas.gameObject.SetActive(true);
                pauseHUDCanvas.canvas.gameObject.SetActive(false);
                break;
            case UIMode.PausedUIMode:
                gameStartHudCanvas.canvas.gameObject.SetActive(false);
                transitionHUDCanvas.canvas.gameObject.SetActive(false);
                playerHUDCanvas.canvas.gameObject.SetActive(false);
                inGameHUDCanvas.canvas.gameObject.SetActive(false);
                pauseHUDCanvas.canvas.gameObject.SetActive(true);
                break;
        }

    }
    */


    public IEnumerator FadeCanvasGroupByTime(CanvasGroup cg,float start, float end, float lerptime = 0.5f,float enddelay = 0, UnityAction callback = null)
    {
        float temp_starttime = Time.time;
        float temp_timesincestarted = Time.time - temp_starttime;
        float temp_percentagecomplete = temp_starttime / lerptime;
        while(true)
        {
            temp_timesincestarted = Time.time - temp_starttime;
            temp_percentagecomplete = temp_timesincestarted / lerptime;
            float currentvalue = Mathf.Lerp(start, end, temp_percentagecomplete);
            cg.alpha = currentvalue;

            if (temp_percentagecomplete >= 1)
                break;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(enddelay);
        callback?.Invoke();
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator ChangeFillerByTime(Image m_filler,float m_curvalue, float m_targetvalue, float lerptime = 0.6f, UnityAction callback = null)
    {
        float temp_starttime = Time.time;
        float temp_timesincestarted = Time.time - temp_starttime;
        float temp_percentagecomplete = temp_starttime / lerptime;
        while (true)
        {
            temp_timesincestarted = Time.time - temp_starttime;
            temp_percentagecomplete = temp_timesincestarted / lerptime;
            float currentvalue = Mathf.Lerp(m_curvalue, m_targetvalue, temp_percentagecomplete);
            m_filler.fillAmount = currentvalue;

            if (temp_percentagecomplete >= 1)
                break;
            yield return new WaitForEndOfFrame();
        }
        callback?.Invoke();
    }

    public IEnumerator ChangeFillerBySpeed(Image m_filler, float m_curvalue, float m_targetvalue, float m_speed = 0.25f, UnityAction callback = null)
    {
        float temp_curvalue = m_curvalue;
        Debug.Log("StartCoroutine : change Filler By Speed");
        while (true)
        {

            if(temp_curvalue > m_targetvalue)
            {
                temp_curvalue -= m_speed * Time.deltaTime;
                temp_curvalue = Mathf.Clamp01(temp_curvalue);
                if(temp_curvalue <= m_targetvalue)
                {
                    temp_curvalue = m_targetvalue;
                    m_filler.fillAmount = temp_curvalue;
                    break;
                }
                m_filler.fillAmount = temp_curvalue;
            }
            else
            {
                temp_curvalue += m_speed * Time.deltaTime;
                temp_curvalue = Mathf.Clamp01(temp_curvalue);
                if (temp_curvalue >= m_targetvalue)
                {
                    temp_curvalue = m_targetvalue;
                    m_filler.fillAmount = temp_curvalue;
                    break;
                }
                m_filler.fillAmount = temp_curvalue;
            }
            yield return new WaitForEndOfFrame();
        }
        callback?.Invoke();
    }

    public float ChangeFillerBySpeed(float m_change, float m_target, float speed)
    {
        float fill = m_change;
        if (m_change > m_target)
        {
            fill = m_change - speed * Time.deltaTime;
            if (fill <= m_target)
            {
                fill = m_target;
                return fill;
            }
        }
        else
        {
            fill = m_change + speed * Time.deltaTime;
            if (fill >= m_target)
            {
                fill = m_target;
                return fill;
            }

        }
        return fill;
    }


}
