using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hide_Show_Panel : MonoBehaviour
{

    CanvasGroup panelCG; //must be on same gameObject as this script

    public bool showOnStart = false; //default is hidden
    public Button closeButton; //clicking will hide
    public Button openButton; //clicking will show
    public CanvasGroup nextPanelToOpen;

    // Use this for initialization
    void Start()
    {
        panelCG = GetComponent<CanvasGroup>(); //must have CanvasGroup component

        if (!showOnStart)
        {
            HidePanel(); //hide at start
        }
        else
        {
            ShowPanel(); //show at start
        }
        //if buttons are set in inspector, configure to open/close panel 
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePanel);
        }
        if (openButton != null)
        {
            closeButton.onClick.AddListener(ShowPanel);
        }

    }

    public void ShowPanel()
    {
        Utility.ShowCG(panelCG);
    }

    public void HidePanel()
    {
        Utility.HideCG(panelCG);
    }

    public void OpenNextPanel()
    {
        if (nextPanelToOpen != null)
        {
            Utility.ShowCG(nextPanelToOpen);
        }
    }
}