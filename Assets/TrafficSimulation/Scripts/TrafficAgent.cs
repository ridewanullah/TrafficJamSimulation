using System.Collections;
using System.Collections.Generic;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using TrafficSimulation;

public class TrafficAgent : Agent {
    public GameObject objectManipulation;
    public Intersection intersection;
    public TrafficObservation trafficObservation;
    public bool isUsingAgent = false;
    private CSVLoader csvLoader;
    private float episode = 1f;
    private float totalReward = 0f;
    private float passedVehicle = 0f;
    private float totalStep = 0f;
    
    public override void Initialize()
    {
        intersection = objectManipulation.GetComponent<Intersection>();
        csvLoader = FindObjectOfType<CSVLoader>();
        if (csvLoader != null && csvLoader.statsList != null && csvLoader.statsList.Count > 0){
            episode = csvLoader.statsList[csvLoader.statsList.Count - 1].episode + 1;
        } else {
            episode = 1; // start from episode 1 if no data
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // int obsCountBefore = sensor.ObservationSize(); // Log size before adding
        // int lightCount = intersection.lights.Count;

        foreach (var light in intersection.lights)
        {
            var queue = light.queueLane;

            float laneLength = Mathf.Max(queue.colliderLength, 1f);
            float maxVehicles = laneLength / 6f;
            float maxDensity = 1f / 6f;
            float maxVolume = maxDensity * 15f;

            Debug.Log($" {intersection.gameObject.name} || {light.gameObject.name}: \n Panjang Jalan: {queue.colliderLength}m \n Jumlah Kendaraan: {queue.totalVehicles} unit \n Kepadatan Jalan: {queue.currentQueueDensity} unit/m \n Volume Jalan: {queue.currentQueueVolume} vehicle/s");

            float normTotalVehicles = Mathf.Clamp(queue.totalVehicles / maxVehicles, 0f, 1f);
            float normDensity = Mathf.Clamp(queue.currentQueueDensity / maxDensity, 0f, 1f);
            float normVolume = Mathf.Clamp(queue.currentQueueVolume / maxVolume, 0f, 1f);
            float normAvgDensity = Mathf.Clamp(queue.avgQueueDensity / maxDensity, 0f, 1f);
            float normAvgVolume = Mathf.Clamp(queue.avgQueueVolume / maxVolume, 0f, 1f);
            float normSignalStatus = Mathf.Clamp((int)light.signalStatus / 2f, 0f, 1f);

            sensor.AddObservation(normTotalVehicles);
            sensor.AddObservation(normDensity);
            sensor.AddObservation(normVolume);
            sensor.AddObservation(normAvgDensity);
            sensor.AddObservation(normAvgVolume);
            sensor.AddObservation(normSignalStatus);
        }

        float normSelection = Mathf.Clamp(intersection.selectionLight / (float)Mathf.Max(1, intersection.lights.Count - 1), 0f, 1f);
        sensor.AddObservation(normSelection);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        switch (action) {
            case 0: break;
            case 1: intersection.LightsNext(); break;
            case 2: intersection.LightsPrev(); break;
            case 3: intersection.SetRedSignal(); break;
            case 4: intersection.SetYellowSignal(); break;
            case 5: intersection.SetGreenSignal(); break;
        }

        float totalDeltaDensity = 0f;
        float totalDeltaVolume = 0f;
        float shortTermDensityGain = 0f;
        float shortTermVolumeGain = 0f;

        foreach (var light in intersection.lights) {
            var col = light.queueLane;
            totalDeltaDensity += col.prevAvgQueueDensity - col.avgQueueDensity;
            totalDeltaVolume += col.prevAvgQueueVolume - col.avgQueueVolume;
            shortTermDensityGain += col.avgQueueDensity - col.currentQueueDensity;
            shortTermVolumeGain += col.avgQueueVolume - col.currentQueueVolume;
        }

        int count = intersection.lights.Count;
        if (count > 0) {
            float avgDeltaDensity = totalDeltaDensity / count;
            float avgDeltaVolume = totalDeltaVolume / count;
            float avgShortTermDensityGain = shortTermDensityGain / count;
            float avgShortTermVolumeGain = shortTermVolumeGain / count;

            float _reward = 0f;
            _reward += avgDeltaDensity > 0 ? +0.3f : -0.1f;
            _reward += avgDeltaVolume > 0 ? +0.3f : -0.1f;
            _reward += avgShortTermDensityGain > 0 ? +0.1f : -0.05f;
            _reward += avgShortTermVolumeGain > 0 ? +0.1f : -0.05f;
            AddReward(_reward);
            AddReward(-0.1f);
            totalReward += _reward - 0.1f;
        }
        totalStep++;
        if (totalStep == 1000f) OnEpisodeEnd();
    }

    public void OnVehiclePassed()
    {
        passedVehicle++;
        AddReward(+0.1f);  // Give a small reward
        totalReward += 0.1f;
    }

    public void OnVehicleStuck(){
        AddReward(-0.2f); // Give a penalty
        totalReward -= 0.2f;
    
    }

    public void OnIntersectionDeadLocked(){
        AddReward(-10f); // Give a penalty
        totalReward -= 10f;
        OnEpisodeEnd();
    }

    public void OnUsingYellowSignal(){
        AddReward(+0.1f);
        totalReward += 0.1f;
    }

    public void CalculateSignalReward(int red, int yellow, int green){
        if (green > 1) AddReward(-0.1f);
        else if (green == 1) AddReward(+0.1f);
        else if (red == intersection.lights.Count - 1) AddReward(+0.1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))      discreteActionsOut[0] = 1; // LightsNext
        else if (Input.GetKeyDown(KeyCode.Alpha2)) discreteActionsOut[0] = 2; // LightsPrev
        else if (Input.GetKeyDown(KeyCode.R))      discreteActionsOut[0] = 3; // SetRedSignal
        else if (Input.GetKeyDown(KeyCode.Y))      discreteActionsOut[0] = 4; // SetYellowSignal
        else if (Input.GetKeyDown(KeyCode.G))      discreteActionsOut[0] = 5; // SetGreenSignal
        else                                       discreteActionsOut[0] = 0; // Do nothing
    }

    public override void OnEpisodeBegin()
    {
        totalStep = 0f;
        episode++;
        totalReward = 0f;
        passedVehicle = 0f;
        Debug.Log($"[Agent] Episode {episode} started for {gameObject.name}");
    }

    public void OnEpisodeEnd(){
        trafficObservation.SaveStatisticsToCSV(totalReward, episode, passedVehicle);
        Debug.Log($"Episode ended. Total Reward: {totalReward}");
        EndEpisode();
        intersection.ResetIntersection();
        OnEpisodeBegin();
    }
}
