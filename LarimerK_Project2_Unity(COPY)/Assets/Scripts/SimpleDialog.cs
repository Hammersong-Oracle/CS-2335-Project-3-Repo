using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDialog : MonoBehaviour
{

    public Button openButton;
    public CanvasGroup nextPanelToOpen;
    public bool showOnStart = false;

    //find in children
    private Button nextButton; //only 1 child button
    private Text dialogText;   //find as a child - Hierarchy order mattersprivate CanvasGroup dialogCG;  //Canvas Group on top level - script attached to this Panel
    private Text speakerText;  //find as a child - Hierarchy order matters
    private CanvasGroup dialogCG; //top level panel

    private Queue<ConversationEntry> conversationsQueue = new Queue<ConversationEntry>();
    public List<ConversationEntry> conversations;

    // Use this for initialization
    void Start()
    {
        dialogCG = GetComponent<CanvasGroup>();
        Text[] textChildren = GetComponentsInChildren<Text>();
        dialogText = textChildren[0];
        speakerText = textChildren[2];

        InitializeDialog();

        nextButton = GetComponentInChildren<Button>();
        nextButton.onClick.AddListener(GetNextDialog);
        if (!showOnStart)
        {   //click button to display panel
            if (openButton != null)
            {
                openButton.onClick.AddListener(ShowDialogPanel);
            }
            Utility.HideCG(dialogCG); //hide initially
        }
        else //show on start
        {
            Utility.ShowCG(dialogCG);
        }

    }//end start

    void InitializeDialog()
    {
        foreach (ConversationEntry item in conversations)
        {
            conversationsQueue.Enqueue(item); //put each string -item in the queue
        }
        GetNextDialog();  //get first item
    }

    void GetNextDialog()
    {
        if (conversationsQueue.Count > 0)
        {
            ConversationEntry item = conversationsQueue.Dequeue();
            dialogText.text = item.dialogTxt;
            speakerText.text = item.speakerName;
        }
        else
        { //no more dialog
            if (nextPanelToOpen != null)
            {
                Utility.ShowCG(nextPanelToOpen);
            }
            Utility.HideCG(dialogCG); //hide the dialog
        }
    }

    public void ShowDialogPanel()
    {
        Utility.ShowCG(dialogCG);
    }//end function

} //end class