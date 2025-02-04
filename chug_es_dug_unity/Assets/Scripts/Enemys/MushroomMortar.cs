using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MushroomMortar : MonoBehaviour
{
    public Transform firePoint;
    public GameObject sporecannonballprefab;
    public SporeCannonball sporeCannonball;
    

    void Update()
    {
        sporeCannonball = GameObject.FindObjectOfType<SporeCannonball>();

        if (Input.GetButtonDown("Fire1"))
        {
            
            Launch();
        }
    }
    public void Launch()
    {
        Instantiate(sporecannonballprefab, firePoint.transform);
        sporeCannonball = GameObject.FindObjectOfType<SporeCannonball>();
        sporeCannonball.Launch();
    }
}
