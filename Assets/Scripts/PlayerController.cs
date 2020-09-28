using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Values")]
    public float movingSpeed = 3;
    public float airMovingSpeed = 1;
    public float jumpingStrength = 1;
    public float downwardBoostStr = 100;
    public float landingCooldownTime = 1f;
    public float invencibilityTime = 2f;
    [Header("References")]
    public AudioSource[] audioSources;
    public AudioClip jumpSnd;
    public AudioClip landSnd;
    public AudioClip dropLandSnd;
    public AudioClip dashSnd;
    public AudioClip wallHitSnd;
    public AudioClip damageSnd;
    public AudioClip stepSnd;
    [Header("Debug")]
    public float MIN_VERT_VELOCITY = 0.001f;
    public float horizontalInput;
    public float verticalInput;
    public bool isFacingRight = true;
    public bool isGrounded;
    public bool isDropping;
    public bool canMove = true;
    public bool isJumping = false;
    public float rayLength = 0.1f;
    private int groundMask;
    private Vector2 checkPoint;
    private Rigidbody2D rb2d;
    private Transform ownTransform;
    private Collider2D ownCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public static System.Action OnDamage;
    private void Awake() 
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        ownCollider = GetComponent<Collider2D>();
        ownTransform = transform;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        checkPoint = ownTransform.position;
    }
    private void Update() 
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Drop();
        Jump();
        CheckDirection();
    }
    private void FixedUpdate() 
    {   
        Move(horizontalInput, movingSpeed);
        Fly(verticalInput);
        isGrounded = CheckGround();
    }
    private void Move(float direction, float speed)
    {
        if (!canMove || isJumping || !isGrounded) return;

        if (horizontalInput == 0)
        {
            rb2d.velocity = Vector2.zero;
            animator.Play("idle");
        }
        else
        {
            if (Mathf.Abs(rb2d.velocity.x) > speed)
            {
                rb2d.velocity = new Vector2(horizontalInput * speed, rb2d.velocity.y);
            }
            else
            {
                rb2d.AddForce(new Vector2(horizontalInput * 100f, 0));
            }

            animator.Play("walk");
        }
    }
    private void Jump()
    {
        if (!isGrounded || isJumping || isDropping) return;

        if (Input.GetButtonDown("Jump") || verticalInput > 0)
        {
            isJumping = true;
            rb2d.AddForce(Vector2.up * jumpingStrength, ForceMode2D.Impulse);
            animator.Play("jump");
            PlaySoundEffect(jumpSnd);
        }
    }
    private void Fly(float direction)
    {
        if (isGrounded || !canMove) return;

        rb2d.velocity = new Vector2(rb2d.velocity.x + (horizontalInput * airMovingSpeed), rb2d.velocity.y);
    }
    private void Drop()
    {
        if (verticalInput >= 0 || isDropping || isGrounded) return;
        canMove = false;
        isDropping = true;
        spriteRenderer.color = Color.red;
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(Vector2.down * downwardBoostStr, ForceMode2D.Impulse);
        PlaySoundEffect(dashSnd);
    }
    private void Land()
    {
        isJumping = false;
        animator.Play("land");

        if (isDropping)
        {
            PlaySoundEffect(dropLandSnd);
            StartCoroutine(LandingCooldown());
        }
        else
        {
            PlaySoundEffect(landSnd);
        }
    }
    private void PlaySoundEffect(AudioClip clip)
    {
        if (!audioSources[0].isPlaying)
            audioSources[0].PlayOneShot(clip);
        else
            audioSources[1].PlayOneShot(clip);
    }
    public void PlayStepSound()
    {
        PlaySoundEffect(stepSnd);
    }
    private void CheckDirection()
    {
        if (!canMove) return;

        if (isFacingRight && horizontalInput < 0)
            ChangeDirection();

        else if (!isFacingRight && horizontalInput > 0)
            ChangeDirection();
    }
    private void ChangeDirection()
    {
        isFacingRight = !isFacingRight;
        Vector3 invertedScale = ownTransform.localScale;
        invertedScale.x *= -1;
        ownTransform.localScale = invertedScale;
    }
    private bool CheckGround()
    {
        if (rb2d.velocity.y > MIN_VERT_VELOCITY) return false;

        Bounds bounds = ownCollider.bounds;
        Vector2 rayOrigin = new Vector2(bounds.min.x, bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, groundMask);
        Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(new Vector2(bounds.max.x, bounds.min.y), Vector2.down * rayLength, Color.red);

        if (hit.collider)
        {
            if (!isGrounded)
            {
                Land();
            }

            return true;
        }
        else
        {
            rayOrigin = new Vector2(bounds.max.x, bounds.min.y);
            hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, groundMask);

            if (hit.collider)
            {
                if (!isGrounded)
                {
                    Land();
                }

                return true;
            }
        }

        animator.Play("fall");
        return false;
    }
    private IEnumerator LandingCooldown()
    {
        float interval = landingCooldownTime / 20;

        for (int i = 0; i < 20; i++)
        {
            Color color = spriteRenderer.color;
            color.g += 0.05f;
            color.b += 0.05f;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(interval);
        }

        spriteRenderer.color = Color.white;
        isDropping = false;
        canMove = true;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (!isGrounded && other.gameObject.layer == 9)
        {
            PlaySoundEffect(wallHitSnd);
        }
        if (isDropping)
        {
            rb2d.velocity = Vector2.zero;
        }
    }
    public void ChangeCheckpoint(Vector2 point)
    {
        checkPoint = point;
    }
    public void Reset()
    {
        if (OnDamage != null) OnDamage();
        PlaySoundEffect(damageSnd);
        StartCoroutine(SpriteBlink());
        isDropping = false;
        ownTransform.position = checkPoint;
        rb2d.velocity = Vector2.zero;
        spriteRenderer.color = Color.white;
        canMove = true;
    }
    private IEnumerator SpriteBlink()
    {
        float interval = invencibilityTime / 20;

        for (int i = 0; i < 20; i++)
        {
            Color color = spriteRenderer.color;
            color.a = i % 2 == 0 ? 0 : 1;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(interval);
        }

        spriteRenderer.color = Color.white;
    }
}
