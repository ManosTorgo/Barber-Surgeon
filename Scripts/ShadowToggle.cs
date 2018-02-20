using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowToggle : MonoBehaviour {

    public SpriteRenderer sr;
    public SpriteRenderer parentSR;

    void Awake()
    {
        sr = this.GetComponent<SpriteRenderer>();
        parentSR = transform.parent.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        sr.enabled = parentSR.enabled;
    }
}
