using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 8)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player)
            {
                player.ChangeCheckpoint(transform.position);
                gameObject.SetActive(false);
            }
        }
    }
}
