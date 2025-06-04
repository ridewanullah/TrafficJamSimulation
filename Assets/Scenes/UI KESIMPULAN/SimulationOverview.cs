using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationOverview : MonoBehaviour
{
    [SerializeField] public TMP_Text timeResult, RoadType, TotalVehicleCrossed;
    // Start is called before the first frame update

    void Awake()
    {
        BGMSystem.Instance.AudioManager.UnPause();
    }

    void Start()
    {
        EventHandler countCrossed = new EventHandler();
        timeResult.text = PlayerPrefs.GetString("durationInput");
        RoadType.text = PlayerPrefs.GetString("chosenGameRoad");
        TotalVehicleCrossed.text = PlayerPrefs.GetString("totalCrossedVehicle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
