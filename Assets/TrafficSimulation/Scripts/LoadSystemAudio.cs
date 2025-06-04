using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSystemAudio : MonoBehaviour
{
    void Awake(){
        if (BGMSystem.Instance != null) BGMSystem.Instance.AudioManager.Pause();
    }
}
