﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Paralax : MonoBehaviour
{
    public Camera cam;
    public GameObject player;
    Vector2 startPosition;
    float startZ;
    Vector2 travel => (Vector2)cam.transform.position - startPosition;
    float distanceFromSubject => transform.position.z - player.transform.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;
    

    public void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    public void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 newPos = startPosition + travel*parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }

}
