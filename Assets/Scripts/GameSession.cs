using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class GameSession : MonoBehaviour
{
    // Start is called before the first frame update
    bool gameRunning = false;
    int score = 0;
    ScoreText scoreText;

    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas restartMenu;
    [SerializeField] Canvas settingsMenu;
    [SerializeField] Player player;

    Rigidbody2D playerRb;
    Singleton singleton;
    TextMeshProUGUI finalScoreText;
    TextMeshProUGUI highScoreText;
     TextMeshProUGUI totalScoreText;

    [SerializeField] TextMeshProUGUI MusicText;
    [SerializeField] TextMeshProUGUI SFXText;
    [SerializeField] TextMeshProUGUI GFXText;

    AudioSource musicAudioSource;
    [SerializeField] AudioClip[] scoreSounds;
    AudioSource pointsAudioSource;
    [SerializeField] GameObject postprocessing;

    public bool GameRunning
    {
        get { return gameRunning; }
        set { gameRunning = value; }
    }
    public int Score
    {
        get { return score; }
        set { score = value; }
    }
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerRb = player.GetComponent<Rigidbody2D>();
        singleton = FindObjectOfType<Singleton>();

        finalScoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        highScoreText = GameObject.Find("High Score").GetComponent<TextMeshProUGUI>();
        totalScoreText = GameObject.Find("Total Score").GetComponent<TextMeshProUGUI>();

        scoreText = FindObjectOfType<ScoreText>();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        musicAudioSource = audioSources[0];
        pointsAudioSource = audioSources[1];

        musicAudioSource.mute = !singleton.MusicEnabled;
        if (!singleton.MusicEnabled)
        {
            MusicText.text = "Music - Off";
        }
        else if (singleton.MusicEnabled)
        {

            MusicText.text = "Music - On";
        }
        if (singleton.SFXEnabled)
        {
            SFXText.text = "Sounds - On";
        }
        else
        {
            SFXText.text = "Sounds - Off";
        }
        if (singleton.GFXQuality)
        {
            GFXText.text = "Graphics - Quality";
            postprocessing.GetComponent<PostProcessVolume>().enabled = true;
        }
        else if (!singleton.GFXQuality)
        {
            GFXText.text = "Graphics - Performance";
            postprocessing.GetComponent<PostProcessVolume>().enabled = false;
        }

    }
    public void StartGame()
    {
        if (!gameRunning) gameRunning = true;
        playerRb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(player.spawnedFireParticles);
        mainMenu.enabled = false;

    }
    public void handleDeath()
    {
        singleton.AccumulativePoints += score;
        if (score > singleton.HighScore)
        {
            singleton.HighScore = score;
        }
        finalScoreText.text = "Score: " + score.ToString();
        highScoreText.text = "High Score: " + singleton.HighScore.ToString();
        totalScoreText.text = "Total Score: " + singleton.AccumulativePoints.ToString();

        StartCoroutine(LowerPitch());
        restartMenu.enabled = true;
        GameRunning = false;
    }
    IEnumerator LowerPitch() //lowers the pitch of background music to 0
    {
        for (; musicAudioSource.pitch > 0;)
        {
            yield return new WaitForSeconds(0.01f);
            musicAudioSource.pitch -= 0.01f;
        }
    }
    public void LoadMainMenu()
    {
        restartMenu.enabled = false;
        settingsMenu.enabled = false;
        mainMenu.enabled = true;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowSettingsMenu()
    {
        mainMenu.enabled = false;
        settingsMenu.enabled = true;
    }

    public void ChangeMusicSetting()
    {
        if (!singleton.MusicEnabled)
        {
            musicAudioSource.mute = false;
            singleton.MusicEnabled = true;
            MusicText.text = "Music - On";
        }
        else if (singleton.MusicEnabled)
        {
            musicAudioSource.mute = true;
            singleton.MusicEnabled = false;
            MusicText.text = "Music - Off";
        }
    }

    public void ChangeSFXSetting()
    {
        if (singleton.SFXEnabled)
        {
            singleton.SFXEnabled = false;
            SFXText.text = "Sounds - Off";
        }
        else if (!singleton.SFXEnabled)
        {
            singleton.SFXEnabled = true;
            SFXText.text = "Sounds - On";
        }
    }
    
    public void ChangeGFXSettins()
    {
        if (GFXText.text == "Graphics - Quality")
        {
            singleton.GFXQuality = false;
            GFXText.text = "Graphics - Performance";
            postprocessing.GetComponent<PostProcessVolume>().enabled = false;
        }
        else if (GFXText.text == "Graphics - Performance")
        {
            singleton.GFXQuality = true;
            GFXText.text = "Graphics - Quality";
            postprocessing.GetComponent<PostProcessVolume>().enabled = true;
        }
    }
    public void HandleScoreGain()
    {
        score++;
        scoreText.UpdateScoreText();
        PlaySfx();
    }

    private void PlaySfx()
    {
        if (singleton.SFXEnabled)
        {
            if (score % 10 != 0)
            {
                int soundindex = UnityEngine.Random.Range(0, 3);
                pointsAudioSource.clip = scoreSounds[soundindex];
                pointsAudioSource.Play();
            }
            else
            {
                pointsAudioSource.clip = scoreSounds[4];
                pointsAudioSource.Play();
            }
        }
    }
}
