using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryHazard : MonoBehaviour
{
    public float activeTime = 2;
    public float inactiveTime = 2;
    public float delay = 0;
    private SpriteRenderer spriteRenderer;
    private Collider2D ownCollider;
    private Animator animator;
    private void Awake() 
    {
        ownCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Start() 
    {
        ownCollider.enabled = false;
        Invoke(nameof(StartLoop), delay);
    }
    private void StartLoop()
    {
        animator.Play("activate");
    }
    private IEnumerator LoopActive()
    {
        yield return new WaitForSeconds(activeTime);
        animator.Play("deactivate");
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(inactiveTime + delay);
        animator.Play("activate");
    }
    public void Activate()
    {
        ownCollider.enabled = true;
        StartCoroutine(LoopActive());
    }
    public void Deactivate()
    {
        ownCollider.enabled = false;
        StartCoroutine(Cooldown());
    }
    public void HideSprite()
    {
        spriteRenderer.enabled = false;
    }
    public void ShowSprite()
    {
        spriteRenderer.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
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
