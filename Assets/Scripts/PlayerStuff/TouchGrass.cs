using UnityEngine;

public class TouchGrass : MonoBehaviour
{
    [SerializeField] private PlayerHealthUI playerHealthUI;
    
    [Header("Base stats")]
    [SerializeField] public int health;
    [SerializeField][Range(1, 10)] public int maxHealth = 5;
    [SerializeField] public bool canTakeDamage = true;
    [SerializeField] public int storedMeth;
    
    AudioManagerScript audioManagerScript;
    
    private void Awake()
    {
        audioManagerScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }
    
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
        audioManagerScript.PlaySFX(audioManagerScript.hpDamage);
        health -= damage;
        playerHealthUI.UpdateHealthUI();
        if (health <= 0)
        {
            //death = loading the main menu
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            Debug.Log("Dead");
        }
        //TODO: remove this debug
        playerHealthUI.UpdateHealthUIBetter();
    }

    public void AddMeth()
    {
        storedMeth++;
    }
}
