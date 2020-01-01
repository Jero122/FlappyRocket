using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    [SerializeField] public Skin[] skinArray;
    [SerializeField] int currentSkinIndex = 0;

    bool sfxEnabled = true;
    bool musicEnabled = true;
    bool gfxQuality = true;

    private int accumulativePoints;
    private int highScore;

    public bool GFXQuality
    {
        get { return gfxQuality; }
        set { gfxQuality = value; }
    }
    public bool SFXEnabled
    {
        get { return sfxEnabled; }
        set { sfxEnabled = value; }
    }
    public int CurrentSkinIndex
    {
        get { return currentSkinIndex; }
        set { currentSkinIndex = value; }
    }
    public bool MusicEnabled
    {
        get { return musicEnabled; }
        set { musicEnabled = value; }
    }
    public int HighScore
    {
        get { return highScore; }
        set { highScore = value; }
    }
    private void Awake()
    {
        if (FindObjectsOfType<Singleton>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public int AccumulativePoints
    {
        get { return accumulativePoints; }
        set { accumulativePoints = value; }
    }
}
