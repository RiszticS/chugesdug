using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeCannonball : MonoBehaviour
{
    public MushroomMortar mushroomMortar;
    public Rigidbody2D sporeCannonball;
    public PlayerController player;
    public Collider2D coll;
    public LayerMask ground;

    public float h = 4;
    public float gravity = -10;

    public bool debugPath;

    void Start()
    {
        mushroomMortar = GameObject.FindObjectOfType<MushroomMortar>();
        player = GameObject.FindObjectOfType<PlayerController>();
        coll = GetComponent<Collider2D>();
        sporeCannonball = GetComponent<Rigidbody2D>();
    }

    void Update()
    {        
       
        if (debugPath)
        {
            DrawPath();
        }

        if (coll.IsTouchingLayers(ground))
        {
            Destroy(gameObject,1);
        }
        else Destroy(gameObject,5);
    }

    public void Launch()
    {

        sporeCannonball.bodyType = RigidbodyType2D.Dynamic;
        Physics.gravity = Vector3.up * gravity;
        sporeCannonball.velocity = CalculateLaunchData().initialVelocity;
    }

    

    LaunchData CalculateLaunchData()
    {
        float displacementY ;
        Vector3 displacementXZ ;
        float time;
        Vector3 velocityY ;
        Vector3 velocityXZ ;

        if (!player.moving)
        {
            displacementY = player.transform.position.y - sporeCannonball.position.y;
             displacementXZ = new Vector3(player.transform.position.x - sporeCannonball.position.x, 0, player.transform.position.z - sporeCannonball.position.x);
             time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
             velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
             velocityXZ = displacementXZ / time;
            return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
        }
        else
        { 
             displacementY = player.future.transform.position.y - sporeCannonball.position.y;
             displacementXZ = new Vector3(player.future.transform.position.x - sporeCannonball.position.x, 0, player.future.transform.position.z - sporeCannonball.position.x);
             time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
             velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
             velocityXZ = displacementXZ / time;
            return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
        }
    }

    void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 futureviousDrawPoint = sporeCannonball.position;

        int resolution = 30;
        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector2 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector2 drawPoint = sporeCannonball.position + displacement;
            Debug.DrawLine(futureviousDrawPoint, drawPoint, Color.green);
            futureviousDrawPoint = drawPoint;
        }
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }
    
    
}
