using UnityEngine;

public class PlayerKillOnFall : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < this.transform.position.y)
        {
            player.transform.position = respawnPoint.position;
        }
    }
}
