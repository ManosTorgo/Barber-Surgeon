using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGUITrigger : MonoBehaviour {

    public PlayerController player;
    public SpriteGUIController GUI;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerController>()!= null)
        {
            player = other.GetComponent<PlayerController>();
            SceneController.instance.activeGUIController = GUI;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            player = other.GetComponent<PlayerController>();
            SceneController.instance.activeGUIController = null;
        }
    }
}
