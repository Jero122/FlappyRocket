using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkinController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject forwardButton;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] GameObject startText;

    Player player;
    SpriteRenderer playerSpriteRenderer;
    Singleton singleton;
    void Start()
    {
        singleton = FindObjectOfType<Singleton>();
        player = FindObjectOfType<Player>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        UpdateSkin();
    }
    private void UpdateSkin()
    {
        playerSpriteRenderer.sprite = singleton.skinArray[singleton.CurrentSkinIndex].sprite;
        if (singleton.AccumulativePoints >= singleton.skinArray[singleton.CurrentSkinIndex].pointsNeeded)
        {
            pointsText.text = "UNLOCKED";
            player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            startText.GetComponent<Button>().interactable = true;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            pointsText.text = singleton.AccumulativePoints + "/" + singleton.skinArray[singleton.CurrentSkinIndex].pointsNeeded + "POINTS";
            startText.GetComponent<Button>().interactable = false;
        }
    }
    public void NextSkin()
    {
        singleton.CurrentSkinIndex++;
        UpdateSkin();
        if (singleton.skinArray.Length -1 == singleton.CurrentSkinIndex)
        {
            forwardButton.GetComponent<Button>().interactable = false;
        }
        if (singleton.CurrentSkinIndex > 0)
        {
            backButton.GetComponent<Button>().interactable = true;
        }
    }
    public void PreviousSkin()
    {
        singleton.CurrentSkinIndex--;
        UpdateSkin();
        if (singleton.CurrentSkinIndex == 0)
        {
            backButton.GetComponent<Button>().interactable = false;
        }
        forwardButton.GetComponent<Button>().interactable = true;
    }
}
