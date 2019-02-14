using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    Stack<Items> item;
    public Text stackTxt;
    public Sprite slotEmpty;
    public Sprite slotHighlighted;

    public bool IsEmpty
    {
        get { return item.Count == 0; }
    }

    public bool IsAvailable
    {
        get
        {
            return CurrentItem.maxSize > item.Count;
        }
    }

    public Items CurrentItem
    {
        get  
        {
            return item.Peek();
        }
    }

    public Stack<Items> Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
        }
    }

    // Use this for initialization
    void Start () 
    {
        item = new Stack<Items>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = stackTxt.GetComponent<RectTransform>();

        int txtScaleFactor = (int) (slotRect.sizeDelta.x * 0.6);
        stackTxt.resizeTextMaxSize = txtScaleFactor;
        stackTxt.resizeTextMinSize = txtScaleFactor;

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,slotRect.sizeDelta.y);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,slotRect.sizeDelta.x);

	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void AddItem(Items addItems)
    {
        item.Push(addItems); 

        if(item.Count > 0)
        {
            stackTxt.text = item.Count.ToString();
        }

        ChangeSprite(addItems.spriteNeutral, addItems.spriteHighlight);
    }

    public void AddItems (Stack<Items> items)
    {
        items = new Stack<Items>(items);

        stackTxt.text = item.Count > 1 ? item.Count.ToString() : string.Empty;

        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlight);

    }

    void ChangeSprite(Sprite neutral, Sprite Highlight)
    {
        GetComponent<Image>().sprite= neutral;

        SpriteState st = new SpriteState();
        st.highlightedSprite = Highlight;
        st.pressedSprite = neutral;

        GetComponent<Button>().spriteState = st;
    }

    void UseItem()
    {
        if(!IsEmpty)
        {
            item.Pop().Use();

            stackTxt.text = item.Count > 1 ? item.Count.ToString() : string.Empty;

            if(IsEmpty)
            {
                ChangeSprite(slotEmpty,slotHighlighted);

                Inventory.EmptySlots++;
            }
        }
    }

    public void ClearSlot()
    {
        item.Clear();
        ChangeSprite(slotEmpty,slotHighlighted);
        stackTxt.text = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }
}
