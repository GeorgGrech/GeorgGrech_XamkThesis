using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyBorder : MonoBehaviour
{
    [SerializeField] private Vector2 moveTo; //Vector2 since just x and z should be influenced
    [SerializeField] private float moveForce;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            Vector3 moveToV3 = new Vector3(moveTo.x, rb.position.y, moveTo.y); //Use y as z

            Vector3 dir = (moveToV3 - rb.position).normalized;
            rb.AddForce(dir * moveForce); //Keep adding force
        }
    }
}
