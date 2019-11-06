using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [SerializeField] DroneController drone;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] KeyCode ropeShooterButton = KeyCode.Space;
    [SerializeField] SpringJoint2D rope;
    [SerializeField] bool usingRope = false;
    [SerializeField] bool canUseRope = true;
    Rigidbody2D rb;

    Vector2 savedVelocity;

    [SerializeField] float ropeZOffset = 0.0f;

    private void Awake()
    {
        drone = GameObject.FindGameObjectWithTag("Drone").GetComponent<DroneController>();
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canUseRope)
        {
            GetInput();
            UpdateRopePos();
        }
    }

    private void LateUpdate()
    {
        CheckForInputRelease();
    }

    void GetInput()
    {
        if (!usingRope && Input.GetKeyDown(ropeShooterButton) && drone.WithinRopeDistance())
        {
            usingRope = true;
            ShootRope();
        }
    }

    void CheckForInputRelease()
    {
        if (usingRope && !Input.GetKey(ropeShooterButton))
        {
            usingRope = false;
            rope.enabled = false;
            lineRenderer.enabled = false;
        }
    }

    void ShootRope()
    {
        //Setup springy stuff
        rope.enabled = true;
        rope.connectedAnchor = drone.transform.position;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, rope.connectedAnchor);
    }

    void UpdateRopePos()
    {
        if (rope && usingRope)
        {
            rope.connectedAnchor = drone.transform.position;

            Vector3 pos1 = transform.position;
            pos1.z = ropeZOffset;

            Vector3 pos2 = rope.connectedAnchor;
            pos2.z = ropeZOffset;

            lineRenderer.SetPosition(0, pos1);
            lineRenderer.SetPosition(1, pos2);
        }
    }

    public void CanUseRope(bool maybe)
    {
        canUseRope = maybe;

        if(!maybe)
        {
            savedVelocity = rb.velocity;
            rb.isKinematic = true;
            rb.velocity = new Vector2(0.0f,0.0f);
        }
        else if (maybe)
        {
            rb.isKinematic = false;
            rb.AddForce(savedVelocity, ForceMode2D.Impulse);

            savedVelocity = new Vector2(0.0f, 0.0f);
        }
    }

    public bool CanUseRope()
    {
        return canUseRope;
    }

    public void ResetVelocity()
    {
        savedVelocity = new Vector2(0.0f, 0.0f);
        rb.velocity = new Vector2(0.0f, 0.0f);
    }
}
