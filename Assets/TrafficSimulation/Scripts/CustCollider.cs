using System.Collections.Generic;
using UnityEngine;

public class CustCollider : MonoBehaviour
{
    public Dictionary<Collider, int> triggerCounts = new Dictionary<Collider, int>();
    public float colliderLength;
    public float totalVehicles;
    public float avgQueueDensity;
    public float prevAvgQueueDensity;
    public float currentQueueDensity;
    public float avgQueueVolume;
    public float prevAvgQueueVolume;
    public float currentQueueVolume;

    private float totalElapsedTime = 0f;

    void Update()
    {
        float deltaTime = Time.deltaTime;
        Statistic(deltaTime);
    }

    public void NotifyEnterTrigger(Collider other)
    {
        if (triggerCounts.ContainsKey(other)){
            triggerCounts[other]++;
        } else {
            triggerCounts[other] = 1;
        }
    }

    public void NotifyExitTrigger(Collider other)
    {
        if (!triggerCounts.ContainsKey(other)) return;

        triggerCounts[other]--;
        if (triggerCounts[other] <= 0)
        {
            triggerCounts.Remove(other);
        }
    }

    public float CalculateDensity()
    {
        currentQueueDensity = colliderLength > 0 ? triggerCounts.Count / colliderLength : 0;
        return currentQueueDensity;
    }

    public float CalculateVolume()
    {
        float totalSpeed = 0;
        foreach (var kvp in triggerCounts)
        {
            var vehicle = kvp.Key.gameObject;
            var _vehicle = vehicle.GetComponent<TrafficSimulation.WheelDrive>();
            totalSpeed += _vehicle.GetSpeedMS();
        }

        float avgSpeed = triggerCounts.Count > 0 ? totalSpeed / triggerCounts.Count : 0;
        currentQueueVolume = avgSpeed > 0 ? currentQueueDensity * avgSpeed : 0;
        return currentQueueVolume; // vehicles/second
    }

    public void Statistic(float deltaTime)
    {
        totalElapsedTime += deltaTime;

        totalVehicles = triggerCounts.Count;

        prevAvgQueueDensity = avgQueueDensity;
        prevAvgQueueVolume = avgQueueVolume;

        float densityNow = CalculateDensity();
        float volumeNow = CalculateVolume(); // vehicles/sec

        avgQueueDensity = ((totalElapsedTime - deltaTime) * avgQueueDensity + deltaTime * densityNow) / totalElapsedTime;
        avgQueueVolume = ((totalElapsedTime - deltaTime) * avgQueueVolume + deltaTime * volumeNow) / totalElapsedTime;
    }

    public void ResetStatistic(){
        triggerCounts.Clear();
        avgQueueDensity = 0;
        avgQueueVolume = 0;
        prevAvgQueueDensity = 0;
        prevAvgQueueVolume = 0;
        currentQueueDensity = 0;
        currentQueueVolume = 0;
    }

    public int ResetRoad(){
        int amount = triggerCounts.Count;
        foreach (var kvp in triggerCounts){
            var vehicle = kvp.Key.gameObject;
            Destroy(vehicle); 
        }
        return amount;
    }
}
