using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Non-Persistent Singleton Class across Scenes
public abstract class Global: MonoBehaviour
{
    private static Global _instance;
    public static Global Instance 
    {   
        get 
        {
            return _instance; 
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void OnDestroy() {
        _instance = null;
    }    
}
