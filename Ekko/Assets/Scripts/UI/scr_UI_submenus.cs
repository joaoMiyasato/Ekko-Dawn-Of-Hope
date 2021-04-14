using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_UI_submenus : MonoBehaviour
{
    private GameObject healMenu;
    private GameObject powerMenu;
    private GameObject controlMenu;
    private GameObject obscureMenu;
    private GameObject syntesisMenu;

    private int healthCost;
    private int energyCost;

////////////////////////////////////////////////////////////////

    private int qtd1 = 0, qtd2 = 0;

////////////////////////////////////////////////////////////////

    private void Start()
    {
        healMenu = UI_manager.instance.Menu1.transform.Find("subMenus").gameObject.transform.Find("MenuCura").gameObject;
        powerMenu = UI_manager.instance.Menu1.transform.Find("subMenus").gameObject.transform.Find("MenuPoder").gameObject;
        controlMenu = UI_manager.instance.Menu1.transform.Find("subMenus").gameObject.transform.Find("MenuControle").gameObject;
        obscureMenu = UI_manager.instance.Menu1.transform.Find("subMenus").gameObject.transform.Find("MenuObscuro").gameObject;
        syntesisMenu = UI_manager.instance.Menu1.transform.Find("subMenus").gameObject.transform.Find("MenuSintese").gameObject;
    }
    private void Update()
    {
        healInterface();
        PowerPointsInterface();
        powerInterface();
    }

    private void PowerPointsInterface()
    {
        powerMenu.transform.Find("Layout").gameObject.transform.Find("powerPoints").gameObject.transform.Find("Qtd").gameObject.GetComponent<Text>().text = scr_player_manager.instance.PowerPoints.ToString();
        powerMenu.transform.Find("Layout").gameObject.transform.Find("energyStones").gameObject.transform.Find("Qtd").gameObject.GetComponent<Text>().text = scr_player_manager.instance.EnergyStones.ToString();
        healMenu.transform.Find("Layout").gameObject.transform.Find("powerPoints").gameObject.transform.Find("Qtd").gameObject.GetComponent<Text>().text = scr_player_manager.instance.PowerPoints.ToString();
    }

#region HealStone
    private void healInterface()
    {
        healMenu.transform.Find("Layout").gameObject.transform.Find("Health").gameObject.GetComponent<Slider>().value = scr_player_manager.instance.maxLife;
        healMenu.transform.Find("Layout").gameObject.transform.Find("Energy").gameObject.GetComponent<Slider>().value = scr_player_manager.instance.maxEnergy;

        healMenu.transform.Find("Layout").gameObject.transform.Find("Health").gameObject.transform.Find("qtd").gameObject.GetComponent<Text>().text = scr_player_manager.instance.maxLife.ToString();
        healMenu.transform.Find("Layout").gameObject.transform.Find("Energy").gameObject.transform.Find("qtd").gameObject.GetComponent<Text>().text = scr_player_manager.instance.maxEnergy.ToString();

        healthCost = (scr_player_manager.instance.maxLife)+(500);
        energyCost = ((scr_player_manager.instance.maxEnergy/100) * 120)+(500);

        healMenu.transform.Find("Layout").gameObject.transform.Find("upgradeHealth").gameObject.transform.Find("healthText").gameObject.GetComponent<Text>().text = healthCost.ToString();
        healMenu.transform.Find("Layout").gameObject.transform.Find("upgradeEnergy").gameObject.transform.Find("energyText").gameObject.GetComponent<Text>().text = energyCost.ToString();
    }
    public void increaseHealth()
    {
        if(scr_player_manager.instance.PowerPoints >= healthCost)
        {
            scr_player_manager.instance.PowerPoints -= healthCost;
            scr_player_manager.instance.maxLife+= 100;
        }
    }
    public void increaseEnergy()
    {
        if(scr_player_manager.instance.PowerPoints >= energyCost)
        {
            scr_player_manager.instance.PowerPoints -= energyCost;
            scr_player_manager.instance.maxEnergy+=100;
        }
    }
#endregion

#region PowerStone
    private void powerInterface()
    {
        powerMenu.transform.Find("Layout").gameObject.transform.Find("Conversor").gameObject.transform.Find("Qtd1").gameObject.GetComponent<Text>().text = qtd1.ToString();
        powerMenu.transform.Find("Layout").gameObject.transform.Find("Conversor").gameObject.transform.Find("Qtd2").gameObject.GetComponent<Text>().text = qtd2.ToString();

        qtd2 = qtd1/1000;
        if(UI_manager.instance.stonesMenu != 1)
        {
            scr_player_manager.instance.PowerPoints += qtd1;
            qtd1 = 0;
        }
    }
    public void PowerPointsIncrease()
    {
        if(scr_player_manager.instance.PowerPoints >= 1000)
        {
            qtd1 += 1000;
            scr_player_manager.instance.PowerPoints -= 1000;
        }
    }
    public void PowerPointsDecrease()
    {
        if(qtd1 >= 1000)
        {
            qtd1 -= 1000;
            scr_player_manager.instance.PowerPoints += 1000;
        }
    }
    public void Converter()
    {
        scr_player_manager.instance.EnergyStones += qtd2;
        qtd1 = 0;
    }
#endregion

}
