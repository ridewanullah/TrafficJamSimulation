using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PengaturanBGM : MonoBehaviour
{
    
    public void BGMController()
    {
        if (BGMSystem.Instance.toggler == true)
        {
            BGMSystem.Instance.AudioManager.Stop();
            BGMSystem.Instance.toggler = false;
        }
        else if (BGMSystem.Instance.toggler == false)
        {
            BGMSystem.Instance.AudioManager.Play();
            BGMSystem.Instance.toggler = true;
        }
    }
}
