using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpassableArea : MonoBehaviour
{
    [SerializeField] GameObject thisGuyIsOkay;

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (tag.Equals("ImpassableForDrone"))
        {
            thisGuyIsOkay = GameObject.FindGameObjectWithTag("Player");
        }
        else if (tag.Equals("ImpassableForPlayer"))
        {
            thisGuyIsOkay = GameObject.FindGameObjectWithTag("Drone");
        }

        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), thisGuyIsOkay.GetComponent<BoxCollider2D>());
    }

}
