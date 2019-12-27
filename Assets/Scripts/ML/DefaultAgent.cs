using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAgent : Agent
{
    public float speed = 10;
    public Transform Target;
    Rigidbody2D rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    public override void AgentReset()
    {
        // Move the target to a new spot
        Target.position = new Vector3(Random.value * 8 - 4,
                                      0.5f,
                                      Random.value * 8 - 4);
    }

    public override void CollectObservations()
    {
        // Target and Agent positions
        AddVectorObs(Target.position);
        AddVectorObs(this.transform.position);
        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.y);
    }

    public override void AgentAction(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.y = vectorAction[1];
        rBody.AddForce(controlSignal * speed);
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.position);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }
        // Fell off platform
        if (this.transform.position.y < 0)
        {
            Done();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
