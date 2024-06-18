using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heroComp : MonoBehaviour
{
    public GameObject projectile;
    public GameObject shootingPoint;
    public ParticleSystem shootingEffect;
    public AudioSource shootingSound;
    public float movementSpeed = 5f;
    public int shootingPower = 25;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool canMoveRight = true;
    private bool isGrounded;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleShooting();
        HandleMovement();
    }

    void FixedUpdate() // Fizik hesaplamaları için FixedUpdate kullanın
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        HandleJumping();
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // Yerdeyken ateş etme yeteneği
        {
            shootingEffect.Play();
            shootingSound.Play();
            GameObject projectileInstance = Instantiate(projectile, shootingPoint.transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectileInstance.GetComponent<Rigidbody2D>();
            Vector2 force = transform.up * shootingPower;
            projectileRb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * movementSpeed;
        if (moveHorizontal > 0 && !canMoveRight)
        {
            moveHorizontal = 0;
        }
        rb.velocity = new Vector2(moveHorizontal, rb.velocity.y);
        animator.SetBool("run", Mathf.Abs(moveHorizontal) > 0);
    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("jump", true);
        }
        else if (!isGrounded)
        {
            animator.SetBool("jump", false);
        }
    }
}
