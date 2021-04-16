using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float gravity = 10;

    public PlayerBase playerBase;
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public PlayerHabilities playerHabilities;
    public Animator animator;

    public Rigidbody2D rb;
    
    public InventoryObject inventoryJewel;
    public InventoryObject inventoryMemory;
    public InventoryObject inventorySyntesis;
    public InventoryObject inventoryEnemy;

    public InteractableObject chests;
    public bool[] chestOpen;
    public InteractableObject destructableWall;
    public bool[] destructableWallOpen;
    

    private bool hasLantern = false, lanternIsCharged = false;

    private bool skill_Impact = true, skill_Walljump = true, skill_WaterBubble = true, skill_DoubleJump = true;

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
        gravity = rb.gravityScale;
        playerMovement = this.GetComponent<PlayerMovement>();
        playerBase = this.GetComponent<PlayerBase>();
        playerAttack = this.GetComponent<PlayerAttack>();
        playerHabilities = this.GetComponent<PlayerHabilities>();
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
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

#region GET REGION
    public bool getHasLantern()
    {
        return this.hasLantern;
    }
    public bool getLanternIsCharged()
    {
        return this.lanternIsCharged;
    }
    public bool getSkill_Impact()
    {
        return this.skill_Impact;
    }
    public bool getSkill_WallJump()
    {
        return this.skill_Walljump;
    }
    public bool getSkill_WaterBubble()
    {
        return this.skill_WaterBubble;
    }
    public bool getSkill_DoubleJump()
    {
        return this.skill_DoubleJump;
    }

#endregion

#region SET REGION
    public void setHasLantern(bool hasLantern)
    {
        this.hasLantern = hasLantern;
    }
    public void setLanternIsCharged(bool lanternIsCharged)
    {
        this.lanternIsCharged = lanternIsCharged;
    }
    public void setSkill_WallJump(bool skill_Walljump)
    {
        this.skill_Walljump = skill_Walljump;
    }
    public void setSkill_DoubleJump(bool skill_DoubleJump)
    {
        this.skill_DoubleJump = skill_DoubleJump;
    }
    public void setSkill_WaterBubble(bool skill_WaterBubble)
    {
        this.skill_WaterBubble = skill_WaterBubble;
    }
    public void setSkill_Impact(bool skill_Impact)
    {
        this.skill_Impact = skill_Impact;
    }

#endregion
}