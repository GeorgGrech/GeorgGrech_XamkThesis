using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// Demo class for testing health
/// </summary>
public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public bool dead = false;

   
    public Slider healthSlider;

    //BoidManager bm;

    private void Start()
    {
        health = maxHealth;
        //bm = GameObject.Find("Boid Manager").GetComponent<BoidManager>();
        
    }

    public void ChangeHealth(int amount) //Used for adding or subtracting health, based on if amount is positive or negative
    {
       

        if (!dead)
        {
            health += amount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health <= 0)
            {
                health = 0;
                Death();
            }   
            Debug.Log(name+" health: "+health);

            SendMessageUpwards("TrackDamage", -amount, SendMessageOptions.DontRequireReceiver); //Track player damage for DDA system. Will only trigger on player with playerDDATracker script
        }

         if(healthSlider != null)
        {
            Debug.Log("Found once");
            healthSlider.value = health / (float)maxHealth;
        }
        
        
    }

    public void Death()
    {
        dead = true;
        Debug.Log(name + " killed.");
        
        gameObject.SendMessageUpwards("OnDeath", SendMessageOptions.DontRequireReceiver);

        if(CompareTag("Player"))
        {
            //SceneManager.LoadScene(2);
            FindObjectOfType<WaveManager>().GameOver();
        }
        //Destroy(gameObject);
        //StartCoroutine(DestroyObject(this.gameObject));
    }

    public void Update()
    {
        
    }

    /*IEnumerator DestroyObject(GameObject gameObject)
    {
        Debug.Log("DestroyObject");
        yield return new WaitForSeconds(2);
       if (gameObject)
       {
        Destroy(gameObject);
       }
    }*/
}
