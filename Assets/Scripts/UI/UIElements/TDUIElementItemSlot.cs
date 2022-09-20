using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class TDUIElementItemSlot : Button
{
    public int slotindex;
    public Image backGround;
    public Image itemIcon;
    public Image CDMask;
    public TDInventoryDisplayer parentInventoryDisplayer;



    protected override void Start()
    {
        base.Start();
        this.onClick.AddListener(SlotClicked);
        this.onClick.AddListener(Use);
    }

    protected void Update()
    {
        UpdateCDMask();
    }

    public virtual void Initialization()
    {
        Image[] image = GetComponentsInChildren<Image>();
        for (int i = 0; i < image.Length; i++)
        {
            if(image[i].name == "BackGround")
            {
                backGround = image[i];
            }
            if (image[i].name == "ItemIcon")
            {
                itemIcon = image[i];
            }
            if (image[i].name == "CDMask")
            {
                CDMask = image[i];
            }
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        TDInventoryManager.instance.currentSelectedSlot = this;
        if(TDInventoryManager.instance.SlotIndexCheck(slotindex, parentInventoryDisplayer.targetInventoryName))
        {
            InventoryEvent.Trigger(TDEnums.InventoryEventType.Selected, parentInventoryDisplayer.targetInventoryName, parentInventoryDisplayer.targetInventory.contents[slotindex], 0, slotindex);
        }
    }

    public virtual void SlotClicked()
    {
        
    }

    public virtual void UpdateCDMask()
    {
        if(!TDInventoryManager.instance.SlotIndexCheck(slotindex,parentInventoryDisplayer.targetInventoryName))
        {
            return;
        }
        if(parentInventoryDisplayer.targetInventory.contents[slotindex].type != TDEnums.ItemType.UsedItem)
        {
            return;
        }
        if (parentInventoryDisplayer.targetInventory.contents[slotindex].cdTime != 0 )
        {
            CDMask.fillAmount = Mathf.Lerp(1, 0, parentInventoryDisplayer.targetInventory.contents[slotindex].GetCDRatio());
        }
    }

  

    public void Use()
    {
        if(!parentInventoryDisplayer.targetInventory.IsItemValid(slotindex))
        {
            Debug.Log("The slotindex that you require to use was out of range!");
            return;
        }
        InventoryEvent.Trigger(TDEnums.InventoryEventType.ItemUseRequest, parentInventoryDisplayer.targetInventoryName, parentInventoryDisplayer.targetInventory.contents[slotindex],1, slotindex);
    }


}
