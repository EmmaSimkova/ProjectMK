using UnityEngine;

public class MethCouldron : MonoBehaviour
{
    public int requiredMeth = 1000;
    [SerializeField] private int currentMeth = 0;
    private GameObject player;
    public bool hasbeenFilled = false;
    [SerializeField]private GateExit gateExit;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    
    AudioManagerScript audioManagerScript;
    
    private void Awake()
    {
        audioManagerScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        text.text = currentMeth + "/" + requiredMeth;
    }
    
    public void AddMeth()
    {
        currentMeth += player.GetComponent<TouchGrass>().storedMeth;
        player.GetComponent<TouchGrass>().storedMeth = 0;
        text.text = currentMeth + "/" + requiredMeth;
        if (currentMeth >= requiredMeth)
        {
            audioManagerScript.PlaySFX(audioManagerScript.depozit);
            hasbeenFilled = true;
            gateExit.letsGo = true;
            StartCoroutine(gateExit.UpdateColor());
        }
    }
}
