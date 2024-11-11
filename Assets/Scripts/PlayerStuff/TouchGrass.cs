using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchGrass : MonoBehaviour
{
    [SerializeField] private PlayerHealthUI playerHealthUI;
    
    [Header("Base stats")]
    [SerializeField] public int health;
    [SerializeField] public int maxHealth = 5;
    [SerializeField] public bool canTakeDamage = true;
    
    // Start is called before the first frame update
    void Start()
    {
        //set the health to max health
        health = maxHealth;
        playerHealthUI.UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //take damege
    public void TakeDamage(int damage)
    {
        health -= damage;
        playerHealthUI.UpdateHealthUI();
        if (health <= 0)
        {
            //death or smth
            Debug.Log("Dead");
        }
    }
}
