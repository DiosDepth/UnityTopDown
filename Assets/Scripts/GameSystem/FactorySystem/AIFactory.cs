using System.Collections;
using TDEnums;
using UnityEngine;
using UnityEngine.Events;

namespace TDEnums
{
    public enum AISpawnMode
    {
        Random,
        RandomNoRepeat,
        RandomArea,
        Sequence,
        SinglePoint,
    }

    public enum AvaliableAI
    {
        None,
        Bear,
        Wolf,
    }

}
[System.Serializable]
public struct WaveInfo
{
    public float minWaveDelay;
    public float maxWaveDelay;
    public AISpawnInfo[] spawnInfo; 

    public WaveInfo(float m_minwavedelay = 0f, float m_maxwavedelay = 3,int m_spawninfocount = 1)
    {
        minWaveDelay = m_minwavedelay;
        maxWaveDelay = m_maxwavedelay;
        spawnInfo = new AISpawnInfo[m_spawninfocount];
    }
}

[System.Serializable]
public struct AISpawnInfo
{
    public AvaliableAI avaliableAI;
    public string name;
    public int count;
    public float minSpawnDelay;
    public float maxSpawnDelay;
    public Transform[] spawnPoints;
    public AISpawnMode spawnMode;

    public AISpawnInfo(AvaliableAI m_avaliableAI = AvaliableAI.None, string m_name = "", int m_count = 0, float m_minSpawnDelay = 0, float m_maxSpawnDelay = 3, int m_spawnpointcount = 1,AISpawnMode m_spawnmode = AISpawnMode.SinglePoint)
    {
        avaliableAI = m_avaliableAI;
        name = m_name;
        count = m_count;
        minSpawnDelay = m_minSpawnDelay;
        maxSpawnDelay = m_maxSpawnDelay;
        spawnMode = m_spawnmode;
        spawnPoints = new Transform[m_spawnpointcount];
    }

}

[System.Serializable]
public class AIFactory : GameObjectFactory
{
    public bool autoSpawn;
    public bool autoActiveAIAfterSpawn = true;
    public bool spawnPermission = true;
    public bool isFinished;

    [SerializeField]
    public WaveInfo[] waveInfo;
    public int curWaveIndex;
    public int curSpawnIndex;
    public int curCountIndex;

    public GameObject theLastOne;

