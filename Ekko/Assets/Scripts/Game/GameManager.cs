using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isPaused; 
    public bool dragging;
    public bool transition, Xtransition, Ytransition, YtransitionForce;

/////////////////////////////////////////////////////////////////////////////////////////

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

    private void Start()
    {

    }
    private void Update()
    {
        DontDestroyOnLoad(this.gameObject);
        mouseMoving();
    }

    public void Pause_Unpause(bool on_off)
    {
        if(on_off == true)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else if(on_off == false)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    private void mouseMoving()
    {
        if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            dragging = true;
        }
        else
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                dragging = false;
            }
        }
    }
    
}