using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] KeyCode keyCodeUp = KeyCode.W;
    [SerializeField] KeyCode keyCodeDown = KeyCode.S;
    [SerializeField] KeyCode keyCodeRight = KeyCode.D;
    [SerializeField] KeyCode keyCodeLeft = KeyCode.A;

    [SerializeField] float maxMovementSpeed = 5.0f;
    [SerializeField] float maxDistance;

    [SerializeField] float idleMoveDistance;
    [SerializeField] float idleMoveSpeed;
    [SerializeField] bool isIdle = true;
    [SerializeField] bool goingDownIdle = false;
    float idleStartTime = 0.0f;
    Vector3 idleStartPos;
    Vector3 idleEndPos;


    bool withinDistance = true;
    //[SerializeField] float acceleration = 0.5f;

    [SerializeField] bool[] movementDirections = { false, false, false, false }; //up, down, right, left

    GameObject player;
    [SerializeField] Color closeColor;
    [SerializeField] Gradient closeColorGradient;
    [SerializeField] Color farColor;
    [SerializeField] Gradient farColorGradient;

    bool canMove = true;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            GetInput();
            IdleAnimations();
        }
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            UpdateDronePosition();
            UpdateDroneColor();
        }
    }

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
            movementDirections[2] = false;
            movementDirections[3] = false;
        }
    }

    public void UpdateDronePosition()
    {
        Vector3 newPosition = transform.position;


        if (movementDirections[0]) //if moving up
        {
            newPosition.y += maxMovementSpeed;
        }
        else if (movementDirections[1]) //if moving down
        {
            newPosition.y -= maxMovementSpeed;
        }

        if (movementDirections[2]) //if moving right
        {
            newPosition.x += maxMovementSpeed;
        }
        else if (movementDirections[3]) //if moving left
        {
            newPosition.x -= maxMovementSpeed;
        }

        transform.position = newPosition;

        if (Vector3.Distance(transform.position, player.transform.position) > maxDistance)
        {
            withinDistance = false;
        }
        else
        {
            withinDistance = true;
        }
    }

    void UpdateDroneColor()
    {
        if (!withinDistance)
        {
            gameObject.GetComponent<SpriteRenderer>().color = farColor;

            gameObject.GetComponentInChildren<TrailRenderer>().colorGradient = farColorGradient;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = closeColor;
            gameObject.GetComponentInChildren<TrailRenderer>().colorGradient = closeColorGradient;
        }
    }
    
    public bool WithinRopeDistance() { return withinDistance; }

    void IdleAnimations()
    {
        bool before = isIdle;
        isIdle = true;
        for(int i = 0; i < 4; i++)
        {
            if(movementDirections[i])
            {
                isIdle = false;
                break;
            }
        }

        if(isIdle && !before)
        {
            InitIdle();
        }

        if (isIdle)
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

    void InitIdle()
    {
        idleStartPos = transform.position;
        idleEndPos = transform.position;
        idleEndPos.y += idleMoveDistance;

        idleStartTime = Time.time;

        //Debug.Log("Start: " + idleStartPos + ", End: " + idleEndPos);
    }

    void SwapIdleDirection()
    {
        idleEndPos = idleStartPos;
        idleStartPos = transform.position;
        idleStartTime = Time.time;

        //Debug.Log("SWAP! Start: " + idleStartPos + ", End: " + idleEndPos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("ImpassableForPlayer"))
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<BoxCollider2D>());
        }
    }

    public void CanMove(bool maybe)
    {
        canMove = maybe;

        idleStartPos = transform.position;
        idleEndPos = transform.position;
    }

    public bool CanMove()
    {
        return canMove;
    }

}
