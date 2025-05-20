using System.Collections;
using UnityEngine;

public class PickaxeHitRotate : MonoBehaviour
{
    [SerializeField] private GameObject pickaxe;
    [SerializeField] public bool isPickaxeHitting;
    public void PickaxeHit(int rotation)
    {
        isPickaxeHitting = true;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        pickaxe.gameObject.SetActive(true);
        //TODO: play the animation
        //TODO: deactivate the pickaxe after the animation
        StartCoroutine(DebugEnumerator());
    }
    
    //debug enumerator TODO: remove this
    private IEnumerator DebugEnumerator()
    {
        Debug.Log("Hit started");
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Hit ended");
        isPickaxeHitting = false;
        pickaxe.gameObject.SetActive(false);
    }
}
