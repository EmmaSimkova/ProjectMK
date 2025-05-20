using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorManager doorManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(doorManager.Travel(gameObject));
        }
    }
}
