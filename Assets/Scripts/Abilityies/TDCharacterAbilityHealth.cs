using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CharacterAbilityEvent_Death
{
    public GameObject target;
    public GameObject caster;
    public CharacterAbilityEvent_Death(GameObject m_caster, GameObject m_target )
    {
        target = m_target;
        caster = m_caster;
    }
    public static CharacterAbilityEvent_Death e;
    public static void Trigger(GameObject m_caster, GameObject m_target)
    {
        e.target = m_target;
        e.caster = m_caster;
        TDEventManager.TriggerEvent<CharacterAbilityEvent_Death>(e);
    }
}

public struct CharacterAbilityEvent_HPChange
{
    public GameObject caster;
    public GameObject target;
    public float changeAmount;
    public float HpRate;
    public CharacterAbilityEvent_HPChange(GameObject  m_caster, GameObject m_target, float m_change_amount, float m_hp_rate)
    {
        caster = m_caster;
        target = m_target;
        changeAmount = m_change_amount;
        HpRate = m_hp_rate;
    }

    public static CharacterAbilityEvent_HPChange e;
    public static void Trigger(GameObject m_caster, GameObject m_target, float m_change_amount, float m_hp_rate)
    {
        e.caster = m_caster;
        e.target = m_target;
        e.changeAmount = m_change_amount;
        e.HpRate = m_hp_rate;
        TDEventManager.TriggerEvent<CharacterAbilityEvent_HPChange>(e);
    }
}


public class TDCharacterAbilityHealth : TDCharacterAbility
{

    public float currentHP;
    public GameObject lastCaster;
    public TDGUIBase targetHUD;
    public bool isShowHPBarOnHead;

    public delegate void CharacterAbilityHealthDelegate_HPChange(float m_hprate);
    public event CharacterAbilityHealthDelegate_HPChange CharacterAbilityHealthEvent_HPChange;


    public Vector3 barGUIOffset;

 
    protected float damage;

    // Start is called before the first frame update


    public override void InitialAbility()
    {
        base.InitialAbility();
 
       
        switch(owner.characterType)
        {
            case TDEnums.CharacterType.Player:
                InitiatePlayerHPBarGUI();
                break;
            case TDEnums.CharacterType.AI:
                InitiateAIHPBarGUI();
                break;
        }
    }

    public float CalculateDamage(float m_damage)
    {
        float tempDamage = 0;

        if(m_damage - owner.attribute.def <= 0)
        {
            tempDamage = 1;
        }
        else
        {
            tempDamage = m_damage - owner.attribute.def;
        }
        return tempDamage;
    }

    public void TakeDamage(float m_damage,GameObject m_caster,GameObject m_target)
    {
        float temp_damage = CalculateDamage(m_damage);
        Debug.Log("TDCharacterAbilityHealth : " + " take damage of " + temp_damage);
        currentHP -= temp_damage;
        lastCaster = m_caster;
        CharacterAbilityEvent_HPChange.Trigger(m_caster, m_target, temp_damage,GetHPRate(currentHP, owner.attribute.maxHP));
        // FloatChange.Trigger("PlayerHPUpdate", GetHPRate(currentHP, owner.attribute.maxHP));
        CharacterAbilityHealthEvent_HPChange(GetHPRate(currentHP, owner.attribute.maxHP));

        if (currentHP <= 0)
        {
            Death();
            return;
        }
    }

    public float CalculateHeal(float m_heal)
    {
        float temp_heal = 0;
        temp_heal = m_heal * owner.attribute.healMultipler;

        return temp_heal;
    }


    public void Heal(float m_heal, GameObject m_caster, GameObject m_target)
    {
        float temp_heal = CalculateDamage(m_heal);
        Debug.Log("TDCharacterAbilityHealth : " + " Heal of " + temp_heal);
        currentHP += temp_heal;
        currentHP = Mathf.Clamp(currentHP, 0, owner.attribute.maxHP);
        lastCaster = m_caster;
        CharacterAbilityEvent_HPChange.Trigger(m_caster,m_target, temp_heal, GetHPRate(currentHP, owner.attribute.maxHP));
        CharacterAbilityHealthEvent_HPChange(GetHPRate(currentHP, owner.attribute.maxHP));
        if (currentHP >= owner.attribute.maxHP)
        {
            currentHP = owner.attribute.maxHP;
            return;
        }
    }


