using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;

public class JewelButton : MonoBehaviour, ISelectHandler
{
    public int buttonID;
    public ItemObject thisItem;
    public Tooltip tooltip;
    private Vector2 position;

    private ItemObject GetThisItem()
    {
        for(int i = 0; i < PlayerManager.instance.inventoryJewel.Container.Count; i++)
        {
            if(buttonID == i)
            {
                thisItem = PlayerManager.instance.inventoryJewel.Container[i].item;
            }
        }

        return thisItem;
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowItem();
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
            tooltip.ShowTooltip();
            tooltip.UpdateTooltip(GetDetailText(thisItem), thisItem.sprite);
        }
        else if(thisItem == null)
        {
            tooltip.HideTooltip();
            tooltip.UpdateTooltip("",null);
        }
    }
}
