using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Demo class for testing health & basic enemy functionality
/// </summary>
public class DemoEnemy : MonoBehaviour
{
    public GrappleObject grappleAttached; //Grapple that is attached to this enemy

    public WaveManager waveManager;

    private Rigidbody rb;
    public GameObject explosionVfx;

    private bool isDead = false;

    [Space(10)]
    [Header("DDA-Related Health settings")]
    [SerializeField] private float minHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float defaultHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Health health = GetComponent<Health>();
        int healthVal = GetHealth();
        health.health = healthVal;
        health.maxHealth = healthVal; //This one probably doesnt do anything
    }
    public void OnDeath()
    {
        waveManager.OnEnemyKilled();

        if(CompareTag("boid"))
            waveManager.RemoveBoidFromList(GetComponent<Boid>());

        DetachGrapple(); //If player is attached to this enemy, stop grapple function
        //Destroy(gameObject);
        isDead = true;
        if(CompareTag("motorbike"))
        {
            Instantiate(explosionVfx, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            
        }
    }

    public void GrappleAttached(GrappleObject grapple)
    {
        grappleAttached = grapple;
    }

    private void DetachGrapple()
    {
        if(grappleAttached != null)
        {
            grappleAttached.CancelGrapple();
        }
    }

    void Update()
    {
        if(isDead)
        {
            if (transform.position.y > 0.5f)
            {
                rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(isDead)
        {
            Instantiate(explosionVfx, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public int GetHealth()
    {
        return (int)DDAManager._instance.GetDynamicValue(false, false, minHealth, maxHealth, defaultHealth);
    }
}
