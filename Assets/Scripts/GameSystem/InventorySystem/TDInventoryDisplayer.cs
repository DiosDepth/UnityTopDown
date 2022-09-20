using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using TDEnums;

namespace TDEnums
{
    public enum InventoryDisplayerType
    {
        Static,
        Dynamic,
    }
}


public class TDInventoryDisplayer : MonoBehaviour, EventListener<InventoryEvent>
{
    public InventoryDisplayerType displayerType = InventoryDisplayerType.Static;
    public GameObject slotprefab;
    public Image broader;
    public string targetInventoryName;
    public TDInventory targetInventory;



    public GridLayoutGroup layoutGroup;
    public RectTransform rectTRS;
    public Vector2Int slotsize;
    public int maxSlotCount { get { return slotsize.x * slotsize.y; } }
    public TDUIElementItemSlot[] slotContener;
    // Start is called before the first frame update


    private void OnEnable()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialization()
    {
        this.StartListening<InventoryEvent>();
        layoutGroup = GetComponent<GridLayoutGroup>();
        rectTRS = GetComponent<RectTransform>();

        //slotArry = new TDUIElementItemSlot[slotcount];
        ResizeLayoutGroup(slotsize.x,slotsize.y);
        SetNavigation();
        Focus();
    }

    public void OnEvent(InventoryEvent evt)
    {
        switch (evt.evttype)
        {
            case TDEnums.InventoryEventType.PickRequest:
               
                break;
            case TDEnums.InventoryEventType.ContentChanged:
                DrowItemIcon(evt.slotindex);
                break;
            case TDEnums.InventoryEventType.ItemUseRequest:
                break;
            case TDEnums.InventoryEventType.ItemUsing:
                break;
            case TDEnums.InventoryEventType.ItemUsed:
                break;
        }

    }

    public void SetSize(int x, int y)
    {
        slotsize.x = x;
        slotsize.y = y;
    }

    public void Focus(int index = 0)
    {
        switch (displayerType)
        {
            case InventoryDisplayerType.Static:
                //slotContener[index].Select();
                EventSystem.current.SetSelectedGameObject( slotContener[index].gameObject);
                EventSystem.current.firstSelectedGameObject = slotContener[index].gameObject;
                
                break;
            case InventoryDisplayerType.Dynamic:
                break;
        }
    }


        public void ResizeLayoutGroup(int x_count, int y_count)
    {
        ClearSlot();
        if(slotsize.x == 0 || slotsize.y == 0)
        {
            Debug.Log("slot size can't be zero");
            return;
        }
        rectTRS.sizeDelta = new Vector2(layoutGroup.padding.left + x_count * layoutGroup.cellSize.x + (x_count - 1) * layoutGroup.spacing.x + layoutGroup.padding.right,
            layoutGroup.padding.top + y_count * layoutGroup.cellSize.y + (y_count - 1) * layoutGroup.spacing.y + layoutGroup.padding.bottom);

        slotContener = new TDUIElementItemSlot[maxSlotCount];
        for (int i = 0; i < maxSlotCount; i++)
        {
            GameObject obj = Instantiate(slotprefab, this.transform);
            slotContener[i] = obj.GetComponent<TDUIElementItemSlot>();
            slotContener[i].slotindex = i;
            slotContener[i].parentInventoryDisplayer = this;
            slotContener[i].Initialization();
        }
       // SetNavigation();
    }

    public void SetNavigation()
    {
        int index = 0;
        for (int y = 0; y < slotsize.y; y++)
        {
            for (int x = 0; x < slotsize.x; x++)
            {
                Navigation navigation = slotContener[index].navigation;
                if(y != 0)
                {
                    navigation.selectOnUp = slotContener[index - slotsize.x];
                }
                if(y != slotsize.y-1)
                {
                    navigation.selectOnDown = slotContener[index + slotsize.x];
                }
                if(x != 0)
                {
                    navigation.selectOnLeft = slotContener[index - 1];
                }
                if(x != slotsize.x -1)
                {
                    navigation.selectOnRight = slotContener[index + 1];
                }
                slotContener[index].navigation = navigation;
                index++;

            }
        }
    }
    public void ClearSlot()
    {
        TDUIElementItemSlot[] objlist = layoutGroup.GetComponentsInChildren<TDUIElementItemSlot>();
        if(objlist.Length >0)
        {
            for (int i = 0; i < objlist.Length; i++)
            {

                DestroyImmediate(objlist[i].gameObject);
            }
        }

    }

    public void DrowItemIcon(int slotindex)
    {
        if(!TDInventoryManager.instance.SlotIndexCheck(slotindex, targetInventoryName))
        {
            slotContener[slotindex].itemIcon.sprite = null;
            return;
        }
        if(targetInventory.contents[slotindex] != null)
        {
            slotContener[slotindex].itemIcon.sprite = targetInventory.contents[slotindex].icon;
        }
        else
        {
            slotContener[slotindex].itemIcon.sprite = null;
        }
        
    }

    public void ResizeArray<T>(T[] arr)
    {

    }

    public void UseItem(int slotIndex)
    {

    }

    
}