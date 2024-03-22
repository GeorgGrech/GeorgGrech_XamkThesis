using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSafetyBorder : MonoBehaviour
{
    [SerializeField] private float moveForce;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            rb.AddForce(new Vector3(0, moveForce, 0)); //Keep adding force
        }
    }
}
