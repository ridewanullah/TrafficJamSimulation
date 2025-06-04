using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TrafficSimulation;
using TMPro;

public class Interaction : MonoBehaviour
{
    public List<GameObject> selectableIntersection;
    public GameObject pause;
    public TerminalCommandRunner trainer;
    public TextMeshProUGUI text;

    private readonly string[] states = { "Play Mode", "Auto Mode", "AI Mode" };
    private int stateIndex = 1;
    private int currentIntersectionSelection = 0;
    private int currentControllerSelection = 0;
    private bool hastenSpeed = false;
    private GameObject currentIntersectionObject;
    private Intersection prevIntersectionObject;
    private GameObject currentControllerObject;

    void Start()
    {
        UpdateIntersectionSelection();
        foreach (var intersections in selectableIntersection)
        {
            var selectedIntersection = intersections.GetComponent<Intersection>();
            AutoMode(selectedIntersection);
        }
    }

    public void OnModeClick()
    {
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        switch (stateIndex)
        {
            case 0: PlayMode(); break;
            case 1: AutoMode(intersection); break;
            case 2: AIMode(); break;
        }
        stateIndex = (stateIndex + 1) % states.Length;
    }

    void CheckGlobalAutoMode()
    {
        foreach (var go in selectableIntersection)
        {
            var intersection = go.GetComponent<Intersection>();
            intersection.StopAutoSwitchLights();
        }
    }

    void CheckGlobalAIMode()
    {
        foreach (var go in selectableIntersection)
        {
            var intersection = go.GetComponent<Intersection>();
            var agent = intersection.agent;
            agent.isUsingAgent = false;
            intersection.propertyOf.SetActive(false);
            trainer.ExitCommand();
        }
    }

    void CheckLocalAIMode()
    {
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        var agent = intersection.agent;
        agent.isUsingAgent = false;
        intersection.propertyOf.SetActive(false);
        trainer.ExitCommand();
    }

    void CheckLocalAutoMode()
    {
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        intersection.StopAutoSwitchLights();
    }

    public void PlayMode()
    {
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        intersection.intersectionType = IntersectionType.PLAY;
        text.text = states[stateIndex];
        CheckGlobalAutoMode();
        Debug.Log("Exiting Auto Mode");
        Debug.Log($"Play Mode Initiated");
    }

    void AutoMode(Intersection intersections)
    {
        CheckLocalAIMode();
        var intersection = intersections.GetComponent<Intersection>();
        text.text = states[stateIndex];
        intersection.intersectionType = IntersectionType.AUTO;
        intersection.StartAutoSwitchLights();
        Debug.Log("Starting Auto Mode");
    }

    void AIMode()
    {
        Debug.Log($"AI Mode Initiated");
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        intersection.intersectionType = IntersectionType.AI;
        text.text = states[stateIndex];
        CheckLocalAutoMode();
        ReadyTrainer();        
    }

    void ReadyTrainer()
    {
        CheckGlobalAIMode();
        Debug.Log($"Initiating AI mode");
        GameObject obj = GameObject.Find("Notification");
        var pausing = pause.GetComponent<PauseScreen>();
        var intersection = currentIntersectionObject.GetComponent<Intersection>();

        pausing.pause();
        trainer.RunCommand(intersection.cmd);
        if (!obj.activeSelf) obj.SetActive(true);
        Debug.Log($"Ready the trainer, please wait a seconds...");
    }

    public void ConfirmReadiness(){
        GameObject obj = GameObject.Find("Notification");
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        var agent = intersection.agent;
        var pausing = pause.GetComponent<PauseScreen>();

        intersection.propertyOf.SetActive(true);
        agent.isUsingAgent = true;
        pausing.Resume();
        obj.SetActive(false);
    }

    public void RedLightButton()
    {
        CheckLocalAutoMode();
        var status = currentControllerObject.GetComponent<RedLightStatus>();
        status.SetTrafficLightStatus(SignalStatus.RED);
    }

    public void YellowLightButton()
    {
        CheckLocalAutoMode();
        var status = currentControllerObject.GetComponent<RedLightStatus>();
        status.SetTrafficLightStatus(SignalStatus.YELLOW);
    }

    public void GreenLightButton()
    {
        CheckLocalAutoMode();
        var status = currentControllerObject.GetComponent<RedLightStatus>();
        status.SetTrafficLightStatus(SignalStatus.GREEN);
    }

    void UpdateIntersectionSelection()
    {
        if (currentIntersectionObject != null && currentControllerObject != null) prevIntersectionObject = currentControllerObject.GetComponent<Intersection>();
        currentIntersectionObject = selectableIntersection[currentIntersectionSelection];
        UpdateModeButton();
        Debug.Log("Traffic Light " + currentIntersectionObject.name + " is selected");
    }

    public void IntersectionPrevButton()
    {
        currentIntersectionSelection = (currentIntersectionSelection - 1 + selectableIntersection.Count) % selectableIntersection.Count;
        UpdateIntersectionSelection();
    }

    public void IntersectionNextButton()
    {
        currentIntersectionSelection = (currentIntersectionSelection + 1) % selectableIntersection.Count;
        UpdateIntersectionSelection();
    }

    void UpdateControllerSelection()
    {
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        currentControllerObject = intersection.selectableTrafficLights[currentControllerSelection];
        Debug.Log($"Intersection {currentIntersectionObject.name} - Traffic Light {currentControllerObject.name} is selected");
    }

    public void ControllerPrevButton()
    {
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        currentControllerSelection = (currentControllerSelection - 1 + intersection.selectableTrafficLights.Count) % intersection.selectableTrafficLights.Count;
        UpdateControllerSelection();
    }

    public void ControllerNextButton()
    {
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        currentControllerSelection = (currentControllerSelection + 1) % intersection.selectableTrafficLights.Count;
        UpdateControllerSelection();
    }

    void UpdateModeButton()
    {
        var intersection = currentIntersectionObject.GetComponent<Intersection>();
        if (intersection.intersectionType == IntersectionType.PLAY) stateIndex = 0;
        else if (intersection.intersectionType == IntersectionType.AUTO) stateIndex = 1;
        else if (intersection.intersectionType == IntersectionType.AI) stateIndex = 2;
        text.text = states[stateIndex];
    }

    public void OnHastenClick()
    {
        if (hastenSpeed) { hastenSpeed = false; Time.timeScale = 1; }
        hastenSpeed = true;
        Time.timeScale = 3;
    }
}
