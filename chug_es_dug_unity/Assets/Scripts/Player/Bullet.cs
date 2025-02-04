using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 40;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy!=null)
        {
            enemy.Hit();
            enemy.TakeDamage(damage);         
        }
        if (!(hitInfo.gameObject.CompareTag("Turn")|| hitInfo.gameObject.CompareTag("Player") || hitInfo.gameObject.CompareTag("Ladder")|| hitInfo.gameObject.CompareTag("Collectable")))
            Destroy(gameObject);


    }

}
