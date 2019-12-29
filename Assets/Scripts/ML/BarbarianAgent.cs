using MLAgents;
using UnityEngine;
using TMPro;
using System.Collections;

public class BarbarianAgent : Agent
{
    [Header("Reward vaues")]
    public float dodgeReward;
    public float oncommingReward;
    public float killReward;

    [Header("Punishment values")]
    public float takingDamagePunishment;
    public float diePunishment;

    [Header("Area values")]
    public GameObject projectTilesSpawned;
    public Transform spawn; // spawn point position
    public float dangerZone; // safe distance

    [Header("This character parametrs")]
    private float currentHealth;
    private Enemy thisCharacter; 
    // Should be removed
    public Transform oponent; // current position of Goblin
    // First oponent position to be saved and restored
    private Vector3 oponentSpawn;

    [Header("UI")]
    public TextMeshProUGUI scoreUI;
    private int score = 0;

    void Start()
    {
        thisCharacter = GetComponent<Enemy>();
        currentHealth = thisCharacter.MaxHealth.InitialValue;
        oponentSpawn = oponent.transform.localPosition;
    }

    public void IncScore()
    {
        ++score;
        scoreUI.text = "" + score;
    }

    public void TakeDamage(float value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            Punish(diePunishment);
            Done();
        }
        else
        {
            StartCoroutine(thisCharacter.GetDamage());
            Punish(takingDamagePunishment);
        }
    }

    public override void AgentReset()
    {
        currentHealth = thisCharacter.MaxHealth.InitialValue;
        oponent.transform.localPosition = new Vector3(oponentSpawn.x + Random.Range(-1f, 1f), oponentSpawn.y + Random.Range(-1f, 1f), 0f);
        transform.localPosition = new Vector3(spawn.localPosition.x + Random.Range(-1f, 1f), spawn.localPosition.y + Random.Range(-1f, 1f), 0f);
        thisCharacter.Body.velocity = new Vector2(0f, 0f);
        foreach (Transform item in projectTilesSpawned.transform)
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
        AddVectorObs(thisCharacter.Body.velocity.x);
        AddVectorObs(thisCharacter.Body.velocity.y);
        AddVectorObs(oponent.position);
        AddVectorObs(currentHealth);
        foreach (Transform item in projectTilesSpawned.transform)
        {
            if (Vector2.Distance(transform.localPosition, item.localPosition) <= dangerZone)
            {
                AddVectorObs(item.position);
                AddVectorObs(item.gameObject.GetComponent<Rigidbody2D>().velocity.x);
                AddVectorObs(item.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                AddVectorObs(oponent.position - item.position);
            }
        }
    }

    public void Punish(float value)
    {
        SetReward(-value);
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
        thisCharacter.Body.AddForce(moveDir * thisCharacter.MoveSpeed);
        if ((thisCharacter.Body.velocity.x + thisCharacter.Body.velocity.x) >= 0)
        {
            thisCharacter.FlipSprite(false);
        }
        else
        {
            thisCharacter.FlipSprite(true);
        }
        if (act[2] == 1)
        {
            thisCharacter.Attack();
        }
    }

    public override void AgentAction(float[] vectorAction)
    {
        AgentAct(vectorAction);
        if (Vector3.Distance(transform.position, oponent.position) < 2f)
        {
            SetReward(oncommingReward);
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[3];
        if (Input.GetKeyDown(KeyCode.W))
        {
            action[0] = 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            action[0] = 2;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            action[1] = 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            action[1] = 2;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            action[2] = 1;
        }
        else
        {
            action[2] = 0;
        }
        return action;
    }
}
