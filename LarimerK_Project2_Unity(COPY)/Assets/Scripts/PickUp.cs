using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//combines frontEnd UI and BackEnd 
public class PickUp : MonoBehaviour {

    public ItemInstance itemInstance;

    private int value;

    //read-only property
    public int Value
    {
        get { return value; }
    }

    private void Start()
    {
        this.value = itemInstance.item.value;
    }

    /// <summary>
    /// Adds the item to GameData Inventory
    /// Can be executed by button.onClick
    /// when added as a listener
    /// </summary>
    public void AddItem( ) //can be called onClick for a button
    {
        GameData.instanceRef.AddItem(this.itemInstance);
    }
}
