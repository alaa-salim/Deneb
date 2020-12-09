using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{

    public float speed;

    public bool movingRight = true;

    public float rayLength = .5f;

    public Transform groundDetection;

    public int health = 100;

    public GameObject deathEffect;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector2.right * speed *Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, rayLength);

        if(groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                movingRight = false;
            }
            else
            {
                movingRight = true;
            }
        }
        if (movingRight == true)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        //animator.SetBool("Death", true);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
