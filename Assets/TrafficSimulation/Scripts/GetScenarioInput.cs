using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetScenarioInput : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject SpawnInput, DurationInput;
    [SerializeField] public TMP_Text SpawnLabel, DurationLabel;
    void Awake()
    {
        Debug.Log("first this is from saved PlayerPrefs: " + PlayerPrefs.GetString("spawnInput"));
        Debug.Log("second this is from saved PlayerPrefs: " + PlayerPrefs.GetString("durationInput"));
    }
    public void SaveVehicleAmount()
    {
        Debug.Log(SpawnInput.GetComponent<TMP_InputField>().text);
        PlayerPrefs.SetString("spawnInput", SpawnInput.GetComponent<TMP_InputField>().text);
        PlayerPrefs.Save();
        Debug.Log("this is from saved PlayerPrefs: " + PlayerPrefs.GetString("spawnInput"));

    }

    public void SaveGameDuration()
    {
        Debug.Log(DurationInput.GetComponent<TMP_InputField>().text);
        PlayerPrefs.SetString("durationInput", DurationInput.GetComponent<TMP_InputField>().text);
        PlayerPrefs.Save();
        Debug.Log("this is from saved PlayerPrefs: " + PlayerPrefs.GetString("durationInput"));
    }

    public void SaveChosenGameRoad1()
    {
        PlayerPrefs.SetString("chosenGameRoad", "JLN. MT HARYONO");
    }
    public void SaveChosenGameRoad2()
    {
        PlayerPrefs.SetString("chosenGameRoad", "JLN. LAKSADA ADI SUCIPTO");
    }
    public void SaveChosenGameRoad3()
    {
        PlayerPrefs.SetString("chosenGameRoad", "JLN. BOROBUDUR");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
