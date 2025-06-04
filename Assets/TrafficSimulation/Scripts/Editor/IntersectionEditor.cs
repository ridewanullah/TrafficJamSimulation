// Traffic Simulation
// https://github.com/mchrbn/unity-traffic-simulation

using UnityEngine;
using UnityEditor;

namespace TrafficSimulation {
    [CustomEditor(typeof(Intersection))]
    public class IntersectionEditor : Editor
    {
        private Intersection intersection;

        void OnEnable(){
            intersection = target as Intersection;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Always start with this

           intersection.intersectionType = (IntersectionType)EditorGUILayout.EnumPopup("Intersection type", intersection.intersectionType);

            // Draw propertyOf (GameObject)
            SerializedProperty sPropertyOf = serializedObject.FindProperty("propertyOf");
            EditorGUILayout.PropertyField(sPropertyOf, new GUIContent("Property Of"));
            SerializedProperty sCmd = serializedObject.FindProperty("cmd");
            EditorGUILayout.PropertyField(sCmd, new GUIContent("Trainer Command"));
            EditorGUI.EndDisabledGroup();

            intersection.lightsDuration = EditorGUILayout.FloatField("Light Duration (in s.)", intersection.lightsDuration);
            intersection.orangeLightDuration = EditorGUILayout.FloatField("Orange Light Duration (in s.)", intersection.orangeLightDuration);

            SerializedProperty sTrafficLight = serializedObject.FindProperty("selectableTrafficLights");
            EditorGUILayout.PropertyField(sTrafficLight, new GUIContent("Traffic light cycle object"), true);

            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties(); // Always end with this
        }
    }
}
