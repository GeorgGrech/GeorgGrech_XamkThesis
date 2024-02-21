using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyBorder : MonoBehaviour
{
    [SerializeField] private Vector2 moveTo; //Vector2 since just x and z should be influenced
    [SerializeField] private float moveForce;//Vector2 since just x and z should be influenced
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) //Do for both players and enemies
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            Vector3 moveToV3 = new Vector3(moveTo.x, rb.position.y, moveTo.y); //Use y as z

            Vector3 dir = (moveToV3 - rb.position).normalized;
            rb.AddForce(dir * moveForce); //Keep adding force
        }
    }
}