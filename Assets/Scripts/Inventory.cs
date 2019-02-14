using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    RectTransform inventoryRect;
    float inventoryWidth, inventoryHeight;
    List<GameObject> allSlots;

    Slot from, to;

    public int slots;
    public int row;
    public float paddingLeft, paddingTop;
    public float slotSize;
    public GameObject slotPrefab;

    static int emptySlots;

    public static int EmptySlots
    {
        get
        {
            return emptySlots;
        }

        set
        {
            emptySlots = value;
        }
    }


    // Use this for initialization
    void Start () 
    {
        CreateLayout();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateLayout()
    {
        allSlots = new List<GameObject>();

        emptySlots = slots;

        inventoryWidth = (slots / row) * (slotSize + paddingLeft) + paddingLeft;
        inventoryHeight = row * (slotSize + paddingTop) + paddingTop;

        inventoryRect = GetComponent<RectTransform>();
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,inventoryHeight);

        int column = slots / row;

        for (int y = 0; y < row; y++ )
        {
            for (int x = 0; x < column; x++)
            {
                GameObject newSlot = Instantiate(slotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slot";

                newSlot.transform.SetParent(this.transform.parent);

                slotRect.localPosition = inventoryRect.localPosition + new Vector3(paddingLeft * (x + 1) + (slotSize * x), -paddingTop * (y + 1) - (slotSize * y));
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,slotSize);

                allSlots.Add(newSlot);
            }
        }
    }

    public bool AddItem(Items item)
    {
        if(item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else 
        {
            foreach(GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if(!tmp.IsEmpty)
                {
                    if(tmp.CurrentItem.type == item.type && tmp.IsAvailable)
                    {
                        tmp.AddItem(item);
                        return true;
                    }
                }
            }
            if (emptySlots > 0)
            {
                PlaceEmpty(item);
            }
        }
        return false;
    }

    bool PlaceEmpty(Items item)
    {
        if(emptySlots > 0)
        {
            foreach(GameObject slot in allSlots)
            {
                Slot temp = slot.GetComponent<Slot>();

                if(temp.IsEmpty)
                {
                    temp.AddItem(item);
                    emptySlots--;

                    return true;
                }
            }
        }

        return false;
    }

    public void MoveItem(GameObject clicked)
    {
        if(from == null)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.grey;
            }
        }
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
        }

        if (from != null && to != null)
        {
            Stack<Items> tmpTo = new Stack<Items>(to.Item);
            to.AddItems(from.Item);

            if (tmpTo.Count <= 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
        }
    }
}
