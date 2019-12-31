using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class getSingleton : MonoBehaviour
{
    Singleton singleton;
    // Start is called before the first frame update
    void Start()
    {
        singleton = FindObjectOfType<Singleton>();
    }
}
