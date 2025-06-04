using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawnChecker : MonoBehaviour {
    public bool isAreaClear = true;
    private List<GameObject> countObject = new List<GameObject>();
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("AutonomousVehicle")) {
            isAreaClear = false;
            countObject.Add(other.gameObject);
        }
    }
    
    void OnTriggerStay(Collider other){
        if (!countObject.Contains(other.gameObject)) {countObject.Add(other.gameObject);}
        isAreaClear = false;
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("AutonomousVehicle")) {
            countObject.Remove(other.gameObject);
        }
        if (countObject.Count == 0) isAreaClear = true;
    }
}
