using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TDGUIAIInfoHUD : TDGUIBase, EventListener<FloatChange>
{

    [Header("---HPBarSettings---")]
    public CanvasGroup HPBarGroup;
    public Image HPBar;
    public Image HPBarFiller;
    public Image HPBarDelayFiller;
    private bool startUpdateDelayFiller = false;

    public float barChangeSpeed = 0.5f;// per s
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDelayFiller();
    }

    public override void Initialization()
    {
        base.Initialization();
       // TDCharacter character = owner.GetComponent<TDCharacter>();
        TDCharacterAbilityHealth health = owner.GetComponent<TDCharacterAbilityHealth>();
        if(health != null)
        {
            HPBarFiller.fillAmount = health.currentHP;
            HPBarDelayFiller.fillAmount = health.currentHP;
        }
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
        switch (evt.varName)
        {
            case "AIHPUpdate":
                UpdateFiller(HPBarFiller, evt.value);
                //HPBarUpdateCoroutines.Enqueue(TDUIManager.instance.ChangeFillerByTime(HPBarDelayFiller, HPBarDelayFiller.fillAmount, evt.value));
                //StartCoroutine(TDUIManager.instance.ChangeFillerBySpeed(HPBarDelayFiller, HPBarDelayFiller.fillAmount, evt.value, 0.25f));
                startUpdateDelayFiller = true;
                break;
        }
    }
}
