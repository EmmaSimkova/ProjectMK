using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethCouldron : MonoBehaviour
{
    public int requiredMeth = 1000;
    [SerializeField] private int currentMeth = 0;
    private GameObject player;
    public bool hasbeenFilled = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    
    public void AddMeth()
    {
        currentMeth += player.GetComponent<TouchGrass>().storedMeth;
        player.GetComponent<TouchGrass>().storedMeth = 0;
        if (currentMeth >= requiredMeth)
        {
            hasbeenFilled = true;
        }
    }
}
