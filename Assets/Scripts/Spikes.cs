using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private int damageFromSpikes = 1;
    private float damageInterval = 1f;
    private float damageTimer = 0f;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damageFromSpikes);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(damageFromSpikes);
                damageTimer = 0f;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            damageTimer = 0f;
        }
    }
}
