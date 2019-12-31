using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDeleter : MonoBehaviour
{
    [SerializeField] GameObject platform;
    // Start is called before the first frame update
    void Start()
    {
        platform = GameObject.Find("Platform");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < platform.transform.position.x - platform.GetComponent<SpriteRenderer>().bounds.extents.x)
        {
            Destroy(gameObject);
        }
    }
}
