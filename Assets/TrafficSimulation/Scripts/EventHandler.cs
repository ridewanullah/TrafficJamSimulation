using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public void Awake()
    {
        PlayerPrefs.SetString("totalCrossedVehicle", Random.Range(int.Parse(PlayerPrefs.GetString("spawnInput")) / 3, int.Parse(PlayerPrefs.GetString("spawnInput"))).ToString());
    }

}
