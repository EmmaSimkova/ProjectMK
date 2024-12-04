using System;
using System.Collections;
using System.Collections.Generic;
using BaseStuff;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private TouchGrass playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private bool isPickaxe;
    [SerializeField] private bool knockingBack;
    
    [SerializeField] private float knockbackX;
    [SerializeField] private float knockbackY;
    
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
        Debug.Log(this.gameObject.name + " collided with " + other.gameObject.name);
        if (other.gameObject.TryGetComponent(out Damage dmg2))
        {
            if (!playerHealth.canTakeDamage) return;
            playerHealth.canTakeDamage = false;
            if(!isPickaxe){StartCoroutine(TriggerEnter(other));}
            
            Knockback(other);
        }
        if (other.gameObject.TryGetComponent(out MethCrystal methCrystal))
        {
            if (isPickaxe)
            {
                methCrystal.Collect();
            }
        }
    }
    
    //knockback function
    public void Knockback(Collider2D other)
    {
        if (knockingBack) return;
        GameObject player = playerHealth.GameObject();
        knockingBack = true;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<PlayerMovement>().canMove = false;
        Debug.Log("Knockback");
        Vector2 knockbackDirection;
        //flip the knockback direction based on the player's position relative to the object
        if (!isPickaxe)
        {
            knockbackDirection.x = (other.transform.position.x - transform.position.x > 0 ? -knockbackX : knockbackX);
            knockbackDirection.y = (other.transform.position.y - transform.position.y > 0 ? -0.3f*knockbackY : knockbackY);
        }else
        {
            knockbackDirection.x = (other.transform.position.x - transform.position.x > 0 ? -knockbackX : knockbackX);
            knockbackDirection.y = (other.transform.position.y - transform.position.y > 0 ? -knockbackY : 2*knockbackY);
        }

        
        
        player.GetComponent<Rigidbody2D>().AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(KnockbackTimer());
    }
    
    //give back control to the player after a short time
    private IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(0.35f);
        playerMovement.canMove = true;
        knockingBack = false;
    }
    
    
    // Hurtbox collision and player taking damage
    private IEnumerator TriggerEnter(Collider2D other)
    {
        if (other.gameObject.GetComponent<Damage>())
        {
            Debug.Log("Player took " + other.gameObject.GetComponent<Damage>().damage + " damage");
            //take damage
            playerHealth.TakeDamage(other.gameObject.GetComponent<Damage>().damage);
            yield return new WaitForSeconds(0.04f);
            
            //disable the hurtbox for a short time
            yield return new WaitForSeconds(1.5f);
            playerHealth.canTakeDamage = true;
            //flicker player hitbox to make sure the player takes damage once invincibility is over
            playerHealth.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            playerHealth.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
