using UnityEngine;
using System.Collections;

///
/// CS2335 - Spring 2020
///  
/// <summary>
/// I state base.  
/// Interface for all StateX.cs classes  
/// </summary>
public interface IStateBase
{
    /// <summary>
    /// Gets the scene number - enum
    /// </summary>
    /// <value>scene</value>
    GameScene Scene
    { // Interface Property 
        get; //read-only
    }

    //all interface methods are public by default!
    ///<summary>
    /// Similar to Unity Start()  
    /// exectued once, after scene is loaded - called from StateManager
    /// Used to initialize object references - can be used to cache object references
    /// </summary>
    void InitializeObjectRefs();

} //end Interface: IStateBase
