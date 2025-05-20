using UnityEngine;
using UnityEngine.UI;

public class SwapHPImage : MonoBehaviour
{
    //image for health point if enabled
    [SerializeField] private Sprite healthPointEnabled;
    //image for health point if disabled
    [SerializeField] private Sprite healthPointDisabled;
    
    // Start is called before the first frame update
    void Start()
    {
        //check if the images are not null
        if (healthPointEnabled == null || healthPointDisabled == null)
        {
            //find the images in Resources folder
            healthPointEnabled = Resources.Load<Sprite>("Resources/PlayerHPImages/HealthPointEnabled");
            healthPointDisabled = Resources.Load<Sprite>("Resources/PlayerHPImages/HealthPointDisabled");
        }
    }

    //Swap the image from enabled to disabled
    public void SwapImageToDisabled()
    {
        this.GetComponent<Image>().sprite = healthPointDisabled;
    }
    
    //Swap the image from disabled to enabled
    public void SwapImageToEnabled()
    {
        this.GetComponent<Image>().sprite = healthPointEnabled;
    }
}
