using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] public float angle;
    [SerializeField] float force;
    Rigidbody2D rb;
    [SerializeField] GameObject player;
    [SerializeField] GameObject drone;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        rb.AddForce(dir*force);

        player = GameObject.FindGameObjectWithTag("Player");
        drone = GameObject.FindGameObjectWithTag("Drone");

        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), player.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), drone.GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
