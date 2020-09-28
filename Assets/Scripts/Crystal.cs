using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public static System.Action OnCoinPick;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 8)
        {
            OnCoinPick();
            gameObject.SetActive(false);
        }
    }
}
