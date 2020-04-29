using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public enum LevelState
    {
        start,
        level1,
        level2,
        level3,
        lose,
        win
    }

    public UnityEvent onMiniGameOver = new UnityEvent();  //broadcast to MiniGState

    LevelState curLevel; // FSM - 1 unit of memory

    [SerializeField] int maxLevelScore = 30; //when to change levels, set in inspector

    //UI game Objects - LevelValue, StartGameButton, StartGamePanel
    [SerializeField]
    Button startGameButton;

    [SerializeField]
    Text startPanelText; // text on startGameButton

    [SerializeField]
    CanvasGroup startPanelCg; //if using an ResultsPanel

    [SerializeField]
    Text levelText;

    [SerializeField]
    Text timerText;

    [SerializeField]
    Transform playerSpawnPoint;

    GameObject player;
    PlayerController playerController;

    [SerializeField]
    GameObject[] gameObjectLayers;

    void Start()
    {
        //SETUP Event Listeners
        GameData.instanceRef.ResetGameData();

        playerController = FindObjectOfType<PlayerController>(); //Assumes only 1 exists
        player = playerController.gameObject;
        playerController.onPlayerDied.AddListener(ReloadMiniGame);
        playerController.onPlayerReachExit.AddListener(NextLevel);

        GameData.instanceRef.onPlayerDataUpdate.AddListener(CheckLevelEnd);

        startGameButton.onClick.AddListener(NextLevel);

        gameObjectLayers[0].SetActive(true); //Level1 gameObjects, spawner, background, floor
        gameObjectLayers[1].SetActive(false); //Level1 gameObjects, spawner, background, floor
        gameObjectLayers[2].SetActive(false); //Level1 gameObjects, spawner, background, floor
        gameObjectLayers[3].SetActive(false); //Level1 gameObjects, spawner, background, floor
        curLevel = LevelState.start;

        //show Start Panel at start of Scene.
        if (startPanelCg != null)
        {
            Utility.ShowCG(startPanelCg);
        }

    }

    /// <summary>
    /// Checks the level end.
    /// Checks the data to see if the level has ended
    /// </summary>
    public void CheckLevelEnd()
    {
        int levelScore = GameData.instanceRef.LevelScore;

        //Debug.Log("Check if level is over " + levelScore);

        //Add code here to see if health is <=0
        if (GameData.instanceRef.Health <= 0)
        {
            curLevel = LevelState.lose;
            MiniGameOver();
        }
        else if (levelScore >= maxLevelScore)
        { ///level has changed
          ///reset levelScore
            GameData.instanceRef.LevelScore = 0; //reset GameData.LevelScore
            NextLevel(); //go to next level - call FSM
        }

    }//end CheckLevelEnd

    //This method implements the Finite State Machine to Manage Level Logic.
    //You will modify this code to correspond to your game's logic
    //This method is always called when an event has occured to end the level
    //Event types: Score > LevelScore, health <= 0, Player falls, Player reaches exit
    public void NextLevel()
    {
        switch (curLevel) //check the curLevel, find matching case below
        {

            case LevelState.start: // called when StartPanel, StartGameButton is clicked
                curLevel = LevelState.level1; //change level
                StartLevel1();
                break;

            case LevelState.level1: //called when in Level1 from checkLevelEnd( )
                curLevel = LevelState.level2; //change level
                LoadLevel2();
                StartLevel2();
                break;

            case LevelState.level2: //called when in Level2 from checkLevelEnd( )
                curLevel = LevelState.level3; //change level
                LoadLevel3();
                StartLevel3();
                break;

            case LevelState.level3: //called when in Level3 from checkLevelEnd( )
                curLevel = LevelState.win;
                MiniGameOver();
                Debug.Log("Winstate met");
                break;

            default:
                Debug.Log("No match on curLevel");
                break;
        } //end switch-case

    }//end NextLevel

    ////YOU WILL MODIFY THESE METHODS SO THEY CORRESPOND TO YOUR GAME"S LOGIC

    //NEW LOGIC ADDED For When reloading during play-testing
    void LoadLevel1()
    {
        player.SetActive(false);
        StopSpawner(LevelState.level3); //- needs changed for l3
        StopAllCoroutines(); //stop timer

        startGameButton.onClick.AddListener(StartLevel1); //- needs changed for l3
        startPanelText.text = "Start Level 1"; //-- needs changed for l3

        Utility.ShowCG(startPanelCg);  //show panel }
    }

    //expanded logic so this works better when reloading during play-testing
    void StartLevel1()
    {
        //STARTS Gameplay, Spawner, etc
        //Player in correct position already
        gameObjectLayers[2].SetActive(false); //level3 - needs changed for l3
        gameObjectLayers[0].SetActive(true); //level1 - needs changed for l3

        ResetPlayerPosition(); //move player to spawn point
        player.SetActive(true);
        StartSpawner(LevelState.level1);// -- needs changed for l3

        levelText.text = "Level 1"; //- needs changed for l3

        Utility.HideCG(startPanelCg); //hide panel
        //StartCoroutine(reloadTimer(20));


    }



    //stop level1, prepare for starting level2
    void LoadLevel2()
    {
        player.SetActive(false);
        StopSpawner(LevelState.level1); //- needs changed for l3

        StopAllCoroutines(); //stop timer
        startGameButton.onClick.RemoveAllListeners();

        startGameButton.onClick.AddListener(StartLevel2); //- needs changed for l3
        startPanelText.text = "Start Level 2"; //-- needs changed for l3

        Utility.ShowCG(startPanelCg);  //show panel
    }

    //When "Start Level 2" button is selected
    public void StartLevel2()
    {
        gameObjectLayers[0].SetActive(false); //level1 - needs changed for l3
        gameObjectLayers[1].SetActive(true); //level2 - needs changed for l3

        ResetPlayerPosition(); //move player to spawn point
        player.SetActive(true);
        StartSpawner(LevelState.level2);// -- needs changed for l3

        levelText.text = "Level 2"; //- needs changed for l3

        Utility.HideCG(startPanelCg); //hide panel
        //StartCoroutine(reloadTimer(20));  //change per level?
    }


    void LoadLevel3()
    {
        player.SetActive(false);
        StopSpawner(LevelState.level2); //- needs changed for l3

        StopAllCoroutines(); //stop timer
        startGameButton.onClick.RemoveAllListeners();

        startGameButton.onClick.AddListener(StartLevel3); //- needs changed for l3
        startPanelText.text = "Start Level 3"; //-- needs changed for l3

        Utility.ShowCG(startPanelCg);  //show panel
    }

    public void StartLevel3()
    {
        gameObjectLayers[0].SetActive(false); //level1 - needs changed for l3
        gameObjectLayers[1].SetActive(false); //level2 - needs changed for l3
        gameObjectLayers[2].SetActive(true); //level2 - needs changed for l3

        ResetPlayerPosition(); //move player to spawn point
        player.SetActive(true);
        StartSpawner(LevelState.level3);// -- needs changed for l3

        levelText.text = "Level 3"; //- needs changed for l3

        Utility.HideCG(startPanelCg); //hide panel
        //StartCoroutine(reloadTimer(20));  //change per level?
    }
    void LoadWinLevel()
    {
        player.SetActive(false);
        StopSpawner(LevelState.level3); //- needs changed for l3

        StopAllCoroutines(); //stop timer
        startGameButton.onClick.RemoveAllListeners();

        startGameButton.onClick.AddListener(StartWinLevel); //- needs changed for l3
        startPanelText.text = "You Won!"; //-- needs changed for l3

        Utility.ShowCG(startPanelCg);  //show panel
    }

    public void StartWinLevel()
    {
        gameObjectLayers[0].SetActive(false); //level1 - needs changed for l3
        gameObjectLayers[1].SetActive(false); //level2 - needs changed for l3
        gameObjectLayers[2].SetActive(false); //level2 - needs changed for l3
        gameObjectLayers[3].SetActive(true); //level2 - needs changed for l3

        ResetPlayerPosition(); //move player to spawn point
        player.SetActive(true);
        StartSpawner(LevelState.level3);// -- needs changed for l3

        levelText.text = "Level 3"; //- needs changed for l3

        Utility.HideCG(startPanelCg); //hide panel
        //StartCoroutine(reloadTimer(20));  //change per level?
    }


    //What happens when the MiniGame is over due to winning?
    //How does a player leave the MiniGameScene?
    //What happens when the MiniGame is over due to winning?
    //How does a player leave the MiniGameScene: Win or Lose?
    void MiniGameOver()
    {
        if (curLevel == LevelState.win)
        {
            GameData.instanceRef.miniGameWinner = true;
        }
        else //must be LevelState.Lose
        {
            GameData.instanceRef.miniGameWinner = false;
        }
        //In all cases do the following
        // if (onMiniGameEnd != null) //some listener (MiniGState) Just in case
        if (onMiniGameOver != null) //some listener (MiniGState)
        {
            onMiniGameOver.Invoke();
        }
        
        //invoke custom event to notify MiniGState where sceneChange logic an be executed.
    } //end MiniGameOver



    /// <summary>
    /// Starts the spawner.
    /// </summary>
    /// <param name="level">Level.</param>
    void StartSpawner(LevelState level)
    {
        int index = (int)level - 1; //array index is one less than level number
        Spawner spawner = gameObjectLayers[index].GetComponentInChildren<Spawner>();
        spawner.gameObject.SetActive(true);
        spawner.activeSpawning = true;
        spawner.StartSpawning(); //Make sure to remove this code from Start in the spawner script
    }

    /// <summary>
    /// Stops the spawner.
    /// </summary>
    /// <param name="level">Level.</param>
    void StopSpawner(LevelState level)
    {
        int index = (int)level - 1; //array index is one less than level number
        Spawner spawner = gameObjectLayers[index].GetComponentInChildren<Spawner>();
        spawner.gameObject.SetActive(false);
        spawner.StopAllSpawning();
    }


    //moves player to left-edge when new level loaded
    public void ResetPlayerPosition()
    {
        player.transform.localPosition = playerSpawnPoint.transform.localPosition;
    }

    /* /// <summary>
    /// Reloads the Scene when the timer runs out
    /// </summary>
    /// <returns>The timer.</returns>
    /// <param name="reloadTimeInSeconds">Reload time in seconds.</param>
    IEnumerator reloadTimer(float reloadTimeInSeconds)
    {
        float counter = 0;

        while (counter < reloadTimeInSeconds)
        {
            counter += Time.deltaTime;
            timerText.text = "Timer: " + Mathf.RoundToInt(counter).ToString() + " of " + Mathf.RoundToInt(reloadTimeInSeconds).ToString(); ;
            yield return null;
        }
        //Timer is over - if the timer runs out
        ReloadMiniGame();
    }
    */

    /// <summary>
    /// 
    /// </summary>
    public void ReloadMiniGame()
    {
        //ADD NEW METHOD TO RESET ALL GameObjects for Restart
        LoadLevel1();

        GameData.instanceRef.ResetMiniGameData();

        Scene curScene = SceneManager.GetActiveScene();

        if (StateManager.instanceRef != null)
        {
            StateManager.instanceRef.SwitchState(GameScene.MiniGameScene);  //Going to 
        }
        else
        {
            Debug.Log("Play Testing MiniGame - no StateManager so can't switch scene");
            //Use Unity to Reload the scene
            SceneManager.LoadScene(curScene.buildIndex);
            //or Create method to LoadLevel1, reset all things or use new LoadLevel1() method
            //LoadLevel1();
        }
    } //end ReloadMiniGame

}// end class