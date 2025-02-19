using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class GateExit : MonoBehaviour
{
    public bool letsGo = false;
    [SerializeField] private float freqency = 0.1f;
    private Color CurrentGateColor;
    private float[] cgl = new float[6]{0.4f, 0.6f, 0.4f, 0.6f, 0.4f, 0.6f};
    [SerializeField] private GameObject fadeout;
    [SerializeField] private Exitter exitter;
    [SerializeField]private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        //generate random starting color
        CurrentGateColor = new Color(Random.Range(cgl[0], cgl[1]), Random.Range(cgl[2], cgl[3]), Random.Range(cgl[4], cgl[5]), 1);
        GetComponent<Renderer>().material.color = Color.black;
    }

    // Update is called once per frame
    public IEnumerator UpdateColor()
    {
        Vector3 colorMovement;
        // generate random color vector of movement and move the color
        bool withinBounds = false;
        while (!withinBounds)
        {
            colorMovement = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
            Color tempColor = new Color(CurrentGateColor.r + colorMovement.x, CurrentGateColor.g + colorMovement.y, CurrentGateColor.b + colorMovement.z, 0);
            if (tempColor.r >= cgl[0] && tempColor.r <= cgl[1] && tempColor.g >= cgl[2] && tempColor.g <= cgl[3] && tempColor.b >= cgl[4] && tempColor.b <= cgl[5])
            {
                withinBounds = true;
                CurrentGateColor = tempColor;
            }
        }
        GetComponent<Renderer>().material.color = CurrentGateColor;
        yield return new WaitForSeconds(freqency);
        StartCoroutine(UpdateColor());
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("brm brm");
        if (letsGo && other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            letsGo = false;
            exitter.PlayAnimation();
        }
    }
}
