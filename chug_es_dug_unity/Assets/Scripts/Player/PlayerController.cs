using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviourPun
{
    public Rigidbody2D rb;
    public Animator anim;
    public Transform firePoint;
    public Transform future;
    public GameObject bulletPrefab;
    public Image[] hpImage;
    private GameObject[] imageObject;
    public Text killCounterText;
    public PhotonView view;
    public Collider2D groundCheckColl;
    public Ladder ladder;
    public Collider2D coll;
    private Scene currentScene;
    public GameManager gameManager;
    public SpriteRenderer sr;

    public int killCounter = 0;
    public int collectable = 0;
    private float hangtime = .2f;
    private float hangCounter;
    private bool shootended = true;
    private bool canmove = true;
    public bool moving = false;
    public bool crouch = false;
    public bool jumping = false;
    public bool facingLeft = false;
    public bool grabbing = false;
    public bool ledgeclimbingended = true;
    public bool isGrounded = false;
    public bool canclimb = false;
    public bool climbing = false;
    public bool bottomLadder = false;
    public bool topLadder = false;
    public bool isDead = false;
    public int level = 1;

    private enum State {Idle,Walk,Jump,Fall,Hurt,Shoot,WalkShoot,CrouchDown,CrouchUp,Crouching,LedgeClimbing,Climb }
    private State state = State.Idle;
      
    [SerializeField] private float speed=4f;
    [SerializeField] private float jumpForce=10f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] public int health = 100;
    [SerializeField] private LayerMask ground;
    [SerializeField] float climbSpeed = 3f;

    private bool greenBox, redBox;
    public float redXOffset, redYOffset,redXSize, redYSize, greenXOffset,greenYOffset, greenXSize, greenYSize;
    public float startingGrav;
    public float startingDrag;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
        killCounterText = FindObjectOfType<Text>();
        imageObject = GameObject.FindGameObjectsWithTag("HpImage");
        gameManager = GameObject.FindObjectOfType<GameManager>();
        startingGrav = rb.gravityScale;
        startingDrag = rb.drag;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            for (int i = 2; i >= 0; i--)
            {
                hpImage[i] = imageObject[i].GetComponent<Image>();
            }
            if (state==State.Climb)
                Climb();
            else if (state != State.Hurt && !crouch)
                Movement();
            GroundCheck();
            AnimationState();
            anim.SetInteger("state", (int)state); //sets animation based on Enumerator state
            killCounterText.text = killCounter.ToString();
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            Destroy(collision.gameObject);
            collectable += 1;
            gameManager.collectable +=1;
        }

        if (collision.CompareTag("Door"))
        {
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (collision.CompareTag("Spikes"))
        {
            Die();
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.Fall)
            {                
                enemy.JumpedOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else
            {
                crouch = false;
                Hurt();
                TakeDamage(enemy.damage);
                HpStat(health);              
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    public void Movement()
    {
        if (canmove == true && !grabbing && isDead==false)
        { 
            float hDirection = CrossPlatformInputManager.GetAxis("Horizontal");
            //moving left
            if (hDirection<0)
            {
                rb.velocity = new Vector2(speed*hDirection, rb.velocity.y);
               // rb.velocity = new Vector2(0f, vDirection * climbSpeed);
                moving = true;
            }
            //moving right
            else if (hDirection>0)
            {
                rb.velocity = new Vector2(speed * hDirection, rb.velocity.y);
                moving = true;
            }
            else moving = false;
            //flipping
            if (hDirection > 0 && facingLeft)
                Flip();
            else if (hDirection < 0 && !facingLeft)
                Flip();
            //climbing
            if (canclimb && Mathf.Abs(CrossPlatformInputManager.GetAxis("Vertical")) > .1f)
            {
                state = State.Climb;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                rb.drag = 8f;
                transform.position = new Vector3(ladder.transform.position.x, rb.position.y);
                rb.gravityScale = 0f;
            }

            Jump();
            CrouchDown();            
            CrouchUp();
            Shoot();
            LedgeClimbing();
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

    public void KillCounter(int death)
    {
        killCounter += death;
        gameManager.kills += death;
    }

    public void HpStat(int health)
    {
        if (health==60)
        {
            hpImage[2].enabled = false;
        }
        else if (health == 20)
        {
            hpImage[1].enabled = false;
        }
        else if (health<0)
        {
            hpImage[0].enabled = false;
        }
    }

    public void Flip()
    {
        facingLeft = !facingLeft;
        transform.Rotate(0f, -180f, 0f);       
    }

    public void Shoot()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && shootended == true)
        {
            if (Mathf.Abs(rb.velocity.x) > 1f)
            {
                state = State.WalkShoot;
            }
            else
            {
                state = State.Shoot;
                canmove = false;
            }
            shootended = false;
            if (currentScene.name=="Singleplayer")
                 Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            else
                PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
        }       
    }

    public void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump") && hangCounter > 0f &&!jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.Jump;
        }             
        else if (CrossPlatformInputManager.GetButtonUp("Jump") && rb.velocity.y > 0) 
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
            state = State.Jump;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump") && canclimb)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.Jump;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canclimb = false;
            rb.gravityScale = startingGrav;
            rb.drag = startingDrag;
        }       
    }          

    public void GroundCheck()
    {
        if (groundCheckColl.IsTouchingLayers(ground))
        {
            isGrounded = true;
            jumping = false;
            hangtime = .2f;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded)
        {
            hangCounter = hangtime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }
    }

    public void Hurt()
    {
        state = State.Hurt;
    }
    
    public void Die()
    {
        sr.enabled=false;
        rb.bodyType=RigidbodyType2D.Static;
        isDead = true;
        view.RPC("IsDead", RpcTarget.All);
    }

    [PunRPC]
    public bool IsDead() 
    {
        if (isDead)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void Respawn(float x,float y,float z)
    {
        sr.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        isDead = false;
        view.RPC("IsDead", RpcTarget.All);
        Vector3 position = new Vector3(x, y, z);
        this.gameObject.transform.position = position;
    }

    public void CrouchDown()
    {
        if (CrossPlatformInputManager.GetButtonDown("Crouch"))
            state = State.CrouchDown;       
    }

    public void CrouchUp()
    {
        if (CrossPlatformInputManager.GetButtonUp("Crouch"))
            state = State.CrouchUp;
    }

    public void LedgeClimbing()
    {
        greenBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYSize), 0f, ground);
        redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYSize), 0f, ground);
        if (greenBox && !redBox && !grabbing)
        {
            grabbing = true;
        }
        if (grabbing)
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.gravityScale = 0f;
            ledgeclimbingended = false;
            state = State.LedgeClimbing;
        }
    }

    public void LedgeClimbingChangePos()
    {
        if (!facingLeft)
            transform.position = new Vector2(transform.position.x + 0.8f, transform.position.y + 0.4f);
        else if (facingLeft)
            transform.position = new Vector2(transform.position.x - 0.8f, transform.position.y + 0.4f);
        rb.gravityScale = startingGrav;
        grabbing = false;
    }

    public void Climb()
    {
        climbing = true;
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Jump();
        }
        float vDirection = CrossPlatformInputManager.GetAxis("Vertical");
        if (state == State.Climb)
        {
            if (vDirection > .1f && !topLadder)
            {
                rb.velocity = new Vector2(0f, vDirection * climbSpeed);
                anim.speed = 1f;
            }
            else if (vDirection < -.1f && !bottomLadder)
            {
                rb.velocity = new Vector2(0f, vDirection * climbSpeed);
                anim.speed = 1f;
            }
            else
            {
                anim.speed = 0f;
            }
        }
        else anim.speed = 1f;        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYSize));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYSize));
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("ShootAnimationEnded"))
        {
            shootended = true;
            canmove = true;
        }
        if (message.Equals("CrouchUpEnded"))
        {
            crouch = false;
            canmove = true;
        }
        if (message.Equals("CrouchDownEnded"))
        {
            crouch = true;
            canmove = false;
        }
        if (message.Equals("LedgeClimbingEnded"))
        {
            ledgeclimbingended = true;
            LedgeClimbingChangePos();
        }
    }

    public void AnimationState()
    {
        if (state == State.Jump)
        {
            jumping = true;
            if (rb.velocity.y < .1f)
                state = State.Fall;
        }
        else if (state == State.Fall)
        {
            if (groundCheckColl.IsTouchingLayers(ground))
            {
                state = State.Idle;
                jumping = false;
            }
        }
        else if (state == State.Hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.Idle;
                canmove = true;
            }
        }
        else if (state == State.Shoot)
        {
            if (shootended)
                state = State.Idle;
        }
        else if (state == State.WalkShoot)
        {
            if ((Mathf.Abs(rb.velocity.x) > 1f))
            {
                state = State.WalkShoot;
                if (shootended)
                    state = State.Walk;
            }
            else if (shootended)
                state = State.Idle;
        }
        else if (state == State.CrouchDown)
        {
            if (crouch)
                state = State.Crouching;
        }
        else if (state == State.Crouching)
        {
            moving = false;
            if (CrossPlatformInputManager.GetButtonUp("Crouch"))
                state = State.CrouchUp;
        }
        else if (state == State.CrouchUp)
        {
            if (!crouch)
                state = State.Idle;
        }
        else if (state == State.LedgeClimbing)
        {
            if (ledgeclimbingended)
            {
                state = State.Idle;
            }
        }
        else if (state == State.Climb)
        {
        }
        else if (Mathf.Abs(rb.velocity.x) > 1f)
        {
            state = State.Walk;
        }
        else
        {
            state = State.Idle;
            canmove = true;
            crouch = false;
            shootended = true;
            grabbing = false;
            jumping = false;
            moving = false;
            ledgeclimbingended = true;
            canclimb = false;
            climbing = false;
        }
    }
}

