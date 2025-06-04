using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSystem : MonoBehaviour
{
    public static BGMSystem Instance { get; set; }
    public AudioSource AudioManager { get; set; }
    public bool toggler = true;
    void Awake()
    {
            DontDestroyOnLoad(this.gameObject);
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
        AudioManager = GetComponent<AudioSource>();
    }

    internal AudioSource GetComponent<T>(AudioSource audioSource)
    {
        throw new NotImplementedException();
    }
}
