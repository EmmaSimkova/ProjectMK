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
    
    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
