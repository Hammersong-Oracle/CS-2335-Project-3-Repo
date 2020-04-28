using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class ConversationEntry
{
    public string speakerName;

    [TextArea]
    public string dialogTxt;

    public Sprite speakerImg;
}