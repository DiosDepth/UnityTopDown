using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctionTest : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(FadeCavasGroup());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeCavasGroup()
    {
        yield return StartCoroutine(TDUIManager.instance.FadeCanvasGroupByTime(CanvasGroup, 0, 1, 5f, 0f,()=> 
        {
            Debug.Log("Fadein Completed");
            
        }));

        Debug.Log("StartWait");
        yield return new WaitForSeconds(1);
        StartCoroutine(TDUIManager.instance.FadeCanvasGroupByTime(CanvasGroup, 1, 0,5f,0f,()=> 
        {
            Debug.Log("Fadeout Completed");
            StopCoroutine(TDUIManager.instance.FadeCanvasGroupByTime(CanvasGroup, 0, 1, 5f,0f, () => { }));
        }));
    }
}