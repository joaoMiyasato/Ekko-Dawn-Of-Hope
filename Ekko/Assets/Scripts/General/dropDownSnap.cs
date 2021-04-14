using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class dropDownSnap : MonoBehaviour,ISelectHandler, IDeselectHandler
{
    private bool este;
    public Scrolls snap;
    private void Update()
    {
        if(este)
            snap.SnapTo(this.GetComponent<RectTransform>());
    }
    public void OnSelect(BaseEventData eventData)
    {
        este = true;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        este = false;
    }
}