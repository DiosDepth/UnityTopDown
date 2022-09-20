using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;
using UnityEngine.UI;

namespace TDEnums
{
    public enum ItemType
    {
        MainWeapon,
        SecondWeapon,
        UsedItem,
        PassiveItem,
    }

}


[CreateAssetMenu(menuName = "InventorySystem/InventoryItem")]
[System.Serializable]
public class TDInventoryItemBase : ScriptableObject
{
    public ItemType type = ItemType.UsedItem;
    public bool isEquip = false;
    public bool canUse = true;
    public bool isStackItem = false;
    public int maxStackCount;

    public string targetInventoryName;

    public GameObject itemPrefab;
    public GameObject pickerPrefab;

    //public TDItemPicker pickerInstance;

    public float cdTime = 0;
    public float lastUseStamp;
    /// <summary>
    /// 当拾取或者其他操作的时候添加多少个item
    /// </summary>

    /// <summary>
    /// 在Inventory里面的总数；
    /// </summary>
    public int totalCountInventory;
    public int slotIndex;


    [Header("---InventoryDisplaySetting---")]
    public Sprite icon;
    public string itemName;
    private TDInventory _targetInventory;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Initialization()
    {

    }

    public virtual TDInventoryItemBase GetCopy()
    {
        TDInventoryItemBase clone;
        string name = this.name;
        clone = Object.Instantiate(this) as TDInventoryItemBase;
        clone.name = name;
        return clone;
    }

    public virtual void Use()
    {
        lastUseStamp = Time.time;
        Debug.Log("ItemUsed ： "+itemName);
    }

    public virtual void Passive()
    {

    }

    public virtual float GetCDRatio()
    {
        if(cdTime == 0)
        {
            return 1;
        }
        else
        {
            return (Time.time - lastUseStamp) / cdTime;
        }
        
    }
}
