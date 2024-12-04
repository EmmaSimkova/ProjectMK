using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private TouchGrass playerHealth;
    //image for the health points (points will change image when lost)
    [SerializeField] private GameObject[] healthPoints;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthUIBetter();
    }

    //update image when player takes damage or heals
    public void UpdateHealthUI()
    {
        //loop through all the health points
        for (int i = 0; i < healthPoints.Length; i++)
        {
            //if the health is less than the index, set the health point to disabled
            if (playerHealth.health <= i)
            {
                //Swap the image from enabled to disabled, using the SwapHPImage script in the child
                healthPoints[i].GetComponent<SwapHPImage>().SwapImageToDisabled();
            }
            //if the health is more than the index, set the health point to enabled
            else
            {
                //Swap the image from disabled to enabled, using the SwapHPImage script in the child
                healthPoints[i].GetComponent<SwapHPImage>().SwapImageToEnabled();
            }
        }
    }

    public void UpdateHealthUIBetter()
    {
        //make sure the player health is not null
        if (playerHealth == null)
        {
            //get the player health script
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<TouchGrass>();
        }
        
        //add all the health points to the array from children
        healthPoints = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            healthPoints[i] = transform.GetChild(i).gameObject;
        }
        
        //set all the health points to disabled
        for (int i = 0; i < healthPoints.Length; i++)
        {
            healthPoints[i].SetActive(false);
        }
        
        //create the right amount of health points
        for (int i = 0; i < playerHealth.maxHealth; i++)
        {
            healthPoints[i].SetActive(true);
        }
    }
}
