using MLAgents;
using UnityEngine;
using TMPro;

public class BarbarianAgent : Agent
{
    [Header("Physics values")]
    Rigidbody2D rb;
    public float speed; 

    [Header("Area values")]
    public Transform spawn;
    public Transform target;

    [Header("Score variables")]
    public TextMeshProUGUI scoreText;
    private float score = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void AgentReset()
    {
        transform.position = spawn.position;
        rb.velocity = new Vector2(0f, 0f);
        target.position = new Vector3(Random.Range(0, 4), Random.Range(0, 4), 0f);
    }

    public override void CollectObservations()
    {
        AddVectorObs(transform.position);
        AddVectorObs(target.position);
        AddVectorObs(rb.velocity.x);
        AddVectorObs(rb.velocity.y);
    }

    public override void AgentAction(float[] vectorAction)
    {
        scoreText.text = "" + score;
        Vector2 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.y = vectorAction[1];
        rb.AddForce(controlSignal * speed);
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  target.position);
        // Reached target
        if (distanceToTarget < 0.42f)
        {
            SetReward(1.0f);
            score += 1.0f;
            Done();
        }
        if (distanceToTarget > 5)
        {
            Done();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[1] = Input.GetAxisRaw("Vertical");
        action[0] = Input.GetAxisRaw("Horizontal");
        return action;
    }
}
