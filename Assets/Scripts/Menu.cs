using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // public GameObject mainWindow;
    // public GameObject creditsWindow;
    public AudioSource audioSource; 
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    public void PlayMenuSound()
    {
        audioSource.Play();
    }
}
