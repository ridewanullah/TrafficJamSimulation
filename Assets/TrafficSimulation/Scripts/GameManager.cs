using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float waktu;
    public Text textWaktu; 
    public GameObject spawnerObject;
    private Spawner spawner;
    TimeSpan timeSpan;
    string formattedTime;

    void Start()
    {
        spawner = spawnerObject.GetComponent<Spawner>();
        if (!IsInvoking(nameof(Respawn)))
            InvokeRepeating(nameof(Respawn), 0.2f, 0.2f);
    }

    public void ResetTraffic(int amount)
    {
        spawner.countSpawned = Mathf.Max(0, spawner.countSpawned - amount);

        if (!IsInvoking(nameof(Respawn)))
            InvokeRepeating(nameof(Respawn), 0.2f, 0.2f);
    }

    void Respawn()
    {
        spawner.SpawnObject();
        if (spawner.countSpawned >= spawner.maxSpawn && IsInvoking(nameof(Respawn))) CancelInvoke(nameof(Respawn));
    }

    void Update()
    {
        waktu += Time.deltaTime;
        if (textWaktu != null)
        {
            textWaktu.text = GetFormattedTime();
        }
    }

    string GetFormattedTime()
    {
        timeSpan = TimeSpan.FromSeconds(waktu);
        return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}",
                                     timeSpan.Days,
                                     timeSpan.Hours,
                                     timeSpan.Minutes,
                                     timeSpan.Seconds);
    }

    public void Selesai()
    {
        SceneManager.LoadScene("Kesimpulan");
    }
}
