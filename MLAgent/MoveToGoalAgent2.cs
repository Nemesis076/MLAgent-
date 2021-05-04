using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent2 : Agent
{
    [SerializeField] private Transform targetTransform;
    //[SerializeField] private Transform guide1Transform;
    //[SerializeField] private Transform guide2Transform;
    //[SerializeField] private Transform guide3Transform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(-36f, 3.5f, 36f);
        //targetTransform.localPosition = new Vector3(Random.Range(2.4f, +5f), 3.5f, Random.Range(-2f, +2f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        //sensor.AddObservation(guide1Transform.localPosition);
        //sensor.AddObservation(guide2Transform.localPosition);
        //sensor.AddObservation(guide3Transform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 30f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TargetGoal>(out TargetGoal goal))
        {
            SetReward(+1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Guide>(out Guide guide))
        {
            SetReward(+1f);
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }

    }
}
