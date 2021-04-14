using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_UI_button : MonoBehaviour
{
    private GameObject stones;

    private void Start()
    {
        stones = UI_manager.instance.Menu1.transform.Find("subMenus").gameObject;
    }
    public void nextPage()
    {
        UI_manager.instance.curMenuShow.SetActive(false);
        UI_manager.instance.curMenu += 1;

        UI_manager.instance.InterfaceUpdate();
    }

    public void previousPage()
    {
        UI_manager.instance.curMenuShow.SetActive(false);
        UI_manager.instance.curMenu -= 1;
        
        UI_manager.instance.InterfaceUpdate();
    }

    #region Stones
    public void activePowerStone()
    {
        UI_manager.instance.curStoneMenu.SetActive(false);

        UI_manager.instance.stonesMenu = 1;
        UI_manager.instance.stonesInterfaceUpdate();
    }
    public void activeControlStone()
    {
        UI_manager.instance.curStoneMenu.SetActive(false);

        UI_manager.instance.stonesMenu = 2;
        UI_manager.instance.stonesInterfaceUpdate();
    }
    public void activeHealStone()
    {
        UI_manager.instance.curStoneMenu.SetActive(false);

        UI_manager.instance.stonesMenu = 0;
        UI_manager.instance.stonesInterfaceUpdate();
    }
    public void activeObscureStone()
    {
        UI_manager.instance.curStoneMenu.SetActive(false);

        UI_manager.instance.stonesMenu = 3;
        UI_manager.instance.stonesInterfaceUpdate();
    }
    public void activeSyntesisStone()
    {
        UI_manager.instance.curStoneMenu.SetActive(false);

        UI_manager.instance.stonesMenu = 4;
        UI_manager.instance.stonesInterfaceUpdate();
    }
    #endregion
}
