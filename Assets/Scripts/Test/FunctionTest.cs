using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FunctionTest : MonoBehaviour
{
    public float a = 1;
    public float b = 0;
    public float t = 0.1f;
    //public GameObject obj;
    public Transform temp_TRS;
    public Queue<IEnumerator> queue = new Queue<IEnumerator>();
    bool isUpdate = false;
    GameObject gameObj;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {


        //queue.Enqueue(CoroutineA());
       //queue.Enqueue(CoroutineB());
        //queue.Enqueue(CoroutineC());
        //TDDataManager.instance.CollecteCSVDataInfo();

       // StartCoroutine(queue.Dequeue());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            string path = TDDataManager.instance.GetAIDataInfo(TDEnums.AvaliableAI.Bear).PrefabPath;
            TDPoolManager.instance.GetObj(path, true, (obj) => {
                gameObj = obj;
                obj.transform.SetPositionAndRotation(temp_TRS.position, temp_TRS.rotation);
                //obj.GetComponent<TDCharacter>().Initialization();
                isUpdate = true;
                //obj.GetComponent<TDCharacter>().isUpdateAbility = true;

               // obj.SetActive(true);



            });
            //TDPoolManager.instance.GetObj("GamePrefab/AI/Cube");
        }

        if (Input.GetMouseButtonDown(1))
        {
            string path = "GamePrefab/AI/Capsule";
            //isUpdate = true;
            TDPoolManager.instance.GetObj(path, true, (obj) =>
            {

                gameObj = obj;
                
                //gameObj.transform.position = temp_TRS.position;
                
                //StartCoroutine(NextFrame(() => { isUpdate = true;}));
                
                
                
                isUpdate = true;
                gameObj.transform.SetPositionAndRotation(temp_TRS.position, temp_TRS.rotation);
                //gameObj.SetActive(true);
                //isUpdate = true;
            });
            //TDPoolManager.instance.GetObj("GamePrefab/AI/Sphere");
        }

        //Debug.Log(Mathf.SmoothDamp(a,b, ref t, 0.2f));
    }

    public IEnumerator NextFrame(UnityAction fun = null)
    {
        yield return null;
        fun?.Invoke();
        StopCoroutine(NextFrame());
        
    }
    private void FixedUpdate()
    {
        if (isUpdate)
        {
            gameObj.GetComponent<CharacterController>().Move(Vector3.zero);
            
        }
    }
    public IEnumerator CoroutineA()
    {
        Debug.Log("StartCoroutine : A");
        yield return new WaitForSeconds(3);
        Debug.Log("StopCoroutine : A");
    }

    public IEnumerator CoroutineB()
    {
        Debug.Log("StartCoroutine : B");
        yield return new WaitForSeconds(3);
        Debug.Log("StopCoroutine : B");
    }

    public IEnumerator CoroutineC()
    {
        Debug.Log("StartCoroutine : C");
        yield return new WaitForSeconds(3);
        Debug.Log("StopCoroutine : C");
    }
}

