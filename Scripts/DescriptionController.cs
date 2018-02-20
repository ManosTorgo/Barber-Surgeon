using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DescriptionController : MonoBehaviour {

    public static DescriptionController instance;
    public Text text;
    public Canvas canvas;

    void Awake()
    {
        instance = this;
        text = this.GetComponentInChildren<Text>();
        canvas = this.GetComponent<Canvas>();
    }
}
