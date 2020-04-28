using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Required for modifying any UI element
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{

    public enum MiniGameState { idle, active, win, lose }

    public MiniGameState curGameState;

    public Spawner spawner;//populate in inspector

    public Button startButton; //Button component on the StartButton gameObject
    public Text resultsText;   //Text component on the ResultsText gameObject
    public CanvasGroup resultsPanelCG; //canvas group component on the ResultsPanel
    public int winScore = 30;

    // Start is called before the first frame update
    void Start()
    {
        curGameState = MiniGameState.idle;
        Utility.ShowCG(resultsPanelCG); //make sure panel is visible
        resultsText.text = "Score " + winScore + " To Win";
        startButton.onClick.AddListener(ReStartGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (curGameState == MiniGameState.active)
        {
            if (GameData.instanceRef.Health > 0)
            {
                if (GameData.instanceRef.Score >= winScore)
                {
                    //won the game
                    curGameState = MiniGameState.win;
                    resultsText.text = "You are a winner";
                    GameOver();
                    SceneManager.LoadScene("EndScene");
                }
            }
            else  //lost due to health
            {
                curGameState = MiniGameState.lose;
                resultsText.text = "You lost, so sorry, try again";
                GameOver();
                SceneManager.LoadScene("BeginScene");
            }

        } //end if
    } //end Update

    /// <summary>
    /// Restart the game
    /// </summary>
    /// Syntax for a method to be executed by a Unity Event
    /// MUST BE PUBLIC to be executed by Unity EVENT
    public void ReStartGame()  //important syntax
    {
        GameData.instanceRef.ResetGameData();  //use singleton variable to call public method
        Utility.HideCG(resultsPanelCG); //toggle to hide panel
        curGameState = MiniGameState.active;

        //start spawner
        spawner.activeSpawning = true;
        spawner.StartSpawning();
    }

    /// <summary>
    /// Games the over.
    /// Private since only executed from within this class
    /// </summary>
    private void GameOver()
    {
        Utility.ShowCG(resultsPanelCG); //toggle to make visible
        ///TODO stop spawner, destroy all spawned objects
        spawner.activeSpawning = false;
        spawner.DestroyAllPickups();

        /// Get Text that is the child of the StartButton 
        Text btnText = startButton.GetComponentInChildren<Text>();
        btnText.text = "Play Again";
    }

}//end class