using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    [SerializeField]public GameObject[] platforms;
    SpriteRenderer spriteRenderer;
    public Color colourToSpawn;
    float h, s, v;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        for (int i = 0; i < platforms.Length; i++)
        {
            spriteRenderer = platforms[i].GetComponent<SpriteRenderer>();
            Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);
            h += 0.001f;
            if (h > 180) h = -180;
            colourToSpawn = Color.HSVToRGB(h, s, v);
            spriteRenderer.color = colourToSpawn;
        }
    }
}
