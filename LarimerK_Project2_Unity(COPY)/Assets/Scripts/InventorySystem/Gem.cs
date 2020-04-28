using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//modified from
////https://github.com/Toqozz/blog/blob/master/inventory


public enum GemType { Ruby, Diamond, Sapphire, Emerald }

[System.Serializable]
public class Gem : Item {

    public GemType gemType;

    public Gem()
    {
        itemType = ItemType.Gem;
        instanceType = gemType.ToString();
    }

    public override void Use()
    {
        Debug.Log("Using Gem " + this.gemType);
        //TODO what does a gem do?
    }
} //end class Gem
