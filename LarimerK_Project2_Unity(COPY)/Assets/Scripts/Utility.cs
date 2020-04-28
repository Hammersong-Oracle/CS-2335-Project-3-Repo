using UnityEngine;
using UnityEngine.UI; //make sure to add this
using System.Collections;

public static class Utility
{

    public static void ShowCG(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    public static void HideCG(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

} //end Utility.cs