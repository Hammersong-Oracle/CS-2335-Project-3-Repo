using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//modified from
////https://github.com/Toqozz/blog/blob/master/inventory

[System.Serializable]
public class Inventory : ScriptableObject
{
    public UnityEvent onInventoryUpdate = new UnityEvent();

    //List is how inventory items are saved in memory
    public List<ItemInstance> inventory;  

    public void OnEnable()
    {
        if (inventory == null)
        {
            inventory = new List<ItemInstance>(); //initialize
        }
        else
        {
            inventory.Clear(); //clear all items when game starts in first Scene
        }
    }

    public bool SlotEmpty(int index) //check if list element is null
    {
        if (inventory[index] == null || inventory[index].item == null)
        {
            Debug.Log(" empty slot");
            return true;
        }
        return false;
    }

    // Remove an item at an index if one exists at that index.
    //search list to find an item of this type, and remove the first one found
    //searching for an item with matching instanceType to remove
    public bool RemoveOneItem(ItemInstance item)
    {
        string itemType = item.item.instanceType;
        bool  foundOne = false;
        for( int i=0; i< inventory.Count; i++)
        {
            ItemInstance oneItem = inventory[i];
            if (oneItem.item.instanceType == itemType)
            {
                inventory.RemoveAt(i);
                foundOne = true;
                return foundOne;
            }
        }
        return foundOne;  //returns true/false
    }

    // Insert an item, return the index where it was inserted.  -1 if error.
    public void InsertItem(ItemInstance item)
    {

        Debug.Log("item added to inventory " + item.item.name);
        inventory.Add(item); //add to list 

        ///Broadcast event to notify listeners - InventoryDisplay
        if (onInventoryUpdate != null)
        {
            onInventoryUpdate.Invoke();
        }

    } //end method

    //gets and returns an ItemInstance that matches this InstanceType
    public ItemInstance GetByItemType( string itemType)
    {
        foreach( ItemInstance item in inventory)
        {
            if( item.item.instanceType == itemType)
            {
                return item;
            }
        }
        return null;
    }

} //end class