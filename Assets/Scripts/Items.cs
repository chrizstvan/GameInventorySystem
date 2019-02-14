using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { SWORD, SPEAR, AXE };

public class Items : MonoBehaviour 
{
    public ItemType type;
    public Sprite spriteNeutral;
    public Sprite spriteHighlight;
    public int maxSize;

	public void Use()
    {
        switch(type)
        {
            case ItemType.SWORD:
                Debug.Log("You using sword");
                break;

            case ItemType.SPEAR:
                Debug.Log("You using spear");
                break;

            case ItemType.AXE:
                Debug.Log("You using axe"); 
                break;

            
        }
    }
}
