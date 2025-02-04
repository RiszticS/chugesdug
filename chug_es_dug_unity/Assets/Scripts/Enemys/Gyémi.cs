using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyémi : Enemy
{

    private new void Update()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        Walk();
        if (player.transform.position.x-this.transform.position.x>5)
        {
            moving = false;
            anim.SetBool("Hide", true);
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Show", false);
        }
    }

    public void AlertObservers(string message)
    {
         
        if (message.Equals("ShowEnded"))
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Show", false);
            anim.SetBool("Hide", false);
            anim.SetBool("Idle", false);
            moving = true;
        }
        if (message.Equals("HideEnded"))
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Show", false);
            anim.SetBool("Hide", false);
            anim.SetBool("Idle", true);
            moving = false;
        }
        if (message.Equals("DeathStart"))
        {
            moving = false;
        }
    }
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             anim.SetBool("Show", true);
        }
    }

}
