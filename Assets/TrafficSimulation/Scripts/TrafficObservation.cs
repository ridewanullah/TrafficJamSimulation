using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TrafficSimulation;

public class TrafficObservation : MonoBehaviour
{
    public TrafficAgent agent;
    public Intersection intersection;

    public struct LightSnapshot
    {
        public float normTotalVehicles;
        public float normDensity;
        public float normVolume;
        public float normAvgDensity;
        public float normAvgVolume;
        public float normSignalStatus;
    }

    public List<LightSnapshot> stats = new List<LightSnapshot>();
    public float normSelection;

    public void UpdateTrafficObservation()
    {
        stats.Clear();

        foreach (var light in intersection.lights)
        {
            var queue = light.queueLane;
            float laneLength = Mathf.Max(queue.colliderLength, 1f);
            float maxVehicles = laneLength / 6f;
            float maxDensity = 1f / 6f;
            float maxVolume = maxDensity * 15f;

            stats.Add(new TrafficObservation.LightSnapshot
            {
                normTotalVehicles = Mathf.Clamp(queue.totalVehicles / maxVehicles, 0f, 1f),
                normDensity = Mathf.Clamp(queue.currentQueueDensity / maxDensity, 0f, 1f),
                normVolume = Mathf.Clamp(queue.currentQueueVolume / maxVolume, 0f, 1f),
                normAvgDensity = Mathf.Clamp(queue.avgQueueDensity / maxDensity, 0f, 1f),
                normAvgVolume = Mathf.Clamp(queue.avgQueueVolume / maxVolume, 0f, 1f),
                normSignalStatus = Mathf.Clamp((int)light.signalStatus / 2f, 0f, 1f)
            });
        }

        normSelection = Mathf.Clamp(
            intersection.selectionLight / (float)Mathf.Max(1, intersection.lights.Count - 1), 0f, 1f);
    }

    public void SaveStatisticsToCSV(float reward, float episode, float passedVehicle)
    {
        string dir = Path.Combine(Application.persistentDataPath, "Data");
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        string path = Path.Combine(dir, $"{agent.gameObject.name} Statistics_log.csv");

        // If file doesn't exist, write headers first
        if (!File.Exists(path))
        {
            using (StreamWriter header = new StreamWriter(path, false))
            {
                header.WriteLine("episode,time,reward,avgDensity,avgVolume,waitTime,passedVehicles");
            }
        }

        float totalDensity = 0f;
        float totalVolume = 0f;
        float totalWaitTime = 0f;
        int lightCount = intersection.lights.Count;

        foreach (var light in intersection.lights)
        {
            var queue = light.queueLane;
            totalDensity += queue.avgQueueDensity;
            totalVolume += queue.avgQueueVolume;
            totalWaitTime += light.avgWaitTime;
        }

        float avgDensity = lightCount > 0 ? totalDensity / lightCount : 0f;
        float avgVolume = lightCount > 0 ? totalVolume / lightCount : 0f;
        float avgWait = lightCount > 0 ? totalWaitTime / lightCount : 0f;

        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine($"{episode},{Time.time:F2},{reward:F2},{avgDensity:F3},{avgVolume:F3},{avgWait:F2},{passedVehicle}");
        }
    }
}