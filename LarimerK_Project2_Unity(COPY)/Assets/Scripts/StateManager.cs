using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

///
/// CS2335 - Spring 2020
///  
/// <summary>
/// Game Scene. Matches Unity Scenes in Build Settings
/// YOU WILL ADD CODE FOR YOUR SCENES HERE
/// Scene Enums must match your Unity Scene Name
/// </summary>
public enum GameScene
{
    BeginScene = 0,
    Scene2 = 1,
    MiniGameScene = 2,
    Scene3 = 3,
    EndScene = 4
}

/// <summary>
/// State manager.  Add comments
/// </summary>
public class StateManager : MonoBehaviour
{
    public static StateManager instanceRef;
    public IStateBase activeState;
    public GameScene curScene;

    /// <summary>
    /// Create this singleton instance.
    /// Implement the Singleton Design Pattern - initialize managerInstance
    /// </summary>
    void Awake()
    {
        if (instanceRef == null)
        {
            instanceRef = this;
            DontDestroyOnLoad(gameObject);  //the gameObject this is attached to 
        }
        else
        {   //
            DestroyImmediate(gameObject);
            Debug.Log("Destroy GameObject");
        }

    }
    // Use this for initialization
    void Start()
    {
        activeState = new BeginState(); //must customize for your game
        curScene = activeState.Scene;
        activeState.InitializeObjectRefs();  //call to Initialize BeginState object references

        //next scene change - this event will call OnLevelFinishedLoading custom function
        SceneManager.sceneLoaded += OnLevelFinishedLoading;  //add function to sceneLoaded delegate
    }


    /// <summary>
    /// Switchs the state.
    /// </summary>
    /// <param name="newState">New state.</param>
    public void SwitchState(IStateBase newState)
    {
        activeState = newState;
        curScene = newState.Scene; //set scene based on newState's GameScene enum
        SceneManager.LoadScene(curScene.ToString());
        Debug.Log("Switch Scene/State to: " + curScene );
    }

    //Public method to switch state using nextScene enum
    //YOU WILL ADD LOGIC HERE FOR EACH TRANSITION
    public void SwitchState( GameScene nextScene )
    {
        switch (nextScene)
        {
            case GameScene.BeginScene:
                SwitchState(new BeginState());
                break;

            case GameScene.Scene2:
                SwitchState(new Scene2State());
                break;

            case GameScene.MiniGameScene:
                SwitchState(new MiniGameState());
                break;

            case GameScene.Scene3:
                SwitchState(new Scene3State());
                break;

            case GameScene.EndScene:
                SwitchState(new EndState());
                break;

            default:
                Debug.Log("No match on SwitchState - scene " + nextScene);
                    break;
        }
    }


    //StateManager needs to know when a new scene has been loaded
    //then it can call: InitializeObjectRefs for the current state
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        int sceneID = scene.buildIndex;
        if (sceneID == (int)curScene)
        {
            Debug.Log("New function in StateManager to initialize new scene objects - exectued");
            activeState.InitializeObjectRefs();
        }
        else
        {
            Debug.Log("Big Problems - Scene & State Mismatch");
            Debug.Log("LevelFinished Loading :Scene Loaded: " + sceneID + " ActiveState Scene Enum: " + activeState.Scene);
        }


    }
}
