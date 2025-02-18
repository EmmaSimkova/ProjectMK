using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorManager : MonoBehaviour
{
    public GameObject[] door1;
    public GameObject[] door2;
    public GameObject player;
    public Image fadeScreen;

    public IEnumerator Travel(GameObject door)
    {
        int index = -1;
        //find the index of the door that was clicked
        for (int i = 0; i < door1.Length; i++)
        {
            if (door1[i] == door)
            {
                index = i;
                door = door2[index];
                break;
            }
            if (door2[i] == door)
            {
                index = i;
                door = door1[index];
                break;
            }
        }

        if (index == -1)
        {
            Debug.LogError("Door not found in either door1 or door2 arrays.");
            yield break;
        }
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        Vector2 otherDoorPosition = door.transform.position;
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
