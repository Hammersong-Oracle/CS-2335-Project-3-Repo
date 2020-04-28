using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//modified from
////https://github.com/Toqozz/blog/blob/master/inventory

public enum PotionType { Energy, Wisdom, Truth, Health }

public class Potion : Item {

    public PotionType potionType;

    public Potion()
    {
        itemType = ItemType.Potion;
        instanceType = potionType.ToString(); //set item value of potionType enum as string
    }

    public override void Use()
    {
        Debug.Log("Using Potion " + this.potionType);
        switch (this.potionType)
        {
            case PotionType.Health:
                GameData.instanceRef.BoostHealth(this.value);
                break;
            case PotionType.Energy:
               // GameData.instanceRef.BoostEnergy(this.value);
                break;
            case PotionType.Wisdom:
            case PotionType.Truth:
                //GameData.instanceRef.BoostExperience(this.value);
                break;
        }
        GameData.instanceRef.BoostHealth(this.value);
    }

} //end class Potion
