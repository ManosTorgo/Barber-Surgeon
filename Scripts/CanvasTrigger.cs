using UnityEngine;
using System.Collections;

public class CanvasTrigger : MonoBehaviour {

    private PlayerController player;
    public Canvas canvas;
    public string canvasName;
    public bool darkensScreen;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            player = other.gameObject.GetComponent<PlayerController>();
            CanvasManager.instance.activeCanvas = canvas;
            CanvasManager.instance.activeCanvasName = canvasName;
            CanvasManager.instance.activeCanvasDarkens = darkensScreen;
            player.canvasTriggerCol = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            player = other.gameObject.GetComponent<PlayerController>();
            CanvasManager.instance.activeCanvas = null;
            CanvasManager.instance.activeCanvasName = null;
            player.canvasTriggerCol = false;
        }
    }
}
