using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int maxHealth, health;
    public int maxEnergy, energy;
    public bool hasLantern;
    public int powerPoints;
    public int energyStone;
    public float[] position;
    public bool[] chest;
    public bool[] destructableWall;
    public bool skillImpact;
    public bool skillWalljump;

    public PlayerData (scr_player_manager player)
    {
        maxHealth = player.maxLife;
        health = player.curLife;
        maxEnergy = player.maxEnergy;
        energy = player.curEnergy;
        powerPoints = player.PowerPoints;
        energyStone = player.EnergyStones;
        
        hasLantern = player.Lantern;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;

        chest = player.chestOpen;
        destructableWall = player.destructableWallOpen;

        skillImpact = player.Skill_Impact;
        skillWalljump = player.Skill_Walljump;
    }
}
