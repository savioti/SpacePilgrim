using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject wall;
    public GameObject message;
    public GameObject ground;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 8)
            StartCoroutine(LetTheGameStart());
    }
    private IEnumerator LetTheGameStart()
    {
        wall.SetActive(true);
        message.SetActive(true);
        yield return new WaitForSeconds(4f);
        ground.SetActive(false);
    }
}
