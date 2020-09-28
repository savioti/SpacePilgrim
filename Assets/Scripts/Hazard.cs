using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 8)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (player)
            {
                player.Reset();
            }
        }
    }
}
