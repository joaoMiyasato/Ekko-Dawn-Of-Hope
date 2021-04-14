using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_player_manager : MonoBehaviour
{
    public static scr_player_manager instance;

    public float gravity;

    public InventoryObject inventoryJewel;
    public InventoryObject inventoryMemory;
    public InventoryObject inventorySyntesis;
    public InventoryObject inventoryEnemy;

    public scr_player_base Pbase;
    public scr_player_move Pmove;
    public scr_player_attack Pattack;
    public scr_player_habilities Phabilities;
    public Animator anim;

    public Rigidbody2D rb;

    public InteractableObject chests;
    public bool[] chestOpen;
    public InteractableObject destructableWall;
    public bool[] destructableWallOpen;
    
    public int maxLife = 500, curLife;
    public int maxEnergy = 300, curEnergy;
    public float iFrames; private bool mapDamage;
    public bool cantMove, cantAction, cantJump, refreshSkill;

    public int PowerPoints = 10000;
    public int EnergyStones = 0;

    public bool Lantern = false;
    public bool LanternIsCharged = false;

    public bool Skill_Impact, Skill_Walljump, Skill_WaterBubble, Skill_DoubleJump;

    private int gravityChange = 1;

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
        gravity = scr_player_manager.instance.rb.gravityScale;
        Pmove = this.GetComponent<scr_player_move>();
        Pbase = this.GetComponent<scr_player_base>();
        Pattack = this.GetComponent<scr_player_attack>();
        Phabilities = this.GetComponent<scr_player_habilities>();
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        ClearAll();
        if(Save_Load.instance.newGame == false)
        {
            Save_Load.instance.saveConfig();
            Save_Load.instance.LoadPlayer(Save_Load.instance.savePathP, Save_Load.instance.savePathI1, Save_Load.instance.savePathI2, Save_Load.instance.savePathI3, Save_Load.instance.savePathI4);
        }
        else
        {
            Save_Load.instance.newGame = false;
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Save_Load.instance.saveConfig();
            Save_Load.instance.LoadPlayer(Save_Load.instance.savePathP, Save_Load.instance.savePathI1, Save_Load.instance.savePathI2, Save_Load.instance.savePathI3, Save_Load.instance.savePathI4);
        }
        else if(Input.GetKeyDown(KeyCode.K))
        {
            Save_Load.instance.saveConfig();
            Save_Load.instance.SavePlayer(Save_Load.instance.savePathP, Save_Load.instance.savePathI1, Save_Load.instance.savePathI2, Save_Load.instance.savePathI3, Save_Load.instance.savePathI4);
        }
        
        for (int i = 0; i < chests.Container.Count; i++)
        {
            chestOpen[i] = chests.Container[i].Open;
        }
        for (int i = 0; i < destructableWall.Container.Count; i++)
        {
            destructableWallOpen[i] = destructableWall.Container[i].Open;
        }

        if(gravityChange == 0)
        {
            rb.gravityScale = 0;
        }
        else if(gravityChange == 1)
        {
            rb.gravityScale = gravity;
        }
    }

    private void FixedUpdate()
    {
        if(iFrames > 0)
        {
            iFrames -= Time.fixedDeltaTime;
        }
    }

    public void setIframes(float howMuch, bool _mapDamage)
    {
        iFrames = howMuch;
        mapDamage = _mapDamage;
        if(mapDamage)
        {
            StartCoroutine(cantActionFor(iFrames*2/3));
            StartCoroutine(cantJumpFor(iFrames*2/3));
            StartCoroutine(cantMoveFor(iFrames*2/3));
        }
        else
        {
            StartCoroutine(cantActionFor(iFrames/2));
            StartCoroutine(cantJumpFor(iFrames/2));
            StartCoroutine(cantMoveFor(iFrames/2));
        }
    }

    public void ClearAll()
    {
        inventorySyntesis.Container.Clear();
        inventoryEnemy.Container.Clear();
        inventoryJewel.Container.Clear();
        inventoryMemory.Container.Clear();

        for (int i = 0; i < chests.Container.Count; i++)
        {
            chests.Container[i].Open = false;
        }
        for (int i = 0; i < destructableWall.Container.Count; i++)
        {
            destructableWall.Container[i].Open = false;
        }
    }

    public IEnumerator cantMoveFor(float T)
    {
        cantMove = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(T);
        cantMove = false;
    }
    public IEnumerator cantJumpFor(float T)
    {
        cantJump = true;
        yield return new WaitForSeconds(T);
        cantJump = false;
    }
    public IEnumerator cantActionFor(float T)
    {
        cantAction = true;
        yield return new WaitForSeconds(T);
        cantAction = false;
    }
    public IEnumerator refreshTime(float T)
    {
        refreshSkill = true;
        yield return new WaitForSeconds(T);
        refreshSkill = false;
    }
    private void OnApplicationQuit()
    {
        ClearAll();
    }

    public void GravityChange(int padrao1)
    {
        gravityChange = padrao1;
    }

    public void DragChange(float padrao1f)
    {
        rb.drag = padrao1f;
    }
}