using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    public SelectionIconHookpoint[] allSelections;
    public List<SelectionIconHookpoint> activeSelections = new List<SelectionIconHookpoint>();
    public int currentSelectionIndex = 0;
    public float timedelay;

    void Awake()
    {
        AllocateActiveSelections();
    }

    void Update()
    {
        if (SceneController.instance.currentSceneState == SceneController.SceneState.examining)
        {
            for (int i = 0; i < activeSelections.Count; i++)
            {
                if (i == currentSelectionIndex)
                {
                    activeSelections[i].selected = true;
                }
                else
                {
                    activeSelections[i].selected = false;
                }
            }

            if (Input.GetAxis("Horizontal") == 1 && timedelay < Time.time)
            {
                currentSelectionIndex++;
                timedelay = Time.time + 0.15f;
            }
            if (Input.GetAxis("Horizontal") == -1 && timedelay < Time.time)
            {
                currentSelectionIndex--;
                timedelay = Time.time + 0.15f;
            }
            if (currentSelectionIndex >= activeSelections.Count)
            {
                currentSelectionIndex = 0;
            }
            if (currentSelectionIndex == -1)
            {
                currentSelectionIndex = activeSelections.Count - 1;
            }

            DescriptionController.instance.text.text = activeSelections[currentSelectionIndex].hookpointName;

            if (Input.GetButtonDown("Fire1") == true)
            {
                activeSelections[currentSelectionIndex].active = false;
                activeSelections[currentSelectionIndex].selected = false;
                AllocateActiveSelections();
            }
        }
    }

    public void AllocateActiveSelections()
    {
        activeSelections.Clear();

        for(int i = 0; i < allSelections.Length; i++)
        {
            if (allSelections[i].active == true)
            {
                activeSelections.Add(allSelections[i]);
            }
        }

        activeSelections.Sort((x, y) => x.hookpointIndex.CompareTo(y.hookpointIndex));
    }

}
