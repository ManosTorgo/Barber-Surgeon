using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextBoxManager : MonoBehaviour {

    public static TextBoxManager instance;
    public string text;
    public BlackScreenController[] blackScreens;
    public Canvas[] canvases;
    public Text firstFloorText;
    public Text groundFloorText;
    public int dialogueIndex;
    public List<string> dialogue = new List<string>();
    public bool inDialogue;
    public float eLock;

    void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].enabled == true)
            {
                SceneController.instance.SetSceneState(SceneController.SceneState.inText);
                if (Input.GetButtonDown("Fire1") == true && eLock > Time.deltaTime)
                {
                    dialogueIndex++;
                }
                if (dialogueIndex >= dialogue.Count)
                {
                    for (int i2 = 0; i2 < canvases.Length; i2++)
                    {
                        canvases[i2].enabled = false;
                        dialogueIndex = 0;
                        dialogue.Clear();
                        inDialogue = false;
                        SceneController.instance.SetSceneState(SceneController.SceneState.sceneActive);
                    }
                }
                else if(dialogueIndex < dialogue.Count)
                {
                    text = dialogue[dialogueIndex];
                }
                eLock += Time.deltaTime;
            }
        }
        firstFloorText.text = text;
        groundFloorText.text = text;
    }

    public void ActuatingDialogue()
    {
        if (dialogue.Count != 0)
        {
            for (int i = 0; i < blackScreens.Length; i++)
            {
                if (blackScreens[i].sr.enabled == true)
                {
                    inDialogue = true;
                    canvases[i].enabled = true;
                }
            }
        }
    }
}
