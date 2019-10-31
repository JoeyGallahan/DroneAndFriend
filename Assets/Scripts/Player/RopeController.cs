﻿using System.Collections;
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

    private void Awake()
    {
        drone = GameObject.FindGameObjectWithTag("Drone").GetComponent<DroneController>();
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
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

    private void FixedUpdate()
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
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, rope.connectedAnchor);
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
            Debug.Log("Hello");
        }
        else if (maybe)
        {
            rb.isKinematic = false;
            rb.AddForce(savedVelocity, ForceMode2D.Impulse);

            savedVelocity = new Vector2(0.0f, 0.0f);

            Debug.Log("Hey there");
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