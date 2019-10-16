using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnduranceHandler : MonoBehaviour
{
    PlayerController playerController;

    public Player player;
    public enum Player
    {
        One,
        Two,
    }

    [SerializeField] private float ticker = 0.0001f;

    public delegate void OnPlayer1EnduranceChange();
    public static event OnPlayer1EnduranceChange OnP1EnduranceTick;

    public delegate void OnPlayer2EnduranceChange();
    public static event OnPlayer2EnduranceChange OnP2EnduranceTick;

    public delegate void OnPlayer1EnduranceRemove();
    public static event OnPlayer1EnduranceRemove OnP1EnduranceRemove;

    public delegate void OnPlayer2EnduranceRemove();
    public static event OnPlayer2EnduranceRemove OnP2EnduranceRemove;
    void OnEnable()
    {
        playerController = this.transform.GetComponent<PlayerController>();
        if (player == Player.One)
        {

        }
        if (player == Player.Two)
        {

        }
    }

    void FixedUpdate()
    {
        AddEnduranceTicker();
    }

    public void AddEndurance(int amount)
    {

    }

    public void RemoveEndurance(int amount)
    {
        if (playerController.endurance >= amount)
        {
            playerController.endurance -= amount;
            if (player == Player.One)
            {
                if (OnP1EnduranceRemove != null) OnP1EnduranceRemove();
            }
            if (player == Player.Two)
            {
                if (OnP2EnduranceRemove != null) OnP2EnduranceRemove();
            }
        }
    }

    //Attack Endurance Remove (Animation event)
    public void EnduranceRemove()
    {
        RemoveEndurance(2);
    }

    private void AddEnduranceTicker()
    {        
        ticker += Time.fixedDeltaTime;
        if (ticker >= playerController.enduranceTicker)
        {
            if ((playerController.endurance + playerController.enduranceTickerAdd) <= playerController.enduranceMax)
            {
                Debug.Log(gameObject.name + " got " + playerController.enduranceTickerAdd + " Endurance");
                playerController.endurance += playerController.enduranceTickerAdd;
                if (player == Player.One)
                {
                    if (OnP1EnduranceTick != null) OnP1EnduranceTick();
                }
                if (player == Player.Two)
                {
                    if (OnP2EnduranceTick != null) OnP2EnduranceTick();
                }
            }
            ticker -= ticker;
        }
    }
}
