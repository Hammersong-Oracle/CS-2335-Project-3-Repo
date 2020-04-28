using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; //add to all State Files

public class EndState : IStateBase
{

    private GameScene scene;

    //add commenets
    public GameScene Scene
    {
        get { return scene; }
    }

    //GameScene objectRefs
    private Button optionBtn1, optionBtn2;

    //constructor  // add comments
    public EndState()
    {
        scene = GameScene.EndScene;
    }


    //Like Start()  - executed once when scene is first loaded
    public void InitializeObjectRefs()
    {
        optionBtn1 = GameObject.Find("ButtonOption1").GetComponent<Button>();
        optionBtn1.onClick.AddListener(LoadBeginScene);
        Debug.Log("InitializeObj Refs - EndState");
    }


    public void LoadBeginScene()
    {
        Debug.Log("Leaving EndScene, going to BeginScene");
        StateManager.instanceRef.SwitchState(GameScene.BeginScene);

    }
} //end class:  EndState