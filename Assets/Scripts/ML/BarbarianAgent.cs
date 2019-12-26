using MLAgents;
using UnityEngine;
using TMPro;

public class BarbarianAgent : Agent
{
    [Header("Physics values")]
    Rigidbody2D rb;
    public float speed;

    [Header("Area values")]
    public GameObject buffer;
    public Transform spawn;
    public float dangerZone;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void AgentReset()
    {
        transform.localPosition = new Vector3(spawn.localPosition.x + Random.Range(-2f, 2f), spawn.localPosition.y + Random.Range(-2f, 2f), 0f);
        rb.velocity = new Vector2(0f, 0f);
        foreach (Transform item in buffer.transform)
        {
            if (item.gameObject.GetComponent<EnemyProjectTile>())
            {
                item.gameObject.GetComponent<EnemyProjectTile>().DestroyPlus();
            }
        }
    }

    public override void CollectObservations()
    {
        AddVectorObs(transform.localPosition);
        AddVectorObs(rb.velocity.x);
        AddVectorObs(rb.velocity.y);
        foreach (Transform item in buffer.transform)
        {
            if (Vector2.Distance(transform.localPosition, item.localPosition) <= dangerZone)
            {
                AddVectorObs(item.position);
                AddVectorObs(item.gameObject.GetComponent<Rigidbody2D>().velocity.x);
                AddVectorObs(item.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
        }
    }
    public void Punish(float value)
    {
        SetReward(-value);
        Done();
    }

    public void Reward(float value)
    {
        SetReward(value);
    }

    public override void AgentAction(float[] vectorAction)
    {
        Vector2 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.y = vectorAction[1];
        rb.AddForce(controlSignal * speed);
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[1] = Input.GetAxisRaw("Vertical");
        action[0] = Input.GetAxisRaw("Horizontal");
        return action;
    }
}
