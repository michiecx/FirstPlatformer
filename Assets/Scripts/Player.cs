using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private PlayerController playerController;
    private float speed = 5f;
    private Vector2 currentContextValue;
    private bool onStairs = false;
    private float jumpForse = 2f;
    private bool isGrounded = true;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerController = new PlayerController();
        playerController.Player.Jump.performed += _ => Jump(_);
        playerController.Player.Move.performed += _ => Move(_);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (playerController.Player.Move.phase != InputActionPhase.Waiting)
        {
            if (onStairs == false)
            {
                currentContextValue.y = 0;
            }
            rb2d.MovePosition(rb2d.position + currentContextValue * speed * Time.fixedDeltaTime);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Stairs")
        {
            onStairs = true;
            //rb2d.gravityScale = 0; отключение гравитации
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
            onStairs = true;
        }
       

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Stairs")
        {
            onStairs = false;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
            onStairs = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            isGrounded = true;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Waiting && isGrounded == true )
        {
            rb2d.AddForce(Vector2.up * jumpForse, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void Move(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        currentContextValue = context.ReadValue<Vector2>();
    }
}
