using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    //Input Keys
    KeyCode keyCodeUp = KeyCode.W;
    KeyCode keyCodeDown = KeyCode.S;
    KeyCode keyCodeRight = KeyCode.D;
    KeyCode keyCodeLeft = KeyCode.A;

    //Movement
    [SerializeField] float maxMovementSpeed = 5.0f;
    bool[] movementDirections = { false, false, false, false }; //up, down, right, left
    bool canMove = false;

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
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        droneColor = this.gameObject.GetComponent<SpriteRenderer>();
        trailColor = gameObject.GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            GetInput();
            IdleAnimations();

            UpdateDronePosition();
            UpdateDroneColor();
        }
    }

    //Gets the vertical and horizontal inputs that the drone should react to
    void GetInput()
    {
        if (Input.GetKey(keyCodeUp))
        {
            //Can't move both up and down
            movementDirections[0] = true;
            movementDirections[1] = false;
        }
        else if (Input.GetKey(keyCodeDown))
        {
            //Can't move both up and down
            movementDirections[1] = true;
            movementDirections[0] = false;
        }
        else
        {
            //If you don't press up or down then you don't move vertically at all
            movementDirections[0] = false;
            movementDirections[1] = false;
        }

        if (Input.GetKey(keyCodeRight))
        {
            //Can't move both right and left
            movementDirections[2] = true;
            movementDirections[3] = false;
        }
        else if (Input.GetKey(keyCodeLeft))
        {
            //Can't move both right and left
            movementDirections[3] = true;
            movementDirections[2] = false;
        }
        else
        {
            //If you don't press left or right then you don't move horizontally at all
            movementDirections[2] = false;
            movementDirections[3] = false;
        }
    }

    //Updates the drone position based on the direction the drone is moving
    public void UpdateDronePosition()
    {
        Vector3 newPosition = transform.position;

        //Vertical movement
        if (movementDirections[0]) //if moving up
        {
            newPosition.y += (maxMovementSpeed * Time.deltaTime);
        }
        else if (movementDirections[1]) //if moving down
        {
            newPosition.y -= (maxMovementSpeed * Time.deltaTime);
        }

        //Horizontal movement
        if (movementDirections[2]) //if moving right
        {
            newPosition.x += (maxMovementSpeed * Time.deltaTime);
        }
        else if (movementDirections[3]) //if moving left
        {
            newPosition.x -= (maxMovementSpeed * Time.deltaTime);
        }

        transform.position = newPosition; //update the position

        //Check to see if the drone is out of player range
        if (Vector3.Distance(transform.position, player.transform.position) > maxDistance)
        {
            withinDistance = false;
        }
        else
        {
            withinDistance = true;
        }
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
    
    public bool WithinRopeDistance() { return withinDistance; }

    //Just a simple animation that makes the drone look like it's hovering a bit when you're not moving
    void IdleAnimations()
    {
        //If you weren't idle before, you are now
        bool before = isIdle;
        isIdle = true;

        for(int i = 0; i < 4; i++)
        {
            if(movementDirections[i]) //If you made any movements at all, you're not idle
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

}
