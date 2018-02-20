using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGUIController : MonoBehaviour {

    public string name;

	public void EnableGUI()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        for(int i = 0; i < srs.Length; i++)
        {
            if(srs[i].GetComponent<SelectionIconHookpoint>() == null)
            {
                srs[i].enabled = true;
            }
            if(srs[i].GetComponent<SelectionIconHookpoint>() != null)
            {
                if(srs[i].GetComponent<SelectionIconHookpoint>().active == false)
                srs[i].GetComponent<SelectionIconHookpoint>().parentSR.enabled = false;
            }
        }
    }

    public void DisableGUI()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < srs.Length; i++)
        {
            srs[i].enabled = false;
        }
    }
}
