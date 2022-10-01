using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FunctionTest : MonoBehaviour
{
    public Transform divideCenter;
    public List<Vector2> divideDir;
    public Vector3 todir = new Vector3();
    public float angle;
    public int divideCount;
    private void Awake()
    {
        
    }
    void Start()
    {
        divideDir = ExtensionMathTools.DivideAngleByCountXY(divideCenter.transform.up, angle, divideCount);


    }

    // Update is called once per frame
    void Update()
    {
        divideDir = ExtensionMathTools.DivideAngleByCountXY(divideCenter.transform.up, angle, divideCount);
        for (int i = 0; i < divideDir.Count; i++)
        {
            todir = new Vector3(divideDir[i].x, divideDir[i].y, 0f);
            Debug.DrawLine(divideCenter.position, todir * 5 + divideCenter.position, Color.green, 0.2f);
        }
    }
}

