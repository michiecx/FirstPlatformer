using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private CharacterBase characterBase;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb2d;
    private PlayerController playerController;
    private Vector2 currentDirection;
    private bool isStairs = false;
    private int currentCountStairs = 0;
    private int maxHealth = 10;
    private int currentHealth;

    [SerializeField] private Transform respawnPoint;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerController = new PlayerController();
        playerController.Player.Move.performed += _ => Move(_);
        playerController.Player.Move.canceled += _ => EndMove(_);
        playerController.Player.Jump.performed += _ => Jump(_);
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (characterBase.Example == false)
        {
            characterBase.Example = true;
        }
    }

    private void Update()
    {
        UpdateAnimations();
        //Debug.Log(!characterBase.IsGrounded && currentDirection.y <= 0f);
    }

    private void FixedUpdate()
    {
        if (!isStairs)
        {
            rb2d.gravityScale = 1;
            rb2d.velocity = new Vector2(currentDirection.x * speed, rb2d.velocity.y);
        }
        else
        {
            rb2d.gravityScale = 0;
            rb2d.velocity = new Vector2(currentDirection.x * speed, currentDirection.y * speed);
        }
    }

    private void OnEnable()
    {
        playerController.Player.Enable();
    }

    private void OnDisable()
    {
        playerController.Player.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        currentDirection = context.ReadValue<Vector2>();

        if (currentDirection.x > 0)
        {
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
    }

    private void EndMove(InputAction.CallbackContext context)
    {
        currentDirection = Vector2.zero;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (characterBase.IsGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }
    }

    private void UpdateAnimations()
    {
        animator.SetBool("Fall", !characterBase.IsGrounded && currentDirection.y <= 0 && !isStairs);
        animator.SetBool("Run", characterBase.IsGrounded && currentDirection.x != 0);
        animator.SetBool("Jump", !characterBase.IsGrounded && rb2d.velocity.y > 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
            currentCountStairs++;
            isStairs = true;
            animator.Play("Character1Run");

        }
        else if (collision.gameObject.tag == "WinPlace")
        {
            Debug.Log("You win!");
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
            currentCountStairs--;

            if (currentCountStairs == 0)
            {
                isStairs = false;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth - amount > 0)
        {
            currentHealth -= amount;
        }
        else
        {
            Die();
        }

    }

    private void Die()
    {
        Debug.Log("Player is dead");
        playerController.Player.Disable();
        rb2d.velocity = Vector2.zero;
        rb2d.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        Invoke("Respawn", 5f);
    }


    private void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = respawnPoint.position;
        rb2d.isKinematic = false;
        GetComponent<Collider2D>().enabled = true;
        playerController.Player.Enable();
        animator.Play("Idle");
    }
}
