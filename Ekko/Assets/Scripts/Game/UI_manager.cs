using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_manager : MonoBehaviour
{
    public static UI_manager instance;
    public GameObject UI;
    public Animator anim;
    public int curMenu;
    public GameObject curMenuShow;
    public GameObject bagMenu;
    public GameObject Menu0;
    public GameObject Menu1;
    public GameObject Menu2;
    public GameObject Menu3;
    public GameObject Tab;

    public GameObject pauseMenu;

    public GameObject playerInterface;
    public GameObject player;


    public Scrollbar glossaryScroll;
    public Scrollbar memoriesScroll;
    public Scrollbar syntesisScroll;

    public bool isSubMenu = false;
    private GameObject subMenu;
    public int stonesMenu;
    public GameObject curStoneMenu;
    
    public GameObject InventoryFirstButton, RockFirstButton, MapFirstButton, GlossaryFirstButton, PauseFirstButton;

    public ItemObject previousItem;

    public PlayerManager playerSave;

    
    public GameObject[] slots;
    public JewelButton thisButton;
    public JewelButton[] itemButtons;
    public GameObject[] enemiesSlots;
    public GameObject[] memoriesSlots;
    public GameObject[] syntesisSlots;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        UI.SetActive(true);
        anim = UI.GetComponent<Animator>();
        curMenu = 0;
        stonesMenu = 0;

        InterfaceUpdate();
        bagMenu.gameObject.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");
        subMenu = Menu1.transform.Find("subMenus").gameObject;

        playerSave = player.GetComponent<PlayerManager>();
        
        DisplaySyntesis();
        DisplayItems();
        DisplayEnemies();
        DisplayMemories();
    }
    void Update()
    {
        interfaceControl();
        playerInterfaceControl();
        PauseControl();
        BagControl();
        
        DisplaySyntesis();
        DisplayItems();
        DisplayEnemies();
        DisplayMemories();
    }

#region Interface
    private void interfaceControl()
    {
        if(curMenu != 1)
        {
            Menu1.transform.Find("Pedras").gameObject.SetActive(true);
            subMenu.SetActive(false);
            isSubMenu = false;
        }
        else if (curMenu == 1)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Menu1.transform.Find("Pedras").gameObject.SetActive(true);
                subMenu.SetActive(false);
                isSubMenu = false;
                
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(RockFirstButton);
            }
        }
    }

    public void InterfaceUpdate()
    {
        if(curMenu > 3)
        {
            curMenu = 0;
        }
        else if(curMenu < 0)
        {
            curMenu = 3;
        }

        if(curMenu == 0)
        {
            curMenuShow = Menu0;
            curMenuShow.SetActive(true);
            Tab.GetComponent<Text>().text = "Inventário";

            if(PlayerManager.instance.getHasLantern() == true)
            {
                Menu0.transform.Find("Lantern").gameObject.transform.Find("Text").gameObject.SetActive(true);
                if(PlayerManager.instance.getHasLantern() == false)
                {
                    Menu0.transform.Find("Lantern").gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = "Descarregado";
                }
                else
                {
                    Menu0.transform.Find("Lantern").gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = "Carregado";
                }
            }
        }
        else if(curMenu == 1)
        {
            curMenuShow = Menu1;
            curMenuShow.SetActive(true);
            Tab.GetComponent<Text>().text = "Pedra Filosofal";
        }
        else if(curMenu == 2)
        {
            curMenuShow = Menu2;
            curMenuShow.SetActive(true);
            Tab.GetComponent<Text>().text = "Mapa";
        }
        else if(curMenu == 3)
        {
            curMenuShow = Menu3;
            curMenuShow.SetActive(true);
            Tab.GetComponent<Text>().text = "Glossário";
        }
    }

    public void stonesInterfaceUpdate()
    {
        subMenu.SetActive(true);
        isSubMenu = true;
        Menu1.transform.Find("Pedras").gameObject.SetActive(false);
        if(stonesMenu == 0)
        {
            curStoneMenu = subMenu.transform.Find("MenuCura").gameObject;
            curStoneMenu.SetActive(true);
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(subMenu.transform.Find("Cura").gameObject);
        }
        else if(stonesMenu == 1)
        {
            curStoneMenu = subMenu.transform.Find("MenuPoder").gameObject;
            curStoneMenu.SetActive(true);
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(subMenu.transform.Find("Poder").gameObject);
        }
        else if(stonesMenu == 2)
        {
            curStoneMenu = subMenu.transform.Find("MenuControle").gameObject;
            curStoneMenu.SetActive(true);
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(subMenu.transform.Find("Controle").gameObject);
        }
        else if(stonesMenu == 3)
        {
            curStoneMenu = subMenu.transform.Find("MenuObscuro").gameObject;
            curStoneMenu.SetActive(true);
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(subMenu.transform.Find("Obscura").gameObject);
        }
        else if(stonesMenu == 4)
        {
            curStoneMenu = subMenu.transform.Find("MenuSintese").gameObject;
            curStoneMenu.SetActive(true);
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(subMenu.transform.Find("Sintese").gameObject);
        }
    }

    private void playerInterfaceControl()
    {
        playerInterface.transform.Find("Health").gameObject.GetComponent<UnityEngine.UI.Text>().text = "Life: "+PlayerManager.instance.playerBase.getCurrentLife();
        playerInterface.transform.Find("soulBar").gameObject.GetComponent<Slider>().maxValue = PlayerManager.instance.playerBase.getMaxEnergy();
        playerInterface.transform.Find("soulBar").gameObject.GetComponent<Slider>().value = PlayerManager.instance.playerBase.getCurrentEnergy();
    }
