using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatformController : MonoBehaviour
{
    [SerializeField] GameObject collidedObject;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collidedObject = collision.gameObject;
        rb = collidedObject.GetComponent<Rigidbody2D>();

        if (collidedObject.tag.Equals("Player"))
        {
            rb.AddForce(force);
        }
    }
}