    public void HealOverTime(float m_heal,float m_time, GameObject m_caster)
    {
        //todo...
    }

    public void Death()
    {
        switch (owner.characterType)
        {
            case TDEnums.CharacterType.Player:
                currentHP = 0;
                TDLevelManager.instance.ToggleLevelPause();
                break;
            case TDEnums.CharacterType.AI:
                currentHP = 0;
                DestroyHUD();
                //死亡之前需要吧所有相关的component，引用，和设置清空
                TDPoolableGameObject obj = GetComponent<TDPoolableGameObject>();
                if (obj != null)
                {
                    obj.Destroy();
                }
                else
                {
                    Destroy(this.gameObject);
                }
                CharacterAbilityEvent_Death.Trigger(this.gameObject, lastCaster);
                break;
        }


    }

    private void InitiatePlayerHPBarGUI()
    {
        TDGUIPlayerHUD targetHUD = TDUIManager.instance.GetGUI<TDGUIPlayerHUD>("TDGUIPlayerHUD");
        if (targetHUD != null)
        {
            targetHUD.owner = this.gameObject;
            currentHP = owner.attribute.maxHP;
            CharacterAbilityHealthEvent_HPChange += (targetHUD as TDGUIPlayerHUD).UpdateHPBar;
            CharacterAbilityHealthEvent_HPChange(GetHPRate(currentHP, owner.attribute.maxHP));
        }
        else
        {
            Debug.Log("TDCharacterAbilityHealth : " + "can't find TDUIManager.instance.uiCanvas!");
        }
        
        //StartCoroutine(TakeDamageTest());
    }



    private void InitiateAIHPBarGUI()
    {
        TDPoolManager.instance.GetObj("GamePrefab/GUI/AIInfoHUD", true, (obj) => 
        {
            targetHUD = obj.GetComponent<TDGUIAIInfoHUD>();
            targetHUD.owner = this.gameObject;
            currentHP = owner.attribute.maxHP;
            TDUIManager.instance.SetGUIParent(TDEnums.UICanvasLayer.Bot, obj);
            CharacterAbilityHealthEvent_HPChange += (targetHUD as TDGUIAIInfoHUD).UpdateHPBar;
            CharacterAbilityHealthEvent_HPChange(GetHPRate(currentHP, owner.attribute.maxHP));

            UpdateTargetHUDPosition();
        });


       //_hpBar.transform.position = TDCameraManager.instance.currentActiveCamera.WorldToScreenPoint(transform.position);
        //TDUIManager.instance.enemyHPBarList.Add(_hpBar);
    }

    private float GetHPRate(float m_curHP, float m_maxHP)
    {
        if (m_curHP <= 0)
        {
            return 0;
        }
        float tempRate = 1;
        tempRate = m_curHP / m_maxHP;

        tempRate = Mathf.Clamp(tempRate, 0, 1);
        return tempRate;

    }


    private void DestroyHUD()
    {
        switch (owner.characterType)
        {
            case TDEnums.CharacterType.Player:
                CharacterAbilityHealthEvent_HPChange -= (targetHUD as TDGUIPlayerHUD).UpdateHPBar;
                //关闭对应的HUD
                break;
            case TDEnums.CharacterType.AI:
                CharacterAbilityHealthEvent_HPChange -= (targetHUD as TDGUIAIInfoHUD).UpdateHPBar;

                TDPoolableGameObject obj = targetHUD.GetComponent<TDPoolableGameObject>();
                if (obj != null)
                {
                    obj.Destroy();
                }
                else
                {
                    Destroy(this.gameObject);
                }
               
                //销毁UI，退回到objectPool;
                break;
        }
      
        //
        //TDUIManager.instance.enemyHPBarList.Remove(_hpBar);
    }

    public override void AfterUpdateAbility()
    {
        base.AfterUpdateAbility();
        UpdateTargetHUDPosition();

    }

   private void UpdateTargetHUDPosition()
    {
        switch(owner.characterType)
        {
            case TDEnums.CharacterType.Player:
                break;
            case TDEnums.CharacterType.AI:
                if(targetHUD != null)
                {
                    targetHUD.transform.position = TDCameraManager.instance.currentActiveCamera.WorldToScreenPoint(this.gameObject.transform.position);
                }
              
                break;
        }
   
    }

}
