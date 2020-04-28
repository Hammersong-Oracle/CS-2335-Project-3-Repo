using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//modified from
////https://github.com/Toqozz/blog/blob/master/inventory

public class InventoryDisplay : MonoBehaviour
{
    private List<Slot> inventorySlots;
    private Inventory inventory;
   
    public Dictionary<string, int> InstanceTypeCounts = new Dictionary<string, int>(); //dictionary

    public const int NumSlots = 4; //must change if more slots are displayed
    public int displayedRowPointer=0; //
    Animator visibilityAnimator; //comment out if animation is not used
    public bool isVisible = false;

        // Use this for initialization
    void Start()
    {
        inventory = GameData.instanceRef.inventory;

        //inventoryDisplay wants to be notified when inventory changes
        inventory.onInventoryUpdate.AddListener(UpdateDisplay);

        // find all child slots, put in a list
        inventorySlots = new List<Slot>();
        //add child slots to the inventorySlots list.
        inventorySlots.AddRange(Object.FindObjectsOfType<Slot>());

        // Maintain some order (just in case it gets screwed up).
        inventorySlots.Sort((a, b) => a.index - b.index);
        SetSlotIndexes();
        PopulateInitial();

        //comment out code below if not using animation
        visibilityAnimator = GetComponent<Animator>();
        visibilityAnimator.SetBool("IsVisible", false);
    }

    //Listen for Tab key for Animation - every frame
    private void Update()
    {
        if( Input.GetKeyUp( KeyCode.Tab))
        {
            isVisible = !isVisible;
            visibilityAnimator.SetBool("IsVisible", isVisible);
        }
    }

    //update itemCount dictionary: items, counts - get data from
    //inventory
    //condense data into key-value pairs:  item: count
    private void UpdateItemCounts()
    {
      
        InstanceTypeCounts.Clear(); 
         //for all items in the inventory's inventory list
         //count the number of items with the same InstanceType ( enum values )
         //put unique item in dictionary/ with count as value
        
        for( int i=0; i< inventory.inventory.Count; i++)
        {
            //for each item in data inventory list
            //if the inventory list element with index i
            //add key: InstanceType to dictionary, update count

            if (!inventory.SlotEmpty(i)) //is there an item in the inventory for each list item?
            {
                ItemInstance item = inventory.inventory[i]; 
                string instanceType = item.item.instanceType; //is string cast of enum value
                int count;
                if (InstanceTypeCounts.TryGetValue(instanceType, out count))
                {
                    InstanceTypeCounts[instanceType] = ++count; //if already added, increment count
                }
                else //did not find item in the dictionary, so add as key,value=1
                {
                    InstanceTypeCounts.Add(instanceType, 1); //add item
                }
            }
        }
    }

    private void PopulateInitial()
    {
        UpdateItemCounts(); //populate dictionary
        if( InstanceTypeCounts.Count > 0) { //something in the dictionary
        int index = 0;
            //get the keys (all existing items) 
             Dictionary<string, int>.KeyCollection keys = InstanceTypeCounts.Keys;

            //loop through all key-value: InstanceType-count pairs
            foreach (string instanceType in keys)
            {
                if (index < NumSlots) //fill the first 4 slots
                {
                    int value = InstanceTypeCounts[instanceType];//use current key-item to get the count (value)
                   
                     if (instanceType != null) //display slot already has an item of matching key
                    {
                        //Update the slot, by updating the count

                        //get one itemInstance from Inventory using InstanceType match?
                        ItemInstance oneItem = inventory.GetByItemType(instanceType);
                        if (oneItem != null)
                        {
                            inventorySlots[index].SetItem(oneItem, value);
                            index++; //move to next slot
                        }
                    }
                }
                else
                {
                  Debug.Log("more items than slots, some not displayed");
                }
            }
        }

    } //end method

    void SetSlotIndexes()
    {
        foreach( Slot slot in inventorySlots)
        {
            slot.index = inventorySlots.IndexOf(slot);
        }
    }

    public void RemoveItem(ItemInstance item, int index )
    {
        string itemTypeKey = item.item.instanceType; //get item's InstanceType to use as key
        int count=0;
        if(InstanceTypeCounts.TryGetValue( itemTypeKey, out count))
        {
            if( count <= 1) //if the last one
            {
                InstanceTypeCounts.Remove(itemTypeKey); //remove from dictionary itemCounts using key
                inventorySlots[index].RemoveItem(index);//remove from slot display
                inventory.RemoveOneItem(item); //remove from data inventory
            }
            else
            {
                InstanceTypeCounts[itemTypeKey] = count - 1;  //decrease dictionary count for this InstanceType
                inventorySlots[index].SetItem(item, count-1);  //update slot's displayed count value
                inventory.RemoveOneItem(item); //remove one from data inventory
            }
        }
    } //end RemoveItem(  )

    //Listener for onInventoryUpdate Event
    public void UpdateDisplay()
    {
        ClearSlots(); //clear slots
        PopulateInitial(); //reset dictionary count and slots
    }

    //Adds an item to the itemCount dictionary
    //or updates the count if item was already in the dictionary
    void AddToCounts(ItemInstance item)
    {
        int count;
        if (InstanceTypeCounts.TryGetValue(item.item.instanceType, out count))
        {
            InstanceTypeCounts[item.item.instanceType] = ++count;
            Debug.Log("itemCount " + item.item.itemName + " " + count);
        }
        else if (InstanceTypeCounts.Count <= NumSlots) //we still have an available slot
        {
            InstanceTypeCounts.Add(item.item.instanceType, 1); //add first item
        }
    }

    //clear all inventory slots
   private void ClearSlots()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].RemoveItem(i);
        }
    }

    //unregister listener when this object is destroyed 
    private void OnDisable()
    {
        inventory.onInventoryUpdate.RemoveListener(UpdateDisplay);
    }

} //end class InventoryDisplay
