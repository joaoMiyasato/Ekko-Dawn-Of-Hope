using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownCheck : MonoBehaviour
{
    public GameObject menu;

    private void Start()
    {
        if(this.name == "Dropdown List")
        {
            menu.GetComponent<MainMenu>().open = true;
        }
    }

    private void OnDestroy()
    {
        menu.GetComponent<MainMenu>().open = false;
    }
}
