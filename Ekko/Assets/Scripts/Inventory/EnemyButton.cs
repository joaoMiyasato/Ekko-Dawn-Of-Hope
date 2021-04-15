using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public class EnemyButton : MonoBehaviour, ISelectHandler
{
    public int buttonID;
    public ItemObject thisEnemy;
    public Tooltip tooltip;
    private Vector2 position;
    public Scrolls snap;

    private ItemObject GetThisEnemy()
    {
        for(int i = 0; i < PlayerManager.instance.inventoryEnemy.Container.Count; i++)
        {
            if(buttonID == i)
            {
                thisEnemy = PlayerManager.instance.inventoryEnemy.Container[i].item;
            }
        }

        return thisEnemy;
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowEnemy();
        snap.SnapTo(this.GetComponent<RectTransform>());
    }

    private string GetDetailText(ItemObject _enemy)
    {
        if(_enemy == null)
        {
            return "";
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<b><color=black><size=40>{0}</size></color></b>\n\n", _enemy.Name);
            stringBuilder.AppendFormat("<color=black><size=30>{0}</size></color>",_enemy.description);

            return stringBuilder.ToString();
        }
    }

    public void ShowEnemy() 
    {
        GetThisEnemy();

        if(thisEnemy != null)
        {
            tooltip.ShowTooltip();
            tooltip.UpdateTooltip(GetDetailText(thisEnemy), thisEnemy.sprite);
        }
        else if(thisEnemy == null)
        {
            tooltip.HideTooltip();
            tooltip.UpdateTooltip("",null);
        }
    }
}
