using UnityEngine;
using System.Collections;

public class BlackScreenController : MonoBehaviour {

    public SpriteRenderer sr;
    public Canvas textBoxCanvas;
    public BlackScreenController[] bscs;

    void Awake()
    {
        sr = this.GetComponent<SpriteRenderer>();
        GameObject[] bscObjs = GameObject.FindGameObjectsWithTag("Screen");
        System.Array.Resize(ref bscs, bscObjs.Length);
        for(int i = 0; i < bscObjs.Length; i++)
        {
            bscs[i] = bscObjs[i].GetComponent<BlackScreenController>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.GetComponent<PlayerController>() != null)
        if(other.tag == "Player Head")
        {
            Debug.Log(this.name);
            for (int i = 0; i < bscs.Length; i++)
            {
                if (this == bscs[i])
                {
                    sr.enabled = false;
                }
                else
                {
                    bscs[i].sr.enabled = true;
                }
            }
        }
        if (textBoxCanvas != null)
        {
            textBoxCanvas.enabled = false;
        }
    }

}
