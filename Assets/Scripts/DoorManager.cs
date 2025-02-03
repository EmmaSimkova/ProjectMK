using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorManager : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    public GameObject player;
    public Image fadeScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find the doors in the scene
        if(door1 == null || door2 == null)
        {
            door1 = GameObject.Find("Door1");
            door2 = GameObject.Find("Door2");
        }
    }

    public IEnumerator Travel(GameObject door)
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        Vector2 otherDoorPosition = door == door1 ? door2.transform.position : door1.transform.position;
        //fade out the screen
        fadeScreen.color = Color.black;
        fadeScreen.CrossFadeAlpha(0, 0, false);
        fadeScreen.CrossFadeAlpha(1, 1, false);
        yield return new WaitForSeconds(1);
        //move the player to the other door
        player.transform.position = otherDoorPosition;
        //fade in the screen
        fadeScreen.color = Color.gray;
        fadeScreen.CrossFadeAlpha(0, 1, false);
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //update the player to be affected by gravity
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}
