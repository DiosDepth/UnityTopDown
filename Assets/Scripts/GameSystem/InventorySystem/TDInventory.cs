using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDEnums;
using UnityEngine.Events;

namespace TDEnums
{
    public enum InventoryEventType
    {
        PickRequest,
        ContentChanged,
        ItemUseRequest,
        ItemUsing,
        ItemUsed,
        Selected,
    }
}

public struct InventoryEvent
{
    
    public InventoryEventType evttype;
    public string targetinventoryname;
    public TDInventoryItemBase inventoryitem;
    public int quantity;
    public int slotindex;

    public InventoryEvent( InventoryEventType m_type, string m_targetInventoryName, TDInventoryItemBase m_item, int m_quantity, int m_slot)
    {
        
        evttype = m_type;
        targetinventoryname = m_targetInventoryName;
        inventoryitem = m_item;
        quantity = m_quantity;
        slotindex = m_slot;
    }

    public static InventoryEvent e;
    public static void Trigger(InventoryEventType m_type, string m_targetInventoryName, TDInventoryItemBase m_item, int m_quantity, int m_slot)
    {
        
        e.evttype = m_type;
        e.targetinventoryname = m_targetInventoryName;
        e.inventoryitem = m_item;
        e.quantity = m_quantity;
        e.slotindex = m_slot;
        TDEventManager.TriggerEvent<InventoryEvent>(e);
    }
}

public struct InventoryItemInfo
{
    public TDInventoryItemBase item;
    public int inventoryCount;

}


public class TDInventory : MonoBehaviour,EventListener<InventoryEvent>
{
  
    public string inventoryName;
    public TDInventoryDisplayer targetDisplayer;
    public bool canExpand = false;
    /// <summary>
    /// 是否为静态的Inventory，如果是静态，则固定大小，并且每个Item有自己的固定Slot，同类只能替换不能叠加和同时持有2个相同的Item
    /// </summary>
    public bool isSingleInventory = true;

