using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; //add to all State Files

public class BeginState : IStateBase
{

    /// <summary>
    /// The scene.
    /// </summary>
    private GameScene scene;

    /// <summary>
    /// Gets the scene number - enum
    /// </summary>
    /// <value>The scene.</value>
    public GameScene Scene
    {
        get { return scene; }
    }

    //GameScene objectRefs
    private Button optionBtn1, scene2Button, miniGameButton, scene3Button;

    /// <summary>
    /// Initializes a new instance of the <see cref="BeginState"/> class.
    /// </summary>
    public BeginState()
    {
        scene = GameScene.BeginScene; //IMPORTANT:  make sure this matches Unity Scene Name
    }

    /// <summary>
    /// Similar to Unity Start() 
    /// exectued once, after scene is loaded - called from StateManager
    /// Used to initialize object references - can be used to cache object references
    /// </summary>
    public void InitializeObjectRefs()
    {
        optionBtn1 = GameObject.Find("ButtonOption1").GetComponent<Button>();
        optionBtn1.onClick.AddListener(LoadEndScene);

        scene2Button = GameObject.Find("Scene2Button").GetComponent<Button>();
        scene2Button.onClick.AddListener(LoadScene2); //call custom method defined below

        miniGameButton = GameObject.Find("MiniGameButton").GetComponent<Button>();
        miniGameButton.onClick.AddListener(LoadMiniGame); //call custom method defined below

        scene3Button = GameObject.Find("Scene3Button").GetComponent<Button>();
        scene3Button.onClick.AddListener(LoadScene3); //call custom method defined below

        Debug.Log("Initialize Refs - BeginState");
    }

    /// <summary>
    /// Event handler - called when endBtn is clicked
    /// Loads the end scene.
    /// public method can be executed by button onClick event
    /// </summary>
    public void LoadEndScene()
    {
        Debug.Log("Leaving BeginScene going to EndScene");
        StateManager.instanceRef.SwitchState(GameScene.EndScene);  
    }

    public void LoadScene2()
    {
        Debug.Log("Leaving BeginScene going to Scene2");
        StateManager.instanceRef.SwitchState(GameScene.Scene2);
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
} //end class BeginState

