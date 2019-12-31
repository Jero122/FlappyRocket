using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    GameSession gameSession;
    SpikeGenerator spikeGenerator;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        spikeGenerator = FindObjectOfType<SpikeGenerator>();
        player = FindObjectOfType<Player>();
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    public void UpdateScoreText()
    {
        scoreText.text = gameSession.Score.ToString();
        StartCoroutine(PopUp());
        IncreaseDifficulty();
    }
    private void IncreaseDifficulty()
    {
        if (gameSession.Score % 10 == 0)
        {
            player.MoveSpeed += 10;
            spikeGenerator.minGap -= 0.1f;
            spikeGenerator.minDistBetweenSpikes -= 0.1f;
        }
    }

    IEnumerator PopUp()
    {
        scoreText.fontSize = 150;
        //Waits 10 frames , play with this
        for (; scoreText.fontSize > 84;)
        {
            yield return new WaitForSeconds(0.01f);
            scoreText.fontSize -= 10;
        }
        StopCoroutine(PopUp());
    }
}
