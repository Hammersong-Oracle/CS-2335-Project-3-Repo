using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//modified from
////https://github.com/Toqozz/blog/blob/master/inventory

public class Slot : MonoBehaviour, IPointerClickHandler
{
    InventoryDisplay inventoryDisplay; 
    public int index = 0; //is auto-set within PhysicalInventory
    public ItemInstance itemInstance = null;    // Inventory backend representation.
    public string instanceType = "";
   // public GameObject prefabInstance = null;    // Inventory frontend representation.
    public Image childImage;
    public Text childText;
 
    void Start()
    {
        inventoryDisplay = FindObjectOfType<InventoryDisplay>();

    }


    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    { //check to see if an item is in the slot!
        if (itemInstance != null && itemInstance.item != null)
        {
            itemInstance.item.Use();
            inventoryDisplay.RemoveItem(itemInstance, index);
            //TODO USE and REMOVE ITEM
            //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
            Debug.Log(name + " Game Object Clicked!");
        }
    }

    // TODO: it would be better if we used SetActive() etc rather than Instantiate/Destroy.
    // Use this method to set a slot's item.
    // The slot will automatically instantiate the gameobject associated with the item.
    public void SetItem(ItemInstance instance, int count)
    {
        if (instance != null)
        {
            this.itemInstance = instance;
            this.instanceType = instance.item.instanceType;
            this.childImage.sprite = instance.item.sprite;
            this.childImage.preserveAspect = true;
            this.childImage.color = Color.white;
            this.childText.color = Color.white;
            this.childText.text = count.ToString();
        }
    }

    // Remove the item from the slot, and destroy the associated gameobject.
    public void RemoveItem(int index)
    {
        if (this.index == index) //check it is the slot's current index
        {
            this.itemInstance = null;
            this.instanceType = "";
            this.childImage.sprite = null;
            this.childImage.color = Color.clear;
            this.childText.color = Color.clear;
        }
    }
} //end class Slot
