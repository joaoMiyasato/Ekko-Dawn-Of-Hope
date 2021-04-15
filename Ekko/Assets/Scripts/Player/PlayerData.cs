using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int maxHealth, curHealth;
    public int maxEnergy, curEnergy;
    public bool hasLantern;
    public int powerPoints;
    public int energyStone;
    public float[] position;
    public bool[] chest;
    public bool[] destructableWall;
    public bool skillImpact, skillWalljump, skillWaterBubble, skillDoubleJump;

    public PlayerData (PlayerManager player)
    {
        maxHealth = player.playerBase.getMaxLife();
        curHealth = player.playerBase.getCurrentLife();
        maxEnergy = player.playerBase.getMaxEnergy();
        curEnergy = player.playerBase.getCurrentEnergy();
        powerPoints = player.playerBase.getPowerPoints();
        energyStone = player.playerBase.getEnergyStones();
        
        hasLantern = player.getHasLantern();

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;

        chest = player.chestOpen;
        destructableWall = player.destructableWallOpen;

        skillImpact = player.getSkill_Impact();
        skillWalljump = player.getSkill_WallJump();
        skillDoubleJump = player.getSkill_DoubleJump();
        skillWaterBubble = player.getSkill_WaterBubble();
    }
}
