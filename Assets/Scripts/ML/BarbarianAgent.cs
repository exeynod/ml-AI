using MLAgents;
using UnityEngine;
using TMPro;

public class BarbarianAgent : Agent
{
    [Header("Area values")]
    public GameObject buffer;
    public Transform spawn;
    public float dangerZone;

    [Header("This character parametrs")]
    private Enemy thisChar;
    private Rigidbody2D rb;
    public Transform oponent;

    [Header("UI")]
    public TextMeshProUGUI scoreUI;
    private int score = 0;

    void Start()
    {
        thisChar = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void IncScore()
    {
        ++score;
        scoreUI.text = "" + score;
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
        AddVectorObs(oponent.position);
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

    void AgentAct(float[] act)
    {
        Vector2 moveDir = Vector2.zero;
        switch(act[0])
        {
            case 1:
                moveDir.x = 1;
                break;
            case 2:
                moveDir.x = -1;
                break;
        }
        switch(act[1])
        {
            case 1:
                moveDir.y = 1;
                break;
            case 2:
                moveDir.y = -1;
                break;
        }
        rb.AddForce(moveDir * thisChar.MoveSpeed);
        if (act[2] == 1)
        {
            thisChar.Attack();
            // SetReward(-0.05f);
        }
    }

    public override void AgentAction(float[] vectorAction)
    {
        AgentAct(vectorAction);
        if (Vector3.Distance(transform.position, oponent.position) < 2f)
        {
            SetReward(0.5f);
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[3];
        if (Input.GetKeyDown(KeyCode.W))
        {
            action[0] = 1;
        } else if (Input.GetKeyDown(KeyCode.S)) {
            action[0] = 2;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            action[1] = 1;
        } else if (Input.GetKeyDown(KeyCode.D)) {
            action[1] = 2;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            action[2] = 1;
        } else {
            action[2] = 0;
        }
        return action;
    }
}
