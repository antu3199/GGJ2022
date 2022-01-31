using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.SceneManagement;  

// Space background taken from: https://opengameart.org/content/space-background-7
public class MainMenu: MonoBehaviour {  
    public void PlayGame() {  
        SceneManager.LoadScene("Game_Anthony");  
    }  
}  