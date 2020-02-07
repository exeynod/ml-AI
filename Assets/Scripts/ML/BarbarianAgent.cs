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

    [Header("ML options")]
    public Transform oponent;
    private Vector3 oponentSpawn;
    public BattleAcadeny academy;

    [Header("Character values")]
    [HideInInspector] public Enemy thisCharacter;

    [Header("UI variables")]
    private int score = 0;
    public TextMeshProUGUI textField;

    [Header("Rewards")]
    [HideInInspector] public float killReward = 25f;
    [HideInInspector] public float onCommingCloserReward = 2f;
    private bool wasDistanceRewardGiven = false;

    [Header("Punishments")]
    [HideInInspector] public float diePunishment = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        thisCharacter = GetComponent<Enemy>();
        oponentSpawn = oponent.localPosition;
    }

    public void KillEnemy()
    {
        ++score;
        textField.text = "" + score;
        SetReward(killReward);
        Done();
    }

    private float CountDisToOponent()
    {
        return Vector3.Distance(transform.localPosition, oponent.localPosition);
    }

    public override void AgentReset()
    {
        wasDistanceRewardGiven = false;
        thisCharacter.ResetAnim();
        if (academy.FloatProperties.GetPropertyWithDefault("random_agent_spawn", 1) == 0)
        {
            transform.localPosition = spawn.localPosition;
        }
        else
        {
            transform.localPosition = new Vector3(spawn.localPosition.x + Random.Range(-1, 1),
                spawn.localPosition.y + Random.Range(-1, 1), spawn.position.z);
        }
        oponent.localPosition = new Vector3(oponentSpawn.x + Random.Range(-1, 1),
            oponentSpawn.y + Random.Range(-1, 1), oponentSpawn.z);
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
        AddVectorObs(CountDisToOponent());
        AddVectorObs(oponent.gameObject.GetComponent<StaticDamage>().timer);
        AddVectorObs(oponent.gameObject.GetComponent<StaticDamage>().TimeKd - oponent.gameObject.GetComponent<StaticDamage>().timer);
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

    void Move(float[] act)
    {
        Vector2 direction = Vector2.zero;
        switch(act[0]) {
            case 1:
                direction.x = 1;
                break;
            case 2:
                direction.x = -1;
                break;
        }
        switch (act[1])
        {
            case 1:
                direction.y = 1;
                break;
            case 2:
                direction.y = -1;
                break;
        }
        rb.AddForce(direction * speed);
    }

    public override void AgentAction(float[] vectorAction)
    {
        Move(vectorAction);
        if (vectorAction[2] == 1)
            thisCharacter.Attack();
        if (CountDisToOponent() <= 1.05f && !wasDistanceRewardGiven)
        {
            Reward(onCommingCloserReward);
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[3];
        float tempAxis = Input.GetAxisRaw("Horizontal");
        if (tempAxis == -1f)
            action[0] = 2f;
        else if (tempAxis == 1f)
            action[0] = 1f;
        else
            action[0] = 0f;
        tempAxis = Input.GetAxisRaw("Vertical");
        if (tempAxis == -1f)
            action[1] = 2f;
        else if (tempAxis == 1f)
            action[1] = 1f;
        else
            action[1] = 0f;
        if (Input.GetKeyDown(KeyCode.Space))
            action[2] = 1f;
        else
            action[2] = 0f;
        return action;
    }
}
