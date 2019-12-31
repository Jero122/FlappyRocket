using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeGenerator : MonoBehaviour
{
    //Config
    [SerializeField] float startingOffset;
    [SerializeField] public float spikeSpawnDistance;
    [SerializeField] float spikeSpawnX;

    [SerializeField] public float minDistBetweenSpikes;
    [SerializeField] public float maxDistBetweenSpikes;

    //Spike prefabs
    [SerializeField] GameObject spike;
    [SerializeField] GameObject scoreCollider;
    [SerializeField] GameObject platform;
    ColourChanger colourChanger;

    //Lower and Upper bounds for spike dimensions
    [SerializeField] float minWidth;
    [SerializeField] float maxWidth;

    [SerializeField] public float minGap;
    [SerializeField] public float maxGap;

    void Start()
    {
        platform = GameObject.Find("Platform");
        colourChanger = FindObjectOfType<ColourChanger>();
        spikeSpawnDistance = startingOffset;
    }

    void Update()
    {
            generateSpike();
    }
    private void generateSpike()
    {
        if (spikeSpawnDistance < platform.transform.position.x + platform.GetComponent<SpriteRenderer>().bounds.extents.x)
        {
            spikeSpawnDistance += UnityEngine.Random.Range(minDistBetweenSpikes, maxDistBetweenSpikes);
            //Spawn bottom spike
            GameObject bottomSpike = Instantiate(spike, new Vector3(spikeSpawnDistance, -spikeSpawnX, 0), Quaternion.identity);
            bottomSpike.transform.localScale = new Vector3(UnityEngine.Random.Range(minWidth, maxWidth), UnityEngine.Random.Range(minWidth, maxWidth), bottomSpike.transform.localScale.z);
            //Spawn top Spike
            GameObject topSpike = Instantiate(spike, new Vector3(spikeSpawnDistance, spikeSpawnX, 0), Quaternion.identity);
            topSpike.transform.localScale = new Vector3(UnityEngine.Random.Range(minWidth, maxWidth), UnityEngine.Random.Range(minWidth, maxWidth), topSpike.transform.localScale.z);
            topSpike.transform.Rotate(new Vector3(0, 0, 180));
            //Spawn score collider
            GameObject tempScoreCollider = Instantiate(scoreCollider, new Vector3(spikeSpawnDistance, 0, 0), Quaternion.identity);

            topSpike.GetComponent<SpriteRenderer>().color = colourChanger.colourToSpawn;
            bottomSpike.GetComponent<SpriteRenderer>().color = colourChanger.colourToSpawn;
            fixSpikePositions(topSpike, bottomSpike);
        }
    }

    private void fixSpikePositions(GameObject topSpike,GameObject bottomSpike)
    {
        SpriteRenderer bottomSpikeRenderer = bottomSpike.GetComponent<SpriteRenderer>();
        SpriteRenderer topSpikeRenderer = topSpike.GetComponent<SpriteRenderer>();
        float currDistanceBetweenSpikes = (topSpike.transform.position.y - topSpikeRenderer.bounds.extents.y) - (bottomSpikeRenderer.transform.position.y + bottomSpikeRenderer.bounds.extents.y);
        float newdistanceBetweenSpikes = UnityEngine.Random.Range(minGap, maxGap);

        if (currDistanceBetweenSpikes < newdistanceBetweenSpikes)
        {
            float distanceToMoveSpikes = newdistanceBetweenSpikes - currDistanceBetweenSpikes;
            topSpike.transform.Translate(new Vector3(0, -(distanceToMoveSpikes / 2), 0));
            bottomSpike.transform.Translate(new Vector3(0, -(distanceToMoveSpikes / 2), 0));
            float newDistanceBetweenSpikes = (topSpike.transform.position.y - topSpikeRenderer.bounds.extents.y) - (bottomSpikeRenderer.transform.position.y + bottomSpikeRenderer.bounds.extents.y);
        }
    }
}
