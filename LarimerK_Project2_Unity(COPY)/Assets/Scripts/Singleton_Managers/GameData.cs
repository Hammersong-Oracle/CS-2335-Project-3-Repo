using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Singleton - one and only 1 ever in existance
//global variable to allow easy access
public class GameData : MonoBehaviour
{

    /// static means it belongs to the Class, and not to an object instance of the class
    public UnityEvent onPlayerDataUpdate = new UnityEvent();

    public static GameData instanceRef;  ///Global variable 

    public Inventory inventory;//Scriptable Object
    private Dictionary<string, string> choiceData = new Dictionary<string, string>();
    /// <summary>
    /// Properties - provide read/write access to variables
    /// </summary>



    private int levelScore;
    private int score;
    private int health;
    private int experience;
    private int lives;
    private int maxExperience = 100;
    public bool miniGameWinner = false;



    //Properties - Support Encapsulation - protect inner workings of our class
    public int Score
    {
        get { return score; }   //read only access
    }

    public int LevelScore
    {
        get { return levelScore; }
        set { levelScore = value; }
    }

    public int Lives
    {
        get { return lives; }
        set { lives = value; }
    }

    public int Experience
    {
        get { return experience; }   //read only access
    }

    public int Health
    {
        get { return health; }   //read only access
    }

    private void Awake()
    {
        health = 100; //initialize
        score = 0;
        if (instanceRef == null)
        {
            instanceRef = this; //point to itself
            DontDestroyOnLoad(this.gameObject);  //this will never be destroyed
        }
        else
        {
            Destroy(this.gameObject);
            Debug.Log("Duplicate GameData is Destroyed");
        }
    }


    /// <summary>
    /// Add the specified item.
    /// Overloaded method
    /// takes the ItemInstance item as input parameter
    /// </summary>
    /// <param name="item">Item.</param>
    /// 

    public void AddItem(ItemInstance item)
    {
        inventory.InsertItem(item);
        //inventory invokes onInventoryUpdate event
    }

    /// <summary>
    /// Add the specified value.
    /// Overloaded method
    /// takes the PickUp item's value as input parameter
    /// </summary>
    /// <param name="value">Value.</param>
    public void Add(int value)
    {
        score += value;
        levelScore += value;  //Make sure you have this line of code
        Debug.Log("Score is updated " + score);
        InvokePlayerDataUpdate();
    }

    public void BoostHealth(int value)
    {
        health += value;
        health = Mathf.Min(health, 100);
        Debug.Log("boosting health, new health " + health);
        InvokePlayerDataUpdate();
    }

    public void BoostExperience(int value)
    {
        experience += value;
        experience = Mathf.Min(experience, maxExperience);
        Debug.Log("boosting experience: " + experience);
        InvokePlayerDataUpdate();
    }

    //updated TakeDamage: 4/24/19
    public void TakeDamage(int value)
    {
        health -= value;
        if (health < 0) health = 0;  //makes sure health !< 0
        Debug.Log("Health is updated " + health);
        InvokePlayerDataUpdate();
    }

    public void InvokePlayerDataUpdate()
    {
        if (onPlayerDataUpdate != null)
        {
            onPlayerDataUpdate.Invoke();
        }
    }

    public void SaveChoice(string choiceKey, string choiceValue)
    {

        if (choiceData.ContainsKey(choiceKey))
        {
            choiceData[choiceKey] = choiceValue; //change stored value
            Debug.Log("Choice Changed" + choiceKey + " : " + choiceValue);
        }
        else
        {
            choiceData.Add(choiceKey, choiceValue); //adds key,value pair
            Debug.Log("Choice Data Created" + choiceKey + " : " + choiceValue);
        }
    }

    public string GetChoice(string choiceKey)
    {
        string choiceValue = "None";
        choiceData.TryGetValue(choiceKey, out choiceValue);
        Debug.Log("Choice Data Accessed" + choiceKey + " : " + choiceValue);
        return choiceValue;
    }


    //to restart entire game
    public void ResetGameData()
    {
        health = 100; //initialize
        score = 0;
        experience = 0;
        inventory.inventory.Clear();
        miniGameWinner = false;
    }

    //to restart miniGame
    public void ResetMiniGameData()
    {
        health = 100; //initialize
        score = 0;
        levelScore = 0;
    }

} //end class