using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Added missing semicolon here

public class LoadGame : MonoBehaviour
{
    public void PlayGame() // Added parentheses to indicate this is a method
    {
        SceneManager.LoadSceneAsync(1); // Asynchronously loads the scene with index 1
    }
}
