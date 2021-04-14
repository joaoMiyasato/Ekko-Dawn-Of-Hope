using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Scrolls : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 pos1, pos2;

        pos1.x = scrollRect.transform.InverseTransformPoint(contentPanel.position).x;
        pos1.y = scrollRect.transform.InverseTransformPoint(contentPanel.position).y;

        pos2.x = scrollRect.transform.InverseTransformPoint(contentPanel.position).x;
        pos2.y = scrollRect.transform.InverseTransformPoint(target.position).y;
        
        if(!GameManager.instance.dragging)
            contentPanel.anchoredPosition = pos1 - pos2;
    }
}
