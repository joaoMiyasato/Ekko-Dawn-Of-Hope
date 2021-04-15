using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SubMenus : MonoBehaviour
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
        powerMenu.transform.Find("Layout").gameObject.transform.Find("powerPoints").gameObject.transform.Find("Qtd").gameObject.GetComponent<Text>().text = PlayerManager.instance.PowerPoints.ToString();
        powerMenu.transform.Find("Layout").gameObject.transform.Find("energyStones").gameObject.transform.Find("Qtd").gameObject.GetComponent<Text>().text = PlayerManager.instance.EnergyStones.ToString();
        healMenu.transform.Find("Layout").gameObject.transform.Find("powerPoints").gameObject.transform.Find("Qtd").gameObject.GetComponent<Text>().text = PlayerManager.instance.PowerPoints.ToString();
    }

#region HealStone
    private void healInterface()
    {
        healMenu.transform.Find("Layout").gameObject.transform.Find("Health").gameObject.GetComponent<Slider>().value = PlayerManager.instance.maxLife;
        healMenu.transform.Find("Layout").gameObject.transform.Find("Energy").gameObject.GetComponent<Slider>().value = PlayerManager.instance.maxEnergy;

        healMenu.transform.Find("Layout").gameObject.transform.Find("Health").gameObject.transform.Find("qtd").gameObject.GetComponent<Text>().text = PlayerManager.instance.maxLife.ToString();
        healMenu.transform.Find("Layout").gameObject.transform.Find("Energy").gameObject.transform.Find("qtd").gameObject.GetComponent<Text>().text = PlayerManager.instance.maxEnergy.ToString();

        healthCost = (PlayerManager.instance.maxLife)+(500);
        energyCost = ((PlayerManager.instance.maxEnergy/100) * 120)+(500);

        healMenu.transform.Find("Layout").gameObject.transform.Find("upgradeHealth").gameObject.transform.Find("healthText").gameObject.GetComponent<Text>().text = healthCost.ToString();
        healMenu.transform.Find("Layout").gameObject.transform.Find("upgradeEnergy").gameObject.transform.Find("energyText").gameObject.GetComponent<Text>().text = energyCost.ToString();
    }
    public void increaseHealth()
    {
        if(PlayerManager.instance.PowerPoints >= healthCost)
        {
            PlayerManager.instance.PowerPoints -= healthCost;
            PlayerManager.instance.maxLife+= 100;
        }
    }
    public void increaseEnergy()
    {
        if(PlayerManager.instance.PowerPoints >= energyCost)
        {
            PlayerManager.instance.PowerPoints -= energyCost;
            PlayerManager.instance.maxEnergy+=100;
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
            PlayerManager.instance.PowerPoints += qtd1;
            qtd1 = 0;
        }
    }
    public void PowerPointsIncrease()
    {
        if(PlayerManager.instance.PowerPoints >= 1000)
        {
            qtd1 += 1000;
            PlayerManager.instance.PowerPoints -= 1000;
        }
    }
    public void PowerPointsDecrease()
    {
        if(qtd1 >= 1000)
        {
            qtd1 -= 1000;
            PlayerManager.instance.PowerPoints += 1000;
        }
    }
    public void Converter()
    {
        PlayerManager.instance.EnergyStones += qtd2;
        qtd1 = 0;
    }
#endregion

}
