using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TDGUIOptionsMenu : TDGUIBase
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
        GetGUIComponent<Button>("GUI_Back_Btn").onClick.AddListener(OnBackBtnClicked);
    }

    public void OnBackBtnClicked()
    {
        
        TDUIManager.instance.ShowGUI<TDGUIMainMenu>("TDGUIMainMenu", TDEnums.UICanvasLayer.Mid, (gui) =>
        {
            gui.Initialization();
            TDUIManager.instance.HiddenGUI("TDGUIOptionsMenu");
        });
        
    }
}
