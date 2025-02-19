using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exitter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void ExitGame()
    {
        Debug.Log("Exiting game");
        Application.Quit();
    }
    
    public void PlayAnimation()
    {
        animator.SetTrigger("Zapinak");
    }
}
