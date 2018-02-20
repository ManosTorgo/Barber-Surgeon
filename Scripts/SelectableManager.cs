using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class SelectableManager : MonoBehaviour
{
    public List<SelectableController> selectables;
    public Selectable selectable;

    void Awake()
    {
        selectable.Select();
    }
}
