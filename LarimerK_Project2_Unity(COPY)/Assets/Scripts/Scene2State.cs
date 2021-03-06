﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //ADD THIS using directive

public class Scene2State : IStateBase
{
    /// <summary>
    /// The scene enum associated with this state.
    /// </summary>
    private GameScene scene;

    /// <summary>
    /// Read Only Property gives access to the scene number - enum
    /// </summary>
    /// <value>The scene.</value>
    public GameScene Scene
    {
        get { return scene; }
    }

    private Button optionBtn1, scene2Button, miniGameButton, scene3Button, beginSceneButton;

    public Scene2State()
    {
        scene = GameScene.Scene2; //IMPORTANT:  make sure this matches Unity Scene Name
    }

    public void InitializeObjectRefs()
    {

        miniGameButton = GameObject.Find("MiniGameButton").GetComponent<Button>();
        miniGameButton.onClick.AddListener(LoadMiniGame); //call custom method defined below

        scene3Button = GameObject.Find("Scene3Button").GetComponent<Button>();
        scene3Button.onClick.AddListener(LoadScene3); //call custom method defined below

        beginSceneButton = GameObject.Find("BeginSceneButton").GetComponent<Button>();
        beginSceneButton.onClick.AddListener(LoadBeginScene); //call custom method defined below
    }

    public void LoadMiniGame()
    {
        Debug.Log("Leaving BeginScene going to MiniGame");
        StateManager.instanceRef.SwitchState(GameScene.MiniGameScene);
    }

    public void LoadScene3()
    {
        Debug.Log("Leaving BeginScene going to Scene3");
        StateManager.instanceRef.SwitchState(GameScene.Scene3);
    }

    public void LoadBeginScene()
    {
        Debug.Log("Leaving BeginScene going to Scene3");
        StateManager.instanceRef.SwitchState(GameScene.BeginScene);
    }

}
