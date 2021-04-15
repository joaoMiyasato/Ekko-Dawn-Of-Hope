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
            if(PlayerManager.instance.rb.velocity.x > 0)
            {
                PlayerManager.instance.rb.velocity = new Vector2(PlayerManager.instance.playerMovement.SpeedFix*2/3, PlayerManager.instance.rb.velocity.y);
            }
            else if(PlayerManager.instance.rb.velocity.x < 0)
            {
                PlayerManager.instance.rb.velocity = new Vector2(-PlayerManager.instance.playerMovement.SpeedFix*2/3, PlayerManager.instance.rb.velocity.y);
            }
        }
        }
        else if(ver)
        {
        if(GameManager.instance.Ytransition)
        {
            if(PlayerManager.instance.rb.velocity.y < 0)
            {
                GameManager.instance.YtransitionForce = false;
                PlayerManager.instance.rb.velocity = new Vector2(0, -12);
            }
            else if(PlayerManager.instance.rb.velocity.y > 0)
            {
                if(PlayerManager.instance.playerMovement.facingRight)
                {
                    PlayerManager.instance.rb.velocity = new Vector2(PlayerManager.instance.playerMovement.SpeedFix/3+xForce, PlayerManager.instance.rb.velocity.y);
                    if(GameManager.instance.YtransitionForce)
                    {
                        PlayerManager.instance.rb.velocity = new Vector2(PlayerManager.instance.rb.velocity.x, yForce);
                    }
                }
                else if(!PlayerManager.instance.playerMovement.facingRight)
                {
                    PlayerManager.instance.rb.velocity = new Vector2(-PlayerManager.instance.playerMovement.SpeedFix/3-xForce, PlayerManager.instance.rb.velocity.y);
                    if(GameManager.instance.YtransitionForce)
                    {
                        PlayerManager.instance.rb.velocity = new Vector2(PlayerManager.instance.rb.velocity.x, yForce);
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
            PlayerManager.instance.cantMove = true;
            PlayerManager.instance.cantAction = true;
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
