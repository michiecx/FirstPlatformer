using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    private bool _isGrounded;
    private bool _example = false;
    public bool IsGrounded 
    { 
        get
        {
            return _isGrounded;

        }

        private set
        { 

        }
    }

   

    public bool Example { get { return _example; } set { _example = value; } }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _isGrounded = false;
        }
    }
}
