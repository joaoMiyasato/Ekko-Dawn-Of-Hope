using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public class SyntesisButton : MonoBehaviour,ISelectHandler
{
    public int buttonID;
    public ItemObject thisItem;
    public Tooltip tooltip;
    private Vector2 position;
    public GameObject tooltipObj;
    public Scrolls snap;

    private ItemObject GetThisItem()
    {
        for(int i = 0; i < PlayerManager.instance.inventoryManager.syntesis.Container.Count; i++)
        {
            if(buttonID == i)
            {
                thisItem = PlayerManager.instance.inventoryManager.syntesis.Container[i].item;
            }
        }

        return thisItem;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        snap.SnapTo(this.GetComponent<RectTransform>());
    }

    private string GetDetailText(ItemObject _item)
    {
        if(_item == null)
        {
            return "";
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<b><color=black><size=40>{0}</size></color></b>\n\n", _item.Name);
            stringBuilder.AppendFormat("<color=black><size=30>{0}</size></color>",_item.description);

            return stringBuilder.ToString();
        }
    }

    public void ShowItem() 
    {
        GetThisItem();

        if(thisItem != null)
        {
            if(thisItem != UI_manager.instance.previousItem)
            {
                tooltip.ShowTooltip();
                tooltip.UpdateTooltip(GetDetailText(thisItem), thisItem.sprite);
            }
            else
            {
                if(tooltipObj.activeSelf == true)
                {
                    tooltip.HideTooltip();
                    tooltip.UpdateTooltip("",null);
                }
                else
                {
                    tooltip.ShowTooltip();
                    tooltip.UpdateTooltip(GetDetailText(thisItem), thisItem.sprite);
                }
            }
        }
        else if(thisItem == null)
        {
            tooltip.HideTooltip();
            tooltip.UpdateTooltip("",null);
        }

        if(thisItem != null)
        {
            UI_manager.instance.previousItem = thisItem;
        }
    }
}
