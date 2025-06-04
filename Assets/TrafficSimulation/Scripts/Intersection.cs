using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrafficSimulation
{
    public enum IntersectionType
    {
        PLAY,
        AUTO,
        AI
    }

    public class Intersection : MonoBehaviour
    {
        public IntersectionType intersectionType;
        public int id;

        //For stop only
        public List<Segment> prioritySegments;

        //For traffic lights only
        public float lightsDuration = 20;
        public float orangeLightDuration = 5;
        public int selectionLight = 0;
        private float timer = 0;
        private int nextLight;
        private int vehiclesCount = 0;

        public bool isSignalTimeOut = false;
        private bool isWaitingForClearIntersection = false;
        private bool isYellowPhase = false;

        public List<Segment> lightsNbr1;
        public List<Segment> lightsNbr2;
        public List<GameObject> selectableTrafficLights;

        public List<GameObject> vehiclesQueueIntersection;
        private List<GameObject> vehiclesInIntersection;
        public List<RedLightStatus> lights;
        private RedLightStatus selectedLight;
        public TrafficAgent agent;
        public GameObject propertyOf;
        private GameManager gm;
        public string cmd;
        private int i = 0;

        void Start()
        {
            vehiclesQueueIntersection = new List<GameObject>();
            vehiclesInIntersection = new List<GameObject>();
            agent = propertyOf.GetComponent<TrafficAgent>();
            foreach (var l in selectableTrafficLights) {
                lights.Add(l.GetComponent<RedLightStatus>());
            }

            lights[selectionLight].SetTrafficLightStatus(SignalStatus.GREEN);
            selectedLight = lights[selectionLight];
        }

        void Update(){
            if (agent.isUsingAgent) {
                timer += Time.deltaTime;
                if (timer >= 600f)
                {
                    agent.OnEpisodeEnd();
                }
            }
        }

        void FixedUpdate() {
            i++;
            if (i == 50) {
                foreach (var light in lights) {
                    var queue = light.queueLane;
                    Debug.Log($" {gameObject.name} || {light.gameObject.name}: \n Panjang Jalan: {queue.colliderLength}m \n Jumlah Kendaraan: {queue.totalVehicles} unit \n Kepadatan Jalan: {queue.currentQueueDensity} unit/m \n Volume Jalan: {queue.currentQueueVolume} vehicle/s");
                    i = 0;
                }
            }
        }

        public void StartAutoSwitchLights()
        {
            CancelInvoke(); // Clear previous calls to avoid overlap
            isYellowPhase = false;
            isWaitingForClearIntersection = false;

            Invoke(nameof(PeriodicLights), lightsDuration);
        }

        public void StopAutoSwitchLights()
        {
            if (IsInvoking(nameof(PeriodicLights))) CancelInvoke(nameof(PeriodicLights));
        }

        public void ResetIntersection()
        {
            int amount = 0;
            foreach (var l in lights){
                amount += l.queueLane.ResetRoad();
                l.queueLane.ResetStatistic();
                l.SetTrafficLightStatus(SignalStatus.RED);
            }
            gm.ResetTraffic(amount);
        }

        void DeadLocked(){
            // mark episode end
            if (agent.isUsingAgent) {
                agent.OnIntersectionDeadLocked();
                timer = 0;
            }
        }

        public void CheckLights(){
            int red = 0;
            int yellow = 0;
            int green = 0;
            foreach (var light in lights) {
                if (light.signalStatus == SignalStatus.RED) red++;
                else if (light.signalStatus == SignalStatus.YELLOW) yellow++;
                else if (light.signalStatus == SignalStatus.GREEN) green++;
            }

            agent.CalculateSignalReward(red, yellow, green);
        }

        void UpdateLightsSelection(){
            selectedLight = lights[selectionLight];
        }

        public void LightsPrev(){
            selectionLight = (selectionLight - 1 + lights.Count) % lights.Count;
            UpdateLightsSelection();
        }

        public void LightsNext(){
            selectionLight = (selectionLight + 1) % lights.Count;
            UpdateLightsSelection();
        }

        public void SetRedSignal(){
            if (!isSignalTimeOut){
                var l = selectedLight.GetComponent<RedLightStatus>();
                l.prevSignalStatus = l.signalStatus;
                l.SetTrafficLightStatus(SignalStatus.RED);
            }
        }

        public void SetYellowSignal(){
            if (!isSignalTimeOut){
                var l = selectedLight.GetComponent<RedLightStatus>();
                l.prevSignalStatus = l.signalStatus;
                l.SetTrafficLightStatus(SignalStatus.YELLOW);
                if (l.prevSignalStatus == SignalStatus.RED) {agent.OnUsingYellowSignal();} 
                else if (l.prevSignalStatus == SignalStatus.GREEN) {agent.OnUsingYellowSignal();}
            }
        }

        public void SetGreenSignal(){
            if (!isSignalTimeOut){
                var l = selectedLight.GetComponent<RedLightStatus>();
                l.prevSignalStatus = l.signalStatus;
                l.SetTrafficLightStatus(SignalStatus.GREEN);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var obj = other.GetComponent<VehicleAI>();
            obj.isOnIntersection = true;
            vehiclesCount++;
        }

        void OnTriggerStay(Collider other){
            var obj = other.GetComponent<VehicleAI>();
            if (obj.hasCollision) {
                Destroy(other);
                gm.ResetTraffic(1);
                if (agent.isUsingAgent && other.CompareTag("AutonomousVehicle")){
                    DeadLocked();
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            var obj = other.GetComponent<VehicleAI>();
            obj.isOnIntersection = false;
            if (agent.isUsingAgent && other.CompareTag("AutonomousVehicle")) {
                agent.OnVehiclePassed();
            }
            --vehiclesCount;
        }

        IEnumerator LightManagement()
        {
            selectedLight.SetTrafficLightStatus(SignalStatus.YELLOW);
            nextLight = (selectionLight + 1) % lights.Count;
            var updateLight = lights[nextLight];
            updateLight.SetTrafficLightStatus(SignalStatus.YELLOW);
            yield return new WaitUntil(() => vehiclesCount == 0);
            selectedLight.SetTrafficLightStatus(SignalStatus.RED);
            selectionLight = nextLight;
            selectedLight = updateLight;
            selectedLight.SetTrafficLightStatus(SignalStatus.GREEN);
            Invoke(nameof(PeriodicLights), lightsDuration);
        }

        void PeriodicLights()
        {
            StartCoroutine(LightManagement());
        }

        private List<GameObject> memvehiclesQueueIntersection = new List<GameObject>();
        private List<GameObject> memVehiclesInIntersection = new List<GameObject>();

        public void SaveIntersectionStatus()
        {
            memvehiclesQueueIntersection = new List<GameObject>(vehiclesQueueIntersection);
            memVehiclesInIntersection = new List<GameObject>(vehiclesInIntersection);
        }

        public void ResumeIntersectionStatus()
        {
            foreach (GameObject v in vehiclesInIntersection)
            {
                foreach (GameObject v2 in memVehiclesInIntersection)
                {
                    if (v.GetInstanceID() == v2.GetInstanceID())
                    {
                        v.GetComponent<VehicleAI>().vehicleStatus = v2.GetComponent<VehicleAI>().vehicleStatus;
                        break;
                    }
                }
            }
            foreach (GameObject v in vehiclesQueueIntersection)
            {
                foreach (GameObject v2 in memvehiclesQueueIntersection)
                {
                    if (v.GetInstanceID() == v2.GetInstanceID())
                    {
                        v.GetComponent<VehicleAI>().vehicleStatus = v2.GetComponent<VehicleAI>().vehicleStatus;
                        break;
                    }
                }
            }
        }
    }
}