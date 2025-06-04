using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TrafficStat
{
    public float episode;
    public float time;
    public float reward;
    public float avgDensity;
    public float avgVolume;
    public float waitTime;
    public float passedVehicles;
}

public class CSVLoader : MonoBehaviour
{
    public List<TrafficStat> statsList = new List<TrafficStat>();

    void Start()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Data/Statistics_log");
        if (csvFile == null)
        {
            Debug.LogWarning("CSV file not found at Resources/Data/Statistics_log!");
            return;
        }

        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Trim().Split(',');

            // CSV format: episode, time, reward, avgDensity, avgVolume, waitTime, passedVehicles
            if (values.Length >= 7)
            {
                try
                {
                    TrafficStat stat = new TrafficStat
                    {
                        episode = float.Parse(values[0]),
                        time = float.Parse(values[1]),
                        reward = float.Parse(values[2]),
                        avgDensity = float.Parse(values[3]),
                        avgVolume = float.Parse(values[4]),
                        waitTime = float.Parse(values[5]),
                        passedVehicles = float.Parse(values[6])
                    };

                    statsList.Add(stat);
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"Skipping line due to parse error: {line}\n{ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"Skipping malformed line: {line}");
            }
        }

        Debug.Log($"✅ Loaded {statsList.Count} stats from CSV.");
    }
}
