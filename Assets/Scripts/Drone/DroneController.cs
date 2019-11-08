using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    //Movement
    [SerializeField] float maxMovementSpeed = 55.0f;
    [SerializeField] bool canMove = false;
    float moveHor = 0.0f;
    float moveVer = 0.0f;

    //Idling
    [SerializeField] float idleMoveDistance;
    [SerializeField] float idleMoveSpeed;
    bool isIdle = true;
    bool goingDownIdle = false;
    float idleStartTime = 0.0f;
    Vector3 idleStartPos;
    Vector3 idleEndPos;

    //Player
    GameObject player;

    //Colors
    [SerializeField] float maxDistance;
    bool withinDistance = true;
    SpriteRenderer droneColor;
    TrailRenderer trailColor;
    [SerializeField] Color closeColor;
    [SerializeField] Gradient closeColorGradient;
    [SerializeField] Color farColor;
    [SerializeField] Gradient farColorGradient;

    Rigidbody2D rb;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        droneColor = this.gameObject.GetComponent<SpriteRenderer>();
        trailColor = gameObject.GetComponentInChildren<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            UpdateDronePosition();
            IdleAnimations();
            UpdateDistanceToPlayer();
            UpdateDroneColor();
        }
    }

    public void UpdateDronePosition()
    {
        moveHor = Input.GetAxis("Horizontal") * maxMovementSpeed;
        moveVer = Input.GetAxis("Vertical") * maxMovementSpeed;

        Vector2 movement = new Vector2(moveHor, moveVer);

        rb.velocity = movement * maxMovementSpeed * Time.deltaTime;
    }

    //Updates the color of the drone based on the distance to the player
    void UpdateDroneColor()
    {
        if (!withinDistance) //If you're far away from the player
        {
            droneColor.color = farColor; //Change the color of the drone to the preset
            trailColor.colorGradient = farColorGradient; //Change the color of the drone's trail to the preset
        }
        else
        {
            droneColor.color = closeColor; //Change the color of the drone to the preset
            trailColor.colorGradient = closeColorGradient; //Change the color of the drone's trail to the preset
        }
    }

    void UpdateDistanceToPlayer()
    {
        withinDistance = true;

        if (Vector2.Distance(transform.position,player.transform.position) > maxDistance)
        {
            withinDistance = false;
        }
    }
    
    public bool WithinRopeDistance() { return withinDistance; }

    //Just a simple animation that makes the drone look like it's hovering a bit when you're not moving
    void IdleAnimations()
    {
        //If you weren't idle before, you are now
        bool before = isIdle;
        isIdle = true;

        for(int i = 0; i < 4; i++)
        {
            if(moveVer != 0.0f || moveHor != 0.0f) //If you made any movements at all, you're not idle
            {
                isIdle = false; //switch it back
                break; //don't need to go any further. we found what we're looking for.
            }
        }

        //If you just now became idle
        if(isIdle && !before)
        {
            InitIdle(); //Initialize the idle fields
        }

        if (isIdle) //If you are idle but this is not the first frame
        {            
            float timePassed = Time.time - idleStartTime; 
            float distanceCovered = timePassed * idleMoveSpeed;

            transform.position = Vector3.Lerp(idleStartPos, idleEndPos, distanceCovered);

            if (distanceCovered >= 1)
            {
                SwapIdleDirection();
            }
        }
    }

    //Initializes the start and end positions of the 
    void InitIdle()
    {
        idleStartPos = transform.position;
        idleEndPos = transform.position;
        idleEndPos.y += idleMoveDistance;

        idleStartTime = Time.time;
    }

    //Switch directions for the idle
    //He hovers up to the end position and then we swap so he goes back down to the start 
    void SwapIdleDirection()
    {
        idleEndPos = idleStartPos;
        idleStartPos = transform.position;

        idleStartTime = Time.time; //reset the time we started going idle
    }

    //Updates whether or not the drone can move (used for when the game is paused)
    public void CanMove(bool maybe)
    {
        canMove = maybe;

        //Reset the idle positions so that it doesn't get wonky
        idleStartPos = transform.position; 
        idleEndPos = transform.position;
    }

    public bool CanMove()
    {
        return canMove;
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }

}
