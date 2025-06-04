using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class QueueLengthTool : EditorWindow
{
    private List<Transform> queueObjects = new List<Transform>();
    private Vector2 scrollPos;
    private float queueLengthZ = 0f;
    private float parentLength = 0f;

    [MenuItem("Tools/Queue Length Tool")]
    public static void ShowWindow()
    {
        GetWindow<QueueLengthTool>("Queue Length Tool");
    }

    void OnGUI()
    {
        GUILayout.Label("Queue Length Calculator", EditorStyles.boldLabel);

        if (GUILayout.Button("Calculate Length"))
        {
            CalculateQueueLength();
        }

        queueLengthZ = EditorGUILayout.FloatField("Queue Length (Z):", queueLengthZ);
        parentLength = EditorGUILayout.FloatField("Parent Length (Z):", parentLength);

        GUILayout.Space(10);
        GUILayout.Label("Queue Objects", EditorStyles.label);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        int toRemove = -1;

        for (int i = 0; i < queueObjects.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            queueObjects[i] = (Transform)EditorGUILayout.ObjectField(queueObjects[i], typeof(Transform), true);
            if (GUILayout.Button("X", GUILayout.Width(20)))
                toRemove = i;
            EditorGUILayout.EndHorizontal();
        }

        if (toRemove >= 0 && toRemove < queueObjects.Count)
            queueObjects.RemoveAt(toRemove);

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add Selected"))
        {
            foreach (var obj in Selection.transforms)
            {
                if (!queueObjects.Contains(obj))
                    queueObjects.Add(obj);
            }
        }
    }

void CalculateQueueLength()
{
    if (queueObjects == null || queueObjects.Count == 0)
    {
        queueLengthZ = 0f;
        return;
    }

    queueLengthZ = 0f;

    if (parentLength == 0) {
        foreach (Transform t in queueObjects)
        {
            if (t == null) continue;
            queueLengthZ += Mathf.Abs(t.localScale.z); // Absolute in case scale is negative
        }
    } else {
        foreach (Transform t in queueObjects)
        {
            if (t == null) continue;
            queueLengthZ += Mathf.Abs(t.localScale.z); // Absolute in case scale is negative
        }
        queueLengthZ *= parentLength;
    }
}
}
