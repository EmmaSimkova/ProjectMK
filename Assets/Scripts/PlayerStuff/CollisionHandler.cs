using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private TouchGrass playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        //make sure the player health is not null
        if (playerHealth == null)
        {
            //get the player health script
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<TouchGrass>();
        }
    }

    //resolve other object collision
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Damage dmg))
        {
            if (dmg.forceDamage) playerHealth.canTakeDamage = true;
        }
        
        if (other.gameObject.TryGetComponent(out Damage dmg2))
        {
            if (!playerHealth.canTakeDamage) return;
            playerHealth.canTakeDamage = false;
            StartCoroutine(TriggerEnter(other));
        }
    }
    
    // Hurtbox collision and player taking damage
    private IEnumerator TriggerEnter(Collider2D other)
    {
        if (other.gameObject.GetComponent<Damage>())
        {
            Debug.Log("Player took " + other.gameObject.GetComponent<Damage>().damage + " damage");
            //take damage
            playerHealth.TakeDamage(other.gameObject.GetComponent<Damage>().damage);
            
            //disable the hurtbox for a short time
            yield return new WaitForSeconds(1.5f);
            playerHealth.canTakeDamage = true;
        }
    }
}
