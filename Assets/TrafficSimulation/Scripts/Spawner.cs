using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int maxSpawn = 10;
    public GameObject[] vehicle;
    public int countSpawned = 0;
    int randomVehicleID;
    public Transform[] spawnPoints;
    int randomPoints;

    void Awake()
    {
        maxSpawn = int.Parse(PlayerPrefs.GetString("spawnInput"));
    }

    public void SpawnObject()
    {
        randomPoints = Random.Range(0, spawnPoints.Length);
        var spawnArea = spawnPoints[randomPoints].GetComponent<AreaSpawnChecker>();

        if (spawnArea == null) return;

        if (spawnArea.isAreaClear && countSpawned < maxSpawn)
        {
            randomVehicleID = Random.Range(0, vehicle.Length);
            Vector3 randomSpawnPosition = spawnPoints[randomPoints].position;
            Instantiate(
                vehicle[randomVehicleID],
                randomSpawnPosition,
                Quaternion.Euler(spawnPoints[randomPoints].eulerAngles)
            );

            countSpawned++;
        }
    }
}