#endregion

#region Pause
    private void PauseControl()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(GameManager.instance.isPaused)
            {
                PauseResume();
                BagResume();
            }
            else
            {
                PausePause();

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(PauseFirstButton);
            }
        }
    }

    public void PauseResume()
    {
        pauseMenu.SetActive(false);
        GameManager.instance.Pause_Unpause(false);
    }

    public void PausePause()
    {
        pauseMenu.SetActive(true);
        GameManager.instance.Pause_Unpause(true);
    }
#endregion

#region Bag
    private void BagControl()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(GameManager.instance.isPaused)
            {
                PauseResume();
                BagResume();
            }
            else
            {
                BagPause();
            }
        }
    }

    public void BagResume()
    {
        bagMenu.gameObject.SetActive(false);
        GameManager.instance.Pause_Unpause(false);
        
        subMenu.SetActive(false);
        isSubMenu = false;
    }

    public void BagPause()
    {
        bagMenu.gameObject.SetActive(true);
        GameManager.instance.Pause_Unpause(true);
        InterfaceUpdate();

        if(curMenu == 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(InventoryFirstButton);
        }
        else if (curMenu == 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(RockFirstButton);
        }
        else if (curMenu == 2)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(MapFirstButton);
        }
        else if (curMenu == 3)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GlossaryFirstButton);
        }
    }
