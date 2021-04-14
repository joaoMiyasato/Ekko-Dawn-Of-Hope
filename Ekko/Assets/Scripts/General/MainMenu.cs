using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject mainFirstButton, loadFirstButton, configFirstButton;
    private int nMenu;
    private bool config, load;
    public bool open;
    private void Start()
    {
        nMenu = 0;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Retornar();
        }
        if(GameManager.instance.dragging)
        {
            if(!open)
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                if(nMenu == 0)
                {
                    EventSystem.current.SetSelectedGameObject(mainFirstButton);
                }
                if(nMenu == 1)
                {
                    if(load)
                    {
                        EventSystem.current.SetSelectedGameObject(loadFirstButton);
                    }
                    else if(config)
                    {
                        EventSystem.current.SetSelectedGameObject(configFirstButton);
                    }
                }
            }
        }

        if(nMenu == 0)
        {
            transform.Find("Menu").gameObject.SetActive(true);
            transform.Find("Carregar").gameObject.SetActive(false);
            transform.Find("Config").gameObject.SetActive(false);
        }
        else if(nMenu == 1)
        {
            transform.Find("Menu").gameObject.SetActive(false);
            if(load)
            {
                transform.Find("Carregar").gameObject.SetActive(true);
                transform.Find("Config").gameObject.SetActive(false);
            }
            else if(config)
            {
                transform.Find("Carregar").gameObject.SetActive(false);
                transform.Find("Config").gameObject.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        
    }
    public void Retornar()
    {
        if(!open)
        {
            nMenu--;
            if(nMenu < 0) nMenu = 0;
            config = false;
            load = false;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainFirstButton);
        }
    }
    public void ConfigHub()
    {
        nMenu = 1;
        config = true;
        transform.Find("Config").gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(configFirstButton);
    }
    public void CarregarHub()
    {
        nMenu = 1;
        load = true;
        transform.Find("Carregar").gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(loadFirstButton);
    }
    public void Carregar1()
    {
        Save_Load.instance.saveSlot = 0;
        SceneManager.LoadScene("Demo");
        GameManager.instance.Pause_Unpause(false);
    }
    public void Carregar2()
    {
        Time.timeScale = 1f;
        Save_Load.instance.saveSlot = 1;
        SceneManager.LoadScene("Demo");
        Time.timeScale = 1f;
        GameManager.instance.Pause_Unpause(false);
    }
    public void Carregar3()
    {
        Time.timeScale = 1f;
        Save_Load.instance.saveSlot = 2;
        SceneManager.LoadScene("Demo");
        GameManager.instance.Pause_Unpause(false);
    }
    public void Carregar4()
    {
        Time.timeScale = 1f;
        Save_Load.instance.saveSlot = 3;
        SceneManager.LoadScene("Demo");
        GameManager.instance.Pause_Unpause(false);
    }
    public void Delete1()
    {
        Save_Load.instance.saveSlot = 0;
        Save_Load.instance.saveConfig();
        Save_Load.instance.deleteSave(Save_Load.instance.savePathP, Save_Load.instance.savePathI1, Save_Load.instance.savePathI2, Save_Load.instance.savePathI3, Save_Load.instance.savePathI4);
    }
    public void Delete2()
    {
        Save_Load.instance.saveSlot = 1;
        Save_Load.instance.saveConfig();
        Save_Load.instance.deleteSave(Save_Load.instance.savePathP, Save_Load.instance.savePathI1, Save_Load.instance.savePathI2, Save_Load.instance.savePathI3, Save_Load.instance.savePathI4);
    }
    public void Delete3()
    {
        Save_Load.instance.saveSlot = 2;
        Save_Load.instance.saveConfig();
        Save_Load.instance.deleteSave(Save_Load.instance.savePathP, Save_Load.instance.savePathI1, Save_Load.instance.savePathI2, Save_Load.instance.savePathI3, Save_Load.instance.savePathI4);
    }
    public void Delete4()
    {
        Save_Load.instance.saveSlot = 3;
        Save_Load.instance.saveConfig();
        Save_Load.instance.deleteSave(Save_Load.instance.savePathP, Save_Load.instance.savePathI1, Save_Load.instance.savePathI2, Save_Load.instance.savePathI3, Save_Load.instance.savePathI4);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
