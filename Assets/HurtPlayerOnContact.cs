using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerOnContact : MonoBehaviour
{
    public int damage;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                player.health.TakeDamage(damage);
                player.knockbackCount = player.knockbackLength;

                if(collision.transform.position.x < transform.position.x)
                {
                    player.knockFromRight = true;
                }
                else
                {
                    player.knockFromRight = false;
                }
            }
        }
    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (player.invulnerable == false)
            {
                player.TakeDamage(damage);
            }
            if (collision.transform.position.x < transform.position.x)
            {
                StartCoroutine(player.Knockback(.02f, 350, player.transform.position, false));
            }
            else
            {
                StartCoroutine(player.Knockback(.02f, 350, player.transform.position, true));
            }
            
        }
    }
}
