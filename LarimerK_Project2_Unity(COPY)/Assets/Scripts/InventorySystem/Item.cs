using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Modified from
//https://github.com/Toqozz/blog/blob/master/inventory

//https://answers.unity.com/questions/1415831/inheritance-from-a-scriptableobject.html

public enum ItemType
{
    Gem, Potion, Key
}

[System.Serializable]
//abstract classes can not be used to make 
//actual object instances, only child classes of 
//abstract classes can actually be created.
public abstract class Item : ScriptableObject {

    public string itemName;
    public int value;
    public Sprite sprite;

    protected ItemType itemType;

    public string instanceType; // enum type set in child classes - ie: Gem.Sapphire

    /// <summary>
    /// virtual means that this class can be overridden in a child-class, but it is not required
    /// in which case no code would be executed since this default version of the method 
    /// happens to have no code.
    /// </summary>
    public virtual void Use() 
    {
       //no code in this case, so nothing will be executed
    }

    ///other option which requires Use to be overridden in child classes
    /// 
    /*public abstract void Use()
    {
        //no code in this case, so nothing will be executed
    }
    */
}


[System.Serializable]
public class ItemInstance 
{
    // Reference to scriptable object "template".
    public Item item; //should be a child class item


    public ItemInstance(Item item,int value ) 
    {
        this.item = item;
        item.value = value;
    }
}
