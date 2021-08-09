using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipedWeaponButton : MonoBehaviour
{
    public ItemObject thisWeapon;
    public GameObject weaponSelect;
    public bool off, supp;
    void Start()
    {
        if(off && !supp)
        {
            thisWeapon = PlayerManager.instance.getOffEquipedWeapon();
        }
        else if(off && supp)
        {
            thisWeapon = PlayerManager.instance.getOffSuppEquipedWeapon();
        }
        else if(!off && supp)
        {
            thisWeapon = PlayerManager.instance.getSuppEquipedWeapon();
        }
    }
    void Update()
    {
        if(off && !supp)
        {
            thisWeapon = PlayerManager.instance.getOffEquipedWeapon();
        }
        else if(off && supp)
        {
            thisWeapon = PlayerManager.instance.getOffSuppEquipedWeapon();
        }
        else if(!off && supp)
        {
            thisWeapon = PlayerManager.instance.getSuppEquipedWeapon();
        }
        if(thisWeapon != null)
        {
            this.transform.Find("spriteW").GetComponent<Image>().sprite = thisWeapon.sprite;

            this.gameObject.transform.Find("spriteW").gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.transform.Find("spriteW").gameObject.SetActive(false);
        }
    }

    public void selectWeaponSlot()
    {
        UI_manager.instance.selectedWeaponSlot = this.gameObject;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(UI_manager.instance.Menu4.transform.Find("Weapons").transform.Find("Slots").transform.Find("WeaponSlot (0)").gameObject);
    }
}
