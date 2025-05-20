using UnityEngine;

public class BackgroundMusicScript : MonoBehaviour
{
    public static BackgroundMusicScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

}
