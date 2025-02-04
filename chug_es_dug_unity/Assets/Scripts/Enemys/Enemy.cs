using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPun,IPunObservable
{
    public int health = 100;
    public int damage = 40;
    public int death = 0;
    public Animator anim;
    public PlayerController player;
    public float speed;
    public bool MoveRight=false;
    public bool moving=true;

    public void Start()
    {
        anim = GetComponent<Animator>();  
    }

    public void Update()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        Walk();
    }

    public void Walk()
    {
        if (health > 0)
        {
            if (moving)
            {
                if (MoveRight)
                {
                    transform.Translate(2 * Time.deltaTime * speed, 0, 0);
                    transform.localScale = new Vector2(1, 1);
                    anim.SetBool("Walk", true);
                    moving = true;
                }
                else
                {
                    transform.Translate(-2 * Time.deltaTime * speed, 0, 0);
                    transform.localScale = new Vector2(-1, 1);
                    anim.SetBool("Walk", true);
                    moving = true;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health<=0)
        {
            anim.SetTrigger("Death");          
        }
    }

    public void Hit()
    {
        moving = false;
        anim.SetBool("Walk", false);
        anim.SetBool("Hit",true);
    }
    public void HitEnd()
    {
        anim.SetBool("Hit", false);
        anim.SetBool("Walk", true);
        moving = true;
    }

    public void JumpedOn()
    {
        anim.SetTrigger("Death");      
    }

    public void Death()
    {
        death = 1;
        player.KillCounter(death);
        Destroy(this.gameObject);      
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Turn"))
        {
            if (MoveRight)
            {
                MoveRight = false;
                anim.SetBool("Walk", true);
            }
            else
            {
                MoveRight = true;
                anim.SetBool("Walk", true);
            }
        }
    }

    public void OnPhotonSerializedView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Vector3 pos = transform.localPosition;
            stream.Serialize(ref pos);
        }
        else
        {
            Vector3 pos = Vector3.zero;
            stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
        }
    }
}

