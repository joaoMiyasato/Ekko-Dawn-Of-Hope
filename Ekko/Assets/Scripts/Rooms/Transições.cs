using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transições : MonoBehaviour
{
    private UI teleport;
    public bool hor,ver;
    public float xForce = 2, yForce = 21;
    private void Start()
    {
        teleport = UI_manager.instance.UI.GetComponent<UI>();
    }
    private void Update()
    {
        if(hor)
        {
        if(GameManager.instance.Xtransition)
        {
            if(scr_player_manager.instance.rb.velocity.x > 0)
            {
                scr_player_manager.instance.rb.velocity = new Vector2(scr_player_manager.instance.Pmove.SpeedFix*2/3, scr_player_manager.instance.rb.velocity.y);
            }
            else if(scr_player_manager.instance.rb.velocity.x < 0)
            {
                scr_player_manager.instance.rb.velocity = new Vector2(-scr_player_manager.instance.Pmove.SpeedFix*2/3, scr_player_manager.instance.rb.velocity.y);
            }
        }
        }
        else if(ver)
        {
        if(GameManager.instance.Ytransition)
        {
            if(scr_player_manager.instance.rb.velocity.y < 0)
            {
                GameManager.instance.YtransitionForce = false;
                scr_player_manager.instance.rb.velocity = new Vector2(0, -12);
            }
            else if(scr_player_manager.instance.rb.velocity.y > 0)
            {
                if(scr_player_manager.instance.Pmove.facingRight)
                {
                    scr_player_manager.instance.rb.velocity = new Vector2(scr_player_manager.instance.Pmove.SpeedFix/3+xForce, scr_player_manager.instance.rb.velocity.y);
                    if(GameManager.instance.YtransitionForce)
                    {
                        scr_player_manager.instance.rb.velocity = new Vector2(scr_player_manager.instance.rb.velocity.x, yForce);
                    }
                }
                else if(!scr_player_manager.instance.Pmove.facingRight)
                {
                    scr_player_manager.instance.rb.velocity = new Vector2(-scr_player_manager.instance.Pmove.SpeedFix/3-xForce, scr_player_manager.instance.rb.velocity.y);
                    if(GameManager.instance.YtransitionForce)
                    {
                        scr_player_manager.instance.rb.velocity = new Vector2(scr_player_manager.instance.rb.velocity.x, yForce);
                    }
                }
            }
        }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameManager.instance.transition = true;
            scr_player_manager.instance.cantMove = true;
            scr_player_manager.instance.cantAction = true;
            if(hor)
            {
                GameManager.instance.Xtransition = true;
                UI_manager.instance.XroomTransition();
            }
            else if(ver)
            {
                GameManager.instance.Ytransition = true;
                GameManager.instance.YtransitionForce = true;
                teleport.meio = transform.Find("meio").GetComponent<Transform>();
                UI_manager.instance.YroomTransition();
            }
            GameManager.instance.Pause_Unpause(true);
        }
    }
}