    private int[] _randomArrayIndex;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnPermission)
        {
            StopCoroutine(SpawnWithDelay());
        }
    }
    public void WaveStart()
    {
        StartCoroutine(SpawnWithDelay());
        
    }

    public void WaveFinished()
    {

    }
    public void WaveReset()
    {
        
    }

    public IEnumerator SpawnWithDelay()
    {
        float temp_wavedelay;
        float temp_spanwdelay;
        for (int i = 0; i < waveInfo.Length; i++)
        {

            temp_wavedelay = Random.Range(waveInfo[i].minWaveDelay, waveInfo[i].maxWaveDelay);
            yield return new WaitForSeconds(temp_wavedelay);
            curWaveIndex = i;
            Debug.Log("Wave: " + "[" + i + "] delay for " + temp_wavedelay + " seconds" );
            for (int n = 0; n < waveInfo[i].spawnInfo.Length; n++)
            {
                temp_spanwdelay = Random.Range(waveInfo[i].spawnInfo[n].minSpawnDelay, waveInfo[i].spawnInfo[n].maxSpawnDelay);
                yield return new WaitForSeconds(temp_spanwdelay);
                curSpawnIndex = n;
                Debug.Log("Spawn: " + "[" + i + "] delay for " + temp_spanwdelay + " seconds");
                SpawnAIatPosition(waveInfo[i].spawnInfo[n], waveInfo[i].spawnInfo[n].spawnMode);
                
            }

            
        }
    }
    public void SpawnAIatPosition(AISpawnInfo m_spawninfo, AISpawnMode m_spawnmode)
    {
       
        Transform temp_TRS = null;
        switch (m_spawnmode)
        {
            case AISpawnMode.Random:
                for (int i = 0; i < m_spawninfo.count; i++)
                {
                    curCountIndex = i;
                    Spawn(m_spawninfo,false,(obj)=> {
                        theLastOne = obj;
                        int index = Random.Range(0, m_spawninfo.spawnPoints.Length - 1);
                        temp_TRS = m_spawninfo.spawnPoints[index];
                        if (temp_TRS == null)
                        {
                            Debug.Log("AIFactory : " + "No Found Transfrom infomation in Wave[" + curWaveIndex + "],Spawn[" + curSpawnIndex + "],Count[" + curCountIndex + "]");
                            return;
                        }
                        theLastOne.transform.SetPositionAndRotation(temp_TRS.position, temp_TRS.rotation);
                        if (autoActiveAIAfterSpawn)
                        {
                            ActiveAI(theLastOne);
                        }
                    });
                    
                }
                break;
            case AISpawnMode.RandomNoRepeat:
                if(m_spawninfo.spawnPoints.Length < m_spawninfo.count)
                {
                    Debug.Log("AIFactory : " + "No found enough spawn ponts of RandomNoRepeat in Wave[" + curWaveIndex + "],Spawn[" + curSpawnIndex + "]");
                    return;
                }
               /* int[] spawnpoints = new int[m_spawninfo.spawnPoints.Length];*/
                int[] randomPicks = GetRandomPickIndex(m_spawninfo.spawnPoints.Length, m_spawninfo.count);
                Debug.Log(randomPicks.Length +" : "+ randomPicks[0]);
                    /*new int[m_spawninfo.count];
                for (int i = 0; i < spawnpoints.Length; i++)
                {
                    spawnpoints[i] = i;
                }
                int spawnpointscount = spawnpoints.Length;
                //get a new array randomPicks ,pick random points from spawnpoints.
                ///算法原理：
                ///1从原数组中随机一个index出来，
                ///2放入新数组，这个数组就是最后随机的结果。
                ///3将原数组最后一个放入原数组[index],并且减少计数器，这样下一次取值就会避开最后一个数，避免了概率不同的问题
                for (int i = 0; i < m_spawninfo.count; i++)
                {
                    int index = Random.Range(0, spawnpointscount - 1);
                    randomPicks[i] = spawnpoints[index];
                    spawnpoints[index] = spawnpoints[spawnpointscount-1];
                    spawnpointscount--; 
                }*/
                //use randompicks to spawn ai in random position;
                for (int i = 0; i < randomPicks.Length; i++)
                {
                    curCountIndex = i;
                    Spawn(m_spawninfo,false,(obj)=> {
                        theLastOne = obj;
                        temp_TRS = m_spawninfo.spawnPoints[randomPicks[curCountIndex]];
                        if (temp_TRS == null)
                        {
                            Debug.Log("AIFactory : " + "No Found Transfrom infomation in Wave[" + curWaveIndex + "],Spawn[" + curSpawnIndex + "],Count[" + curCountIndex + "]");
                            return;
                        }
                        theLastOne.transform.SetPositionAndRotation(temp_TRS.position, temp_TRS.rotation);
                        if (autoActiveAIAfterSpawn)
                        {
                           ActiveAI(theLastOne);
                        }
                    });
                    
                }
                break;
            case AISpawnMode.RandomArea:
                break;
            case AISpawnMode.Sequence:
                for (int i = 0; i < m_spawninfo.count; i++)
                {
                    curCountIndex = i;
                    if (curCountIndex < m_spawninfo.spawnPoints.Length)
                    {
                        temp_TRS = m_spawninfo.spawnPoints[curCountIndex];
                    }
                    else
                    {
                        Debug.Log("AIFactory : " + " there is no spawn point to use when AISpawnMode = Sequence in Wave[" + curWaveIndex + "],Spawn[" + curSpawnIndex + "],Count[" + curCountIndex + "]");
                        return;
                    }
                }
                break;
            case AISpawnMode.SinglePoint:
                temp_TRS = m_spawninfo.spawnPoints[0];
                for (int i = 0; i < m_spawninfo.count; i++)
                {
                    curCountIndex = i;
                    Spawn(m_spawninfo, false, (obj) =>
                    {
                        theLastOne = obj;
                        if (temp_TRS == null)
                        {
                            Debug.Log("AIFactory : " + "No Found Transfrom infomation in Wave[" + curWaveIndex + "],Spawn[" + curSpawnIndex + "],Count[" + curCountIndex + "]");
                            return;
                        }
                        theLastOne.transform.SetPositionAndRotation(temp_TRS.position, temp_TRS.rotation);
                        if (autoActiveAIAfterSpawn)
                        {
                            ActiveAI(theLastOne);
                        }
                    });
                }
                
                break;
        }


        

    }

    public int[] GetRandomPickIndex(int m_arr_count, int m_pick_count)
    {
        int[] arr = new int[m_arr_count];
        int[] picks = new int[m_pick_count];

        for (int i = 0; i < m_arr_count; i++)
        {
            arr[i] = i;
        }

        int pickindex = m_arr_count;

        for (int i = 0; i < m_pick_count; i++)
        {
            int index = Random.Range(0, pickindex - 1);
            picks[i] = arr[index];
            arr[index] = arr[pickindex - 1];
            pickindex--;
        }
        return picks;
    }

    public void ActiveAI(GameObject m_ai)
    {
        m_ai.gameObject.SetActive(true);
        m_ai.GetComponent<AIBrain>().Initialization();
    
    }

    public void UpdateAbility()
    {
        Debug.Log("Test Update Ability");
        theLastOne.GetComponent<TDCharacter>().isUpdateAbility = true;
    }

    protected void Spawn(AISpawnInfo m_spawninfo,bool m_isactive,UnityAction<GameObject> callback)
    {
        string path = TDDataManager.instance.GetAIDataInfo(m_spawninfo.avaliableAI).PrefabPath;
        TDPoolManager.instance.GetObj(path, m_isactive, callback);

    }



}
