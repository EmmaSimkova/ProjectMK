using UnityEngine;

public class WallJumping : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            playerMovement.otherWall = other.gameObject;
            playerMovement.canWallJump = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            playerMovement.otherWall = null;
            playerMovement.canWallJump = false;
        }
    }
}
