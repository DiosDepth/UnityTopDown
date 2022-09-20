using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TDInventoryManager : Singleton<TDInventoryManager>
{
    public string[] inventoriesName;
    public Dictionary<string, TDInventory> inventoryDic = new Dictionary<string, TDInventory>();
    public TDUIElementItemSlot currentSelectedSlot;


    private const string _baseFolderName = "/Save/";
    private const string _defaultFolderName = "InventoryManager";

    public override void Initialization()
    {
        base.Initialization();
        if(inventoriesName.Length == 0)
        {
            Debug.Log("No inventory has been initialized");
            return;
        }
        for (int i = 0; i < inventoriesName.Length; i++)
        {
            //创建所有的Inventory，并且设置他们的初始化和各种属性
            //创建好的inventory会添加进入inventoryDic
            GameObject obj = new GameObject(inventoriesName[i]);
            obj.transform.parent = this.transform;
            obj.AddComponent<TDInventory>();
            TDInventory t_inventory = obj.GetComponent<TDInventory>();
            t_inventory.Initialization();
            t_inventory.inventoryName = inventoriesName[i];

            inventoryDic.Add(inventoriesName[i], obj.GetComponent<TDInventory>());
        }

    }

    public TDInventory GetInventoryByName(string name)
    {
        TDInventory inventory;
        inventoryDic.TryGetValue(name, out inventory);
        return inventory;
    }

    protected virtual void CheckCurrentlySelectedSlot()
    {
        GameObject currentSelection = EventSystem.current.currentSelectedGameObject;
        if (currentSelection == null)
        {
            return;
        }
        TDUIElementItemSlot currentInventorySlot = currentSelection.gameObject.GetComponent<TDUIElementItemSlot>();
        if (currentInventorySlot != null)
        {
            currentSelectedSlot = currentInventorySlot;
        }
    }

    public bool SlotIndexCheck(int slotindex, string inventoryname)
    {
        
        return slotindex >= inventoryDic[inventoryname].contents.Count ? false : true;
    }
}
