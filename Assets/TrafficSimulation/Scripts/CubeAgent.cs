using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CubeAgent : Agent
{
    public override void Initialize()
    {
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // We just observe the cube's rotation for simplicity
        sensor.AddObservation(transform.rotation.eulerAngles.y / 360f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0]; // 0: left, 1: right

        if (action == 0)
        {
            transform.Rotate(Vector3.up, -1f); // Rotate left
        }
        else if (action == 1)
        {
            transform.Rotate(Vector3.up, 1f); // Rotate right
        }

        // Reward for being "upright" (rotation.y near 0)
        float angle = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, 0);
        float reward = 1.0f - Mathf.Abs(angle) / 180.0f;
        SetReward(reward);

        // Optional: End episode if rotation is too far (demo purposes)
        if (Mathf.Abs(angle) > 90f)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.LeftArrow) ? 0 : 1;
    }

    public override void OnEpisodeBegin()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(-90f, 90f), 0);
    }
}
