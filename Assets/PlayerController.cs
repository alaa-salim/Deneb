using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;

    private float horizontalMove;
    private float verticalInput;
    bool jumpInput = false;
    private bool attackInput;

    public bool canJump = true;
    public Transform spawnPoint;

    [SerializeField]
    private bool isGrounded = true;

    public bool facingRight = false;

    public float runSpeed = 5;
    public float jumpStrength = 15;

    public Transform groundCheckOrigin;
    public float checkRadius = .2f;
    public LayerMask groundLayer;

    //Health
    public Health health;
    public int heartCount = 3;
    public GameObject deathEffect;

    //Knockback
    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;



    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;
    //this one is probably not a good idea, but spaghetti code is as spaghetti code does
    public bool invulnerable = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.numOfHearts = heartCount;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        GroundCheck();
        Animations();
        Jump();

    }

    void FixedUpdate()
    {
        
        Run();

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void GetInput()
    {
        
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        jumpInput = Input.GetButtonDown("Jump");
        attackInput = Input.GetKey(KeyCode.Z);
    }


    /*public IEnumerator Knockback(float knockbackLength, float knockbackPower, Vector3 knockbackDir)
    {
        float timer = 0;
        StartCoroutine(FlashCo());
        while(knockbackLength > timer)
        {
            timer += Time.deltaTime;
            rb.AddForce(new Vector3(knockbackDir.x * -100, knockbackDir.y * knockbackPower, transform.position.z));
        }
        yield return 0;
    }*/

    public IEnumerator Knockback(float knockbackLength, float knockbackPower, Vector3 knockbackDir, bool knockRight)
    {
        float timer = 0;
        StartCoroutine(FlashCo());
        while (knockbackLength > timer)
        {
            timer += Time.deltaTime;
            if (knockRight == false)
            {
                rb.AddForce(new Vector3(knockbackDir.x * -100, knockbackDir.y * knockbackPower, transform.position.z));
            }
            else
            {
                rb.AddForce(new Vector3(knockbackDir.x * 100, knockbackDir.y * knockbackPower, transform.position.z));
            }
        }
        yield return 0;
    }

    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        invulnerable = true;
        while(temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        invulnerable = false;
        triggerCollider.enabled = true;
    }


    private void Run()
    {
        rb.velocity = new Vector2(horizontalMove * runSpeed, rb.velocity.y);
        if (facingRight && horizontalMove < 0)
        {
            Flip();
        }
        else if (facingRight == false && horizontalMove > 0)
        {
            Flip();
        }
    }


    private void Jump()
    {
        //You must be on the ground to jump
        if (!isGrounded)
            return;

        //you can't hold down the jump button and keep jumping
        if (!canJump)
            return;


        //finally, if you are pressing the jump button, you will jump
        if(jumpInput)
        {
            //disables double jump
            canJump = false;

            rb.velocity = Vector2.up * jumpStrength;
          //  rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            animator.SetBool("IsJumping", true);
        }
    }


    

    private void GroundCheck()
    {


        isGrounded = Physics2D.OverlapCircle(groundCheckOrigin.position, checkRadius, groundLayer);
        if(isGrounded)
        {
            canJump = true;
            animator.SetBool("IsJumping", false);
        }

      
    }

    private void Animations()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }


    private void Flip()
    {

        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }


    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
        if (health.health <=0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = spawnPoint.position;
        health.health = heartCount;
    }

    void Die()
    {
        //animator.SetBool("Death", true);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
