using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour {

    public List<string> dialogue = new List<string>();
    public int index;
    public static DialogueController instance;

    void Awake()
    {
        instance = this;
        index = 0;
    }
}
