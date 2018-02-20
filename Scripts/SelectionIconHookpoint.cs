using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIconHookpoint : MonoBehaviour {

    public string hookpointName;
    public int hookpointIndex;
    public bool active;
    public bool selected;
    public Transform hookpointTrans;
    public SpriteRenderer sr;
    public SpriteRenderer parentSR;

    void Awake()
    {
        hookpointTrans = this.transform;
        sr = this.GetComponent<SpriteRenderer>();
        parentSR = hookpointTrans.parent.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sr.enabled = selected;
        if(SceneController.instance.currentSceneState == SceneController.SceneState.examining)
        parentSR.enabled = active;
    }
}