#endregion

    public void returnToMenu()
    {
        Save_Load.instance.saveConfig();
        Save_Load.instance.SavePlayer(Save_Load.instance.savePathP, Save_Load.instance.savePathI1, Save_Load.instance.savePathI2, Save_Load.instance.savePathI3, Save_Load.instance.savePathI4);
        SceneManager.LoadScene("MainMenu");
    }
    
    public void fadeTransition()
    {
        anim.SetTrigger("fade");
    }
    public void XroomTransition()
    {
        anim.SetTrigger("Xtransition");
    }
    public void YroomTransition()
    {
        anim.SetTrigger("Ytransition");
    }

    private void DisplayEnemies()
    {
        for(int i = 0; i < PlayerManager.instance.inventoryEnemy.Container.Count; i++)
        {
            enemiesSlots[i].GetComponent<Text>().color = new Color(1,1,1,1);
            enemiesSlots[i].GetComponent<Text>().text = PlayerManager.instance.inventoryEnemy.Container[i].item.Name;
        }
        for(int i = 0; i < enemiesSlots.Length; i++)
        {
            if(i < PlayerManager.instance.inventoryEnemy.Container.Count)
            {
                enemiesSlots[i].GetComponent<Text>().color = new Color(1,1,1,1);
                enemiesSlots[i].GetComponent<Text>().text = PlayerManager.instance.inventoryEnemy.Container[i].item.Name;
            }
            else
            {
                enemiesSlots[i].GetComponent<Text>().color = new Color(1,1,1,0);
                enemiesSlots[i].GetComponent<Text>().text = "";
            }
        }
    }
    private void DisplayMemories()
    {
        for(int i = 0; i < PlayerManager.instance.inventoryMemory.Container.Count; i++)
        {
            memoriesSlots[i].transform.GetChild(0).GetComponent<Text>().color = new Color(1,1,1,1);
            memoriesSlots[i].transform.GetChild(0).GetComponent<Text>().text = PlayerManager.instance.inventoryMemory.Container[i].item.Name;
        }
        for(int i = 0; i < memoriesSlots.Length; i++)
        {
            if(i < PlayerManager.instance.inventoryMemory.Container.Count)
            {
                memoriesSlots[i].transform.GetChild(0).GetComponent<Text>().color = new Color(1,1,1,1);
                memoriesSlots[i].transform.GetChild(0).GetComponent<Text>().text = PlayerManager.instance.inventoryMemory.Container[i].item.Name;
            }
            else
            {
                memoriesSlots[i].transform.GetChild(0).GetComponent<Text>().color = new Color(1,1,1,0);
                memoriesSlots[i].transform.GetChild(0).GetComponent<Text>().text = "";
            }
        }
    }
    private void DisplayItems()
    {
        for (int i = 0; i < PlayerManager.instance.inventoryJewel.Container.Count; i++)
        {
            slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = PlayerManager.instance.inventoryJewel.Container[i].item.sprite;

            if(PlayerManager.instance.inventoryJewel.Container[i].amount > 1)
            {
                slots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(255,244,0,1);
            }
            else
            {
                slots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(255,244,0,0);
            }
            slots[i].transform.GetChild(1).GetComponent<Text>().text = PlayerManager.instance.inventoryJewel.Container[i].amount.ToString();

            slots[i].transform.GetChild(2).gameObject.SetActive(false);
        }
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < PlayerManager.instance.inventoryJewel.Container.Count)
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = PlayerManager.instance.inventoryJewel.Container[i].item.sprite;
                
                if(PlayerManager.instance.inventoryJewel.Container[i].amount > 1)
                {
                    slots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(255,244,0,1);
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(255,244,0,0);
                }
                slots[i].transform.GetChild(1).GetComponent<Text>().text = PlayerManager.instance.inventoryJewel.Container[i].amount.ToString();
                
                slots[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,0);
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                
                slots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(255,244,0,0);
                slots[i].transform.GetChild(1).GetComponent<Text>().text = null;
                
                slots[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }
    public void DisplaySyntesis()
    {
        for(int i = 0; i < PlayerManager.instance.inventorySyntesis.Container.Count; i++)
        {
            syntesisSlots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
            syntesisSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = PlayerManager.instance.inventorySyntesis.Container[i].item.sprite;

            if(PlayerManager.instance.inventorySyntesis.Container[i].amount > 1)
            {
                syntesisSlots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(204,153,0,1);
            }
            else
            {
                syntesisSlots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(0,0,0,0);
            }
            syntesisSlots[i].transform.GetChild(1).GetComponent<Text>().text = PlayerManager.instance.inventorySyntesis.Container[i].amount.ToString();
        }
        for(int i = 0; i < syntesisSlots.Length; i++)
        {
            if(i < PlayerManager.instance.inventorySyntesis.Container.Count)
            {
                syntesisSlots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
                syntesisSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = PlayerManager.instance.inventorySyntesis.Container[i].item.sprite;
                
                if(PlayerManager.instance.inventorySyntesis.Container[i].amount > 1)
                {
                    syntesisSlots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(204,153,0,1);
                }
                else
                {
                    syntesisSlots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(0,0,0,0);
                }
                syntesisSlots[i].transform.GetChild(1).GetComponent<Text>().text = PlayerManager.instance.inventorySyntesis.Container[i].amount.ToString();
            }
            else
            {
                syntesisSlots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,0);
                syntesisSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                
                syntesisSlots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(0,0,0,0);
                syntesisSlots[i].transform.GetChild(1).GetComponent<Text>().text = null;
            }
        }
    }

    private void ResetButtonItems()
    {
        for(int i = 0; i < itemButtons.Length; i++)
        {
            if(i < PlayerManager.instance.inventoryJewel.Container.Count)
            {
                itemButtons[i].thisItem = PlayerManager.instance.inventoryJewel.Container[i].item;
            }
            else
            {
                itemButtons[i].thisItem = null;
            }
        }
    }
}
