using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Save_Load : MonoBehaviour
{
    public static Save_Load instance;
    public bool newGame = false;
    public int saveSlot;
    public string savePathP, savePathI1, savePathI2, savePathI3, savePathI4;

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
    private void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SavePlayer(string _pathPlayer, string _pathI1, string _pathI2, string _pathI3, string _pathI4)
    {
        SaveSystem.SavePlayer(UI_manager.instance.playerSave, _pathPlayer);
        scr_player_manager.instance.inventoryEnemy.Save(_pathI1);
        scr_player_manager.instance.inventoryJewel.Save(_pathI2);
        scr_player_manager.instance.inventoryMemory.Save(_pathI3);
        scr_player_manager.instance.inventorySyntesis.Save(_pathI4);
    }
    
    public void LoadPlayer(string _pathPlayer, string _pathI1, string _pathI2, string _pathI3, string _pathI4)
    {
        if (!File.Exists(Application.persistentDataPath+_pathPlayer) && 
            !File.Exists(Application.persistentDataPath+_pathI1) && 
            !File.Exists(Application.persistentDataPath+_pathI2) && 
            !File.Exists(Application.persistentDataPath+_pathI3) && 
            !File.Exists(Application.persistentDataPath+_pathI4))
        {
            Debug.Log("Não existe");
        }
        else
        {
            PlayerData data = SaveSystem.LoadPlayer(_pathPlayer);
        
            scr_player_manager.instance.maxLife = data.maxHealth;
            scr_player_manager.instance.curLife = data.health;
            scr_player_manager.instance.maxEnergy = data.maxEnergy;
            scr_player_manager.instance.curEnergy = data.energy;
            scr_player_manager.instance.PowerPoints = data.powerPoints;
            scr_player_manager.instance.EnergyStones = data.energyStone;

            scr_player_manager.instance.Lantern = data.hasLantern;

            Vector2 position;
            position.x = data.position[0];
            position.y = data.position[1];
            GameObject.FindWithTag("Player").gameObject.transform.position = position;

            scr_player_manager.instance.inventoryEnemy.Load(_pathI1);
            scr_player_manager.instance.inventoryJewel.Load(_pathI2);
            scr_player_manager.instance.inventoryMemory.Load(_pathI3);
            scr_player_manager.instance.inventorySyntesis.Load(_pathI4);

            for (int i = 0; i < scr_player_manager.instance.chests.Container.Count; i++)
            {
                scr_player_manager.instance.chests.Container[i].Open = data.chest[i];
            }
            for (int i = 0; i < scr_player_manager.instance.destructableWall.Container.Count; i++)
            {
                scr_player_manager.instance.destructableWall.Container[i].Open = data.destructableWall[i];
            }

            scr_player_manager.instance.Skill_Impact = data.skillImpact;
            scr_player_manager.instance.Skill_Walljump = data.skillWalljump;
        }
    }

    public void SaveMap(string _pathMap)
    {

    }

    public void saveConfig()
    {
        if(saveSlot == 0)
        {
            savePathP = "/saves/saveSlot0/player.data";
            savePathI1 = "/saves/saveSlot0/enemy.data";
            savePathI2 = "/saves/saveSlot0/jewel.data";
            savePathI3 = "/saves/saveSlot0/memory.data";
            savePathI4 = "/saves/saveSlot0/syntesis.data";
        }
        else if(saveSlot == 1)
        {
            savePathP = "/saves/saveSlot1/player.data";
            savePathI1 = "/saves/saveSlot1/enemy.data";
            savePathI2 = "/saves/saveSlot1/jewel.data";
            savePathI3 = "/saves/saveSlot1/memory.data";
            savePathI4 = "/saves/saveSlot1/syntesis.data";
        }
        else if(saveSlot == 2)
        {
            savePathP = "/saves/saveSlot2/player.data";
            savePathI1 = "/saves/saveSlot2/enemy.data";
            savePathI2 = "/saves/saveSlot2/jewel.data";
            savePathI3 = "/saves/saveSlot2/memory.data";
            savePathI4 = "/saves/saveSlot2/syntesis.data";
        }
        else if(saveSlot == 3)
        {
            savePathP = "/saves/saveSlot3/player.data";
            savePathI1 = "/saves/saveSlot3/enemy.data";
            savePathI2 = "/saves/saveSlot3/jewel.data";
            savePathI3 = "/saves/saveSlot3/memory.data";
            savePathI4 = "/saves/saveSlot3/syntesis.data";
        }
    }

    public void deleteSave(string _pathPlayer, string _pathI1, string _pathI2, string _pathI3, string _pathI4)
    {
        if (!File.Exists(Application.persistentDataPath+_pathPlayer) && 
            !File.Exists(Application.persistentDataPath+_pathI1) &&    
            !File.Exists(Application.persistentDataPath+_pathI2) && 
            !File.Exists(Application.persistentDataPath+_pathI3) && 
            !File.Exists(Application.persistentDataPath+_pathI4))
        {
            Debug.Log("Não existe");
        }
        else
        {
            File.Delete(Application.persistentDataPath+_pathPlayer);
            File.Delete(Application.persistentDataPath+_pathI1);
            File.Delete(Application.persistentDataPath+_pathI2);
            File.Delete(Application.persistentDataPath+_pathI3);
            File.Delete(Application.persistentDataPath+_pathI4);

            RefreshEditorProjectWindow();
        }
    }
    void RefreshEditorProjectWindow() 
    {
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }
}
