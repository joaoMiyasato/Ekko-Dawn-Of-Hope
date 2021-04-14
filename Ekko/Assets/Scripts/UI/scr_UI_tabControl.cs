using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_UI_tabControl : MonoBehaviour
{
    private Navigation R;
    private Button RBD;
    private Button RBL;
    private Navigation L;
    private Button LBD;
    private Button LBR;

    void Start()
    {
        L.mode = Navigation.Mode.Explicit;
        R.mode = Navigation.Mode.Explicit;

        LBR = UI_manager.instance.Tab.transform.Find("switch (0)").gameObject.transform.Find("sRight").gameObject.GetComponent<Button>();
        RBL = UI_manager.instance.Tab.transform.Find("switch (1)").gameObject.transform.Find("sLeft").gameObject.GetComponent<Button>();
    }

    void Update()
    {
        if(UI_manager.instance.curMenu == 0)
        {
            RBD = UI_manager.instance.Menu0.transform.Find("Inventário").gameObject.transform.Find("SlotLayout").gameObject.transform.Find("Slots").gameObject.transform.Find("Slot (0)").gameObject.GetComponent<Button>();
            LBD = UI_manager.instance.Menu0.transform.Find("Inventário").gameObject.transform.Find("SlotLayout").gameObject.transform.Find("Slots").gameObject.transform.Find("Slot (0)").gameObject.GetComponent<Button>();
        }
        else if(UI_manager.instance.curMenu == 1)
        {
            if(UI_manager.instance.isSubMenu)
            {
                RBD = UI_manager.instance.Menu1.transform.Find("subMenus").gameObject.transform.Find("Cura").gameObject.GetComponent<Button>();
                LBD = UI_manager.instance.Menu1.transform.Find("subMenus").gameObject.transform.Find("Cura").gameObject.GetComponent<Button>();
            }
            else
            {
                RBD = UI_manager.instance.Menu1.transform.Find("Pedras").gameObject.transform.Find("Roxo (3)").gameObject.GetComponent<Button>();
                LBD = UI_manager.instance.Menu1.transform.Find("Pedras").gameObject.transform.Find("Verde (0)").gameObject.GetComponent<Button>();
            }
        }
        else if(UI_manager.instance.curMenu == 2)
        {
            RBD = null;
            LBD = null;
        }
        else if(UI_manager.instance.curMenu == 3)
        {
            RBD = UI_manager.instance.GlossaryFirstButton.GetComponent<Button>();
            LBD = UI_manager.instance.GlossaryFirstButton.GetComponent<Button>();
        }

        L.selectOnRight = LBR;
        L.selectOnDown = LBD;
        R.selectOnLeft = RBL;
        R.selectOnDown = RBD;

        UI_manager.instance.Tab.transform.Find("switch (0)").gameObject.transform.Find("sRight").gameObject.GetComponent<Button>().navigation = R;
        UI_manager.instance.Tab.transform.Find("switch (1)").gameObject.transform.Find("sLeft").gameObject.GetComponent<Button>().navigation = L;
    }
    
}
