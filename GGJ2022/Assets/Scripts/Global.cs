using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to specify a singleton object
// This is usually used for "Manager" type models that persist between levels.
public class Global<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T instance;

  public static T Instance
  {
    get
    {

      if (instance == null)
      {
        // If no static instance, then find one
        instance = (T)FindObjectOfType(typeof(T));
      }

      if (instance == null) {
        Debug.LogWarning("Could not find instance of" + typeof(T));
      }

      return instance;
    }
  }

  void OnDestroy() {
        instance = null;
    } 
}
