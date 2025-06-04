using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrafficSimulation;
using System.Linq;

namespace TrafficSimulation
{
    public enum SignalStatus
    {
        RED,
        YELLOW,
        GREEN
    }

    public class RedLightStatus : MonoBehaviour
    {
        public Intersection intersection;
        public SignalStatus signalStatus = SignalStatus.RED;
        public SignalStatus prevSignalStatus = SignalStatus.RED;
        public GameObject queueLine;
        public CustCollider queueLane;
        List<GameObject> vehiclesQueueIntersection;

        private Light pointLight;
        private float waitTime;
        private float totalTime;
        public float avgWaitTime;
        private float totalPhase = 0;

        private bool isCounting = false;      // Timer flag
        private float countTimer = 0f;        // Accumulated seconds

        void Awake()
        {
            vehiclesQueueIntersection = new List<GameObject>();
            queueLane = queueLine.GetComponent<CustCollider>();
            pointLight = this.transform.GetChild(0).GetComponent<Light>();
        }

        void Update()
        {
            if (isCounting)
            {
                countTimer += Time.deltaTime;
                waitTime = countTimer;
                if (!isCounting)
                {
                    countTimer = 0f;
                }
            } else {
                countTimer += Time.deltaTime;
                if (countTimer >= 5f){
                    intersection.isSignalTimeOut = false;
                }
            }

        }

        public void SetTrafficLightStatus(SignalStatus _signalStatus)
        {
            signalStatus = _signalStatus;
            intersection.isSignalTimeOut = true;

            switch (_signalStatus)
            {
                case SignalStatus.RED:
                    pointLight.color = Color.red;
                    break;

                case SignalStatus.GREEN:
                    pointLight.color = Color.green;
                    for (int i = vehiclesQueueIntersection.Count - 1; i >= 0; i--)
                        Move(vehiclesQueueIntersection[i]);
                    break;

                case SignalStatus.YELLOW:
                    pointLight.color = Color.yellow;
                    for (int i = vehiclesQueueIntersection.Count - 1; i >= 0; i--)
                        SlowDown(vehiclesQueueIntersection[i]);
                    break;
            }
            if (intersection.agent.isUsingAgent) intersection.CheckLights();
        }

        public void ConfigureQueueTrafficLight(GameObject _vehicle)
        {
            switch (signalStatus)
            {
                case SignalStatus.GREEN: Move(_vehicle); break;
                case SignalStatus.YELLOW: SlowDown(_vehicle); break;
                case SignalStatus.RED: Stop(_vehicle); break;
            }
        }

        void Move(GameObject _vehicle)
        {
            CalculateAvarageWaitTime();
            waitTime = 0f;
            isCounting = false;
            countTimer = 0f;

            var vehicleAI = _vehicle.GetComponent<VehicleAI>();
            vehicleAI.vehicleStatus = Status.GO;
            if (vehiclesQueueIntersection.Contains(_vehicle))
                vehiclesQueueIntersection.Remove(_vehicle);
        }

        void SlowDown(GameObject _vehicle)
        {
            Stop(_vehicle);
        }

        void Stop(GameObject _vehicle)
        {
            var vehicleAI = _vehicle.GetComponent<VehicleAI>();
            vehicleAI.vehicleStatus = Status.STOP;

            if (!vehiclesQueueIntersection.Contains(_vehicle))
            {
                vehiclesQueueIntersection.Add(_vehicle);
                isCounting = true;
            }
        }

        void CalculateAvarageWaitTime()
        {
            ++totalPhase;
            totalTime += waitTime;
            avgWaitTime = totalTime / totalPhase;
        }
    }
}