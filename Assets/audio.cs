using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    private static audio instance;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