    public List<TDInventoryItemBase> contents = new List<TDInventoryItemBase>();
    public TDCharacter owner;

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
        this.StartListening<InventoryEvent>();
       
    }

    public void OnEvent(InventoryEvent evt)
    {
        switch (evt.evttype)
        {
            case InventoryEventType.PickRequest:
                //AddItem(evt.inventoryitem, evt.quantity, evt.inventoryitem.pickerInstance.PickFaild, evt.inventoryitem.pickerInstance.PickSuccess);
                
                if(owner != null)
                {
                    TDItemPicker picker = owner.GetComponent<TDCharacterAbilityPickUp>().currentTouchItemPicker.GetFirst();
                    AddItem(evt.inventoryitem, evt.quantity, picker.PickFaild, picker.PickSuccess);
                }
                break;
            case InventoryEventType.ContentChanged:
                break;
            case InventoryEventType.ItemUseRequest:
                UseItemRequest(evt.slotindex,evt.quantity);
                break;
            case InventoryEventType.ItemUsing:
                UseItem(evt.slotindex, evt.quantity);
                break;
            case InventoryEventType.ItemUsed:
                
                break;
            case InventoryEventType.Selected:
                break;
        }
    }

        public void SetTargetDisplayer(TDInventoryDisplayer displayer)
    {
        targetDisplayer = displayer;
    }



    public virtual bool AddItem(TDInventoryItemBase m_item, int m_count,UnityAction<TDInventoryItemBase> failCallBack, UnityAction<TDInventoryItemBase> successCallBack)
    {
        if(m_item.isStackItem)
        {
            if(contents.Contains(m_item))
            {
                TDInventoryItemBase item = GetItemInInventory(m_item.itemName);
                if(item.totalCountInventory >= m_item.maxStackCount || m_item.maxStackCount <= 1)
                {
                    Debug.Log("max stack count" + item.itemName);
                    if (failCallBack != null)
                    {
                        failCallBack(item);
                    }
                    return false;
                }
                else
                {
                    int total = Mathf.Clamp(item.totalCountInventory + m_count, 0, m_item.maxStackCount);
                    item.totalCountInventory = total;
                    if(successCallBack != null)
                    {
                        successCallBack(item);
                    }
                    InventoryEvent.Trigger(InventoryEventType.ContentChanged, item.targetInventoryName, item, m_count, item.slotIndex);
                    return true;
                }
            }
        }
        TDInventoryItemBase t_item = m_item.GetCopy();

        if (isSingleInventory)
        {
            
            t_item.slotIndex = 0;
            if(contents.Count ==0)
            {
                contents.Add(t_item);
                InventoryEvent.Trigger( InventoryEventType.ContentChanged, t_item.targetInventoryName, t_item, m_count, t_item.slotIndex);
                if (successCallBack != null)
                {
                    successCallBack(t_item);
                }
                return true;
            }
            else
            {
                if (contents[0] != null)
                {
                    if(contents[0].itemName.Equals(t_item.itemName))
                    {
                        if(failCallBack != null)
                        {
                            failCallBack(t_item);
                            
                        }
                        return false;
                    }
                    else
                    {
                        DropItem(0);
                        if (contents.Count <= 0)
                        {
                            contents.Add(t_item);
                        }
                        else
                        {
                            contents[0] = t_item;
                        }
                        InventoryEvent.Trigger(InventoryEventType.ContentChanged, t_item.targetInventoryName, t_item, m_count, t_item.slotIndex);
                        if (successCallBack != null)
                        {
                            successCallBack(t_item);
                        }
                        return true;
                    }
                }

            }

        }

        //是否到达了inventory最大数量。若没有到达。则添加

        if(contents.Count >= targetDisplayer.maxSlotCount)
        {
            Debug.Log("max Inventory slot count");
            if (failCallBack != null)
            {
                failCallBack(m_item);
            }
            return false;
        }

        t_item.slotIndex = contents.Count-1;
        contents.Add(t_item);
        InventoryEvent.Trigger(InventoryEventType.ContentChanged, t_item.targetInventoryName, t_item, m_count, t_item.slotIndex);
        if (successCallBack != null)
        {
            successCallBack(t_item);
        }
        return true;
    }

    public int GetQuantity(TDInventoryItemBase m_item)
    {
        if(!contents.Contains(m_item))
        {
            return 0;
        }
        TDInventoryItemBase item = contents.Find((i => i.itemName.Equals(m_item.itemName)));
        return item.totalCountInventory;
    }

    public TDInventoryItemBase GetItemInInventory(TDInventoryItemBase m_item)
    {
        int index;
        if (!contents.Contains(m_item))
        {
            return null;
        }
        else
        {
           index = contents.IndexOf(m_item);
        }

        return contents[index];
    }

    public TDInventoryItemBase GetItemInInventory(string name)
    {
        return contents.Find((item) => item.itemName.Equals(name));
    }

    public int GetIndexInInventory(TDInventoryItemBase m_item)
    {
        return contents.IndexOf(m_item);
    }


    public int GetIndexInInventory(string name)
    {
        return contents.IndexOf(contents.Find((item) => item.itemName.Equals(name)));
    }

    public void ChangeItem(TDInventoryItemBase m_item)
    {

    }

    public void DropItem(int slotIndex)
    {
        if(slotIndex >= contents.Count)
        {
            return;
        }
        if(contents[slotIndex] == null)
        {
            return;
        }

        GameObject obj = Instantiate<GameObject>(contents[slotIndex].pickerPrefab, null);
        if(obj == null)
        {
            Debug.Log("can't instantiate picker prefab in " + contents[slotIndex].itemName);
            return;
        }
        TDInventoryItemBase item = contents[slotIndex];
        obj.GetComponent<TDItemPicker>().lastUsedTime = item.lastUseStamp;
        obj.GetComponent<TDItemPicker>().dropTime = Time.time;

        obj.transform.position = owner.transform.position;
        obj.transform.rotation = Quaternion.identity;

        int count = item.totalCountInventory;
        obj.GetComponent<TDItemPicker>().itemCount = count;
        contents.RemoveAt(slotIndex);
        InventoryEvent.Trigger(InventoryEventType.ContentChanged, inventoryName, item, count, slotIndex);
    }
    //实际的使用item
    public void UseItem(int slotIndex, int qutity)
    {
        if(!IsItemValid(slotIndex))
        {
            Debug.Log("item slotIndex out of range");
            return;
        }
        if(contents[slotIndex].isStackItem)
        {
            contents[slotIndex].totalCountInventory -= qutity;

            contents[slotIndex].Use();
            if(contents[slotIndex].totalCountInventory <= 0)
            {
                contents.RemoveAt(slotIndex);
                InventoryEvent.Trigger(InventoryEventType.ContentChanged, inventoryName, contents[slotIndex], 1, slotIndex);
            }
        }
        else
        {
            contents[slotIndex].Use();
        }
    }
    //请求使用item。判断各种条件
    public void UseItemRequest(int slotIndex,int qutity)
    {
        if(!contents[slotIndex].canUse)
        {
            Debug.Log("Can't use this item : " + contents[slotIndex].itemName);
            return;
        }
        if(contents[slotIndex].isStackItem && contents[slotIndex].totalCountInventory - qutity <= 0)
        {
            Debug.Log("Not enough item count :" + contents[slotIndex].itemName);
            return;
        }
        //是否有CD设置的item
        if (contents[slotIndex].cdTime == 0)
        {
            InventoryEvent.Trigger(InventoryEventType.ItemUsing, inventoryName, contents[slotIndex], qutity, slotIndex);
        }
        else
        {
            if (contents[slotIndex].GetCDRatio() < 1)
            {
                Debug.Log("CD Ratio less then 1");
                return;
               
            }
            InventoryEvent.Trigger(InventoryEventType.ItemUsing, inventoryName, contents[slotIndex], qutity, slotIndex);

        }
    }

    public void EquipItem(TDInventoryItemBase m_item)
    {
        if (m_item.isEquip != true)
        {
            Debug.Log("this Item can't be equiped ! cus it's not a equiped item");
            return;
        }

        if(m_item.type == TDEnums.ItemType.MainWeapon)
        {

        }

        if(m_item.type == TDEnums.ItemType.SecondWeapon)
        {

        }
    
    }

    public bool IsItemValid(int index)
    {
        if(index >= contents.Count)
        {
            return false;
        }
        return true;
    }

    public bool IsItemValid(TDInventoryItemBase item)
    {
        if(!contents.Contains(item))
        {
            return false;
        }
        return true;
    }
}