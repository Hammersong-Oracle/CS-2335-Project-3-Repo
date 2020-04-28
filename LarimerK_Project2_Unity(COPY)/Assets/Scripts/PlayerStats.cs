using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    //make connection with Text elements in the inspector
    public Text healthText, scoreText;
    // Use this for initialization

    void Start()
    {
        UpdateDisplay();
    }

    void Update()
    { //called every frame - polling to see if data changed
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        healthText.text = "Health: " + GameData.instanceRef.Health;
        scoreText.text = "Score: " + GameData.instanceRef.Score;

    }
} // end Class