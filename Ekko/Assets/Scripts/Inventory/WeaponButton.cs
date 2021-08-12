using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponButton : MonoBehaviour
{
    public WeaponObject thisWeapon;
    void Start()
    {
        this.gameObject.GetComponent<Image>().sprite = thisWeapon.sprite;
    }

    public void selectWeapon()
    {
        if(UI_manager.instance.selectedWeaponSlot.GetComponent<EquipedWeaponButton>().off && !UI_manager.instance.selectedWeaponSlot.GetComponent<EquipedWeaponButton>().supp)
        {
            PlayerManager.instance.setOffEquipedWeapon(thisWeapon);
        }
        else if(UI_manager.instance.selectedWeaponSlot.GetComponent<EquipedWeaponButton>().off && UI_manager.instance.selectedWeaponSlot.GetComponent<EquipedWeaponButton>().supp)
        {
            PlayerManager.instance.setOffSuppEquipedWeapon(thisWeapon);
        }
        else if(!UI_manager.instance.selectedWeaponSlot.GetComponent<EquipedWeaponButton>().off && UI_manager.instance.selectedWeaponSlot.GetComponent<EquipedWeaponButton>().supp)
        {
            PlayerManager.instance.setSuppEquipedWeapon(thisWeapon);
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(UI_manager.instance.selectedWeaponSlot);
        UI_manager.instance.selectedWeaponSlot = null;
    }
}
