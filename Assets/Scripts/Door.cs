using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorManager doorManager;
    
    // Start is called before the first frame update
    void Start()
    {
        //get the door managerif not already set
        if(doorManager == null)
        {
            doorManager = GameObject.Find("DoorManager").GetComponent<DoorManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(doorManager.Travel(gameObject));
        }
    }
}
