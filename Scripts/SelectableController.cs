using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectableController : MonoBehaviour, ISelectHandler, IDeselectHandler {

    public Selectable selectable;
    public Color newColour;
    private Color defaultColour;
    public bool isSelected;
    public int listIndex;
    public bool isActive;

    public void Awake()
    {
        if (this.GetComponent<InventoryItem>() != null)
        selectable = this.GetComponent<Selectable>();
    }

    void Update()
    {

    }

	public void OnSelect(BaseEventData eventData)
    {
            isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
            isSelected = false;
    }
}
