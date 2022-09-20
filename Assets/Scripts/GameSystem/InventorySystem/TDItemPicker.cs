using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TDEnums;

public class TDItemPicker : MonoBehaviour, IHeapItem<TDItemPicker>
{
    public Sprite icon;
    public string itemname;
    public ItemType type;
    public GameObject itemPrefab;
    public TDInventoryItemBase inventoryItem;
    public int itemCount;
    public string targetInventoryName;
    public TDInventory targetInventory;
    public BoxCollider pickerTrigger;
    public float triggerDelayTime = 0.3f;

    public float lastUsedTime;
    public float dropTime;


    private int heapindex;
    public int HeapIndex { get { return heapindex; }  set { heapindex = value; } }

    public TDCharacter pickerCaster;

    public int CompareTo(TDItemPicker target)
    {
        float sqrDistance = Vector3.SqrMagnitude(TDLevelManager.instance.currentPlayer.transform.position - this.transform.position);
        float targetsqrDistance = Vector3.SqrMagnitude(TDLevelManager.instance.currentPlayer.transform.position - target.transform.position);

        if(sqrDistance < targetsqrDistance)
        {
            return 1;
        }
        if(sqrDistance > targetsqrDistance)
        {
            return -1;
        }
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Initialization()
    {
        StartCoroutine(ActiveDelay(triggerDelayTime));
    }

    public void Pick(TDCharacter caster)
    {
        pickerCaster = caster;
        if(targetInventory == null)
        {
            targetInventory = TDInventoryManager.instance.GetInventoryByName(targetInventoryName);
        }
        //inventoryItem.pickerInstance = this;
        inventoryItem.lastUseStamp = lastUsedTime;
        InventoryEvent.Trigger(InventoryEventType.PickRequest, targetInventoryName, inventoryItem, itemCount,0);
    }

    public void PickFaild(TDInventoryItemBase item)
    {
        pickerCaster.GetComponent<TDCharacterAbilityPickUp>().PickFaild();
        Debug.Log(this.transform.name + " pick Faild");
    }

    public void PickSuccess(TDInventoryItemBase item)
    {
        pickerCaster.GetComponent<TDCharacterAbilityPickUp>().PickSuccess();
        Debug.Log(this.transform.name + " has been pickuped");

        //targetInventory.AddItem(inventoryItem);
        pickerTrigger.enabled = false;
        Destroy(this.gameObject);
    }

    public IEnumerator ActiveDelay(float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        pickerTrigger.enabled = true;
    }
}
