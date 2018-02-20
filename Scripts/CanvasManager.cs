using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager instance;
    public Canvas activeCanvas;
    public Image darkenScreen;
    public bool activeCanvasDarkens;
    public string activeCanvasName;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(activeCanvas != null)
        {
            if(activeCanvas.enabled == true)
            {
                darkenScreen.enabled = activeCanvasDarkens;
            }
            else
            {
                darkenScreen.enabled = false;
            }
        }
    }
}
