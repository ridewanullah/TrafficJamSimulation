using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSimulation{
    public class TrafficLightTrigger : MonoBehaviour {
        public RedLightStatus redLightStatus;

        void OnTriggerEnter(Collider other) {
            if (other.CompareTag("AutonomousVehicle")) {
                redLightStatus.ConfigureQueueTrafficLight(other.gameObject);
            }
        }
    }
}
