using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MethShard : MonoBehaviour
{
    [SerializeField] bool isCollected;
     
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(CollectAfterTime());
    }
    
    //collision with platform detection
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3)
        {
            isCollected = true;
            StartCoroutine(Movement());
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //add meth to the player
            player.GetComponent<TouchGrass>().AddMeth();
            //destroy the meth shard
            Destroy(this.gameObject);
        }
    }
    
    private IEnumerator Movement()
    {
        Vector3 point0 = transform.position;
        Vector3 point1 = player.position;
        float t = 0f;
        float speed = 1f;
        while (t <1)
        {
            t += Time.deltaTime * speed;
            point0 = transform.position; 
            point1 = player.position;
            // Interpolate position between the two points
            Vector3 positionOnLine = Vector3.Lerp(point0, point1, t);

            // Move the object along the line
            transform.position = positionOnLine;
            yield return new WaitForFixedUpdate();
        }
       
    }

    private IEnumerator CollectAfterTime()
    {
        yield return new WaitForSeconds(4f);
        if (isCollected)
        {
            StopCoroutine(CollectAfterTime());
        }
        StartCoroutine(Movement());
    }
}
