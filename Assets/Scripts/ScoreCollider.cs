using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreCollider : MonoBehaviour
{
    // Start is called before the first frame update
    GameSession gameSession;
    Player player;
    bool collided = false;
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !collided && !player.Dead)
        {
            collided = true;
            gameSession.HandleScoreGain();
        }
    }
}
