using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DarkScreenController : MonoBehaviour {

    public Image screen;
    public bool playerCol;

    void Awake()
    {
        screen = this.GetComponent<Image>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            screen.enabled = false;
            playerCol = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            screen.enabled = true;
            playerCol = false;
        }
    }
}
