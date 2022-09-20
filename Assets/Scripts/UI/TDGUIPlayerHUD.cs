using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TDGUIPlayerHUD : TDGUIBase, EventListener<FloatChange>
{
    [Header("---HPBarSettings---")]
    public CanvasGroup HPBarGroup;
    public Image HPBar;
    public Image HPBarFiller;
    public Image HPBarDelayFiller;
    private bool startUpdateDelayFiller = false;

    public float barChangeSpeed = 0.5f;// per s


    [Header("---WeaponSlotGroupSettings---")]
    public CanvasGroup inventoryHotBar;
    public TDInventoryDisplayer mainWeaponDisplayer;
    public TDInventoryDisplayer secondWeaponDisplayer;
    public TDInventoryDisplayer usedItemDisplayer;


    // Start is called before the first frame update
    void Start()
    {
        
        //this.StartListening<FloatChange>();

       
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDelayFiller();
    }

    private void UpdateDelayFiller()
    {
        if (startUpdateDelayFiller)
        {
            HPBarDelayFiller.fillAmount = TDUIManager.instance.ChangeFillerBySpeed(HPBarDelayFiller.fillAmount, HPBarFiller.fillAmount, barChangeSpeed);
            if (HPBarDelayFiller.fillAmount == HPBarFiller.fillAmount)
            {
                startUpdateDelayFiller = false;
            }
        }
    }

    public void UpdateHPBar(float m_hprate)
    {
        UpdateFiller(HPBarFiller, m_hprate);
        startUpdateDelayFiller = true;
    }

    public void OnEvent(FloatChange evt)
    {
        switch(evt.varName)
        {
            case "PlayerHPUpdate":
                UpdateFiller(HPBarFiller, evt.value);
                //HPBarUpdateCoroutines.Enqueue(TDUIManager.instance.ChangeFillerByTime(HPBarDelayFiller, HPBarDelayFiller.fillAmount, evt.value));
                //StartCoroutine(TDUIManager.instance.ChangeFillerBySpeed(HPBarDelayFiller, HPBarDelayFiller.fillAmount, evt.value, 0.25f));
                startUpdateDelayFiller = true;
                break;
        }
    }

}
