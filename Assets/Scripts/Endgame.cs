using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endgame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 8)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
        }
    }
}
