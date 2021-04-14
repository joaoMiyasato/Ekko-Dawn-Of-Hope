using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public class scr_memory_button : MonoBehaviour, ISelectHandler
{
    public int buttonID;
    public ItemObject thisMemory;
    public scr_tooltip tooltip;
    private Vector2 position;
    public Scrolls snap;

    private ItemObject GetThisMemory()
    {
        for(int i = 0; i < scr_player_manager.instance.inventoryMemory.Container.Count; i++)
        {
            if(buttonID == i)
            {
                thisMemory = scr_player_manager.instance.inventoryMemory.Container[i].item;
            }
        }

        return thisMemory;
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowMemory();
        snap.SnapTo(this.GetComponent<RectTransform>());
    }

    private string GetDetailText(ItemObject _memory)
    {
        if(_memory == null)
        {
            return "";
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<b><color=black><size=40>{0}</size></color></b>\n\n", _memory.Name);
            stringBuilder.AppendFormat("<color=black><size=30>{0}</size></color>",_memory.description);

            return stringBuilder.ToString();
        }
    }

    public void ShowMemory() 
    {
        GetThisMemory();

        if(thisMemory != null)
        {
            tooltip.ShowTooltip();
            tooltip.UpdateTooltip(GetDetailText(thisMemory), thisMemory.sprite);
        }
        else if(thisMemory == null)
        {
            tooltip.HideTooltip();
            tooltip.UpdateTooltip("",null);
        }
    }
}
