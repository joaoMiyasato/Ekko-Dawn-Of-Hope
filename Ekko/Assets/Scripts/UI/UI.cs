using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Transform meio;
    public LayerMask wall;
    private void Update()
    {
    }
    public void unPause()
    {
        GameManager.instance.Pause_Unpause(false);
    }
    public void endTransition()
    {
        GameManager.instance.transition = false;
        GameManager.instance.Xtransition = false;
        GameManager.instance.Ytransition = false;
        PlayerManager.instance.playerBase.setCantMove(false);
        PlayerManager.instance.playerBase.setCantAction(false);
    }
    public void Xteleport()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, Vector2.down, 99, PlayerManager.instance.playerMovement.whatIsGround);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = new Vector3(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.x, GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.y-hit.distance+2,0);
    }
    public void Yteleport()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = new Vector3(meio.position.x, GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.y);
    }
    public void yTeleportForce()
    {
        GameManager.instance.YtransitionForce = false;
    }
}
