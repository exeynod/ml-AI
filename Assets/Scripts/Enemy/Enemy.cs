using System.Collections;
using UnityEngine;

public enum EnemyState
{
    walk,
    attack,
    stagger,
    die
}
public class Enemy : MonoBehaviour
{
    [HideInInspector]public EnemyState CurrentState;

    [Header("Movement and attack variables")]
    public float MoveSpeed;
    public float AttackKD;
    public bool isActive;
    [HideInInspector] public Rigidbody2D Body;

    [Header("Animations")]
    public float DeadAnimTime;
    [SerializeField] private float AttackAnimTime;
    [HideInInspector] public SpriteRenderer Sprite;
    [HideInInspector] public Animator Anim;

    [Header("Health variables")]
    public FloatValue MaxHealth;
    public float deffense;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float TimeKd;

    [Header("Items")]
    [SerializeField] private GameObject GoldenCoin;
    [SerializeField] private int baseDroppedCoins;

    [Header("Sigals")]
    [SerializeField] private Signal EnemyBorn;
    [SerializeField] private Signal EnemyDied;

    [Header("Interaction variables")]
    public string team;
    public GameObject Target;

    public virtual void Start()
    {
        Anim = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
        Body = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth.InitialValue;
        CurrentState = EnemyState.walk;
        TimeKd = AttackKD;
        Physics2D.queriesStartInColliders = false;
        if (EnemyBorn)
        {
            EnemyBorn.Raise();
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void Attack()
    {
        StartCoroutine(AttackCo());
    }
    
    private IEnumerator AttackCo()
    {
        if (CurrentState != EnemyState.attack)
        {
            CurrentState = EnemyState.attack;
            Anim.SetBool("IsAttacking", true);
            yield return new WaitForSeconds(AttackAnimTime);
            Anim.SetBool("IsAttacking", false);
            CurrentState = EnemyState.walk;
        }
    }

    virtual public void Knock(float KnockTime, float Damage)
    {
        if (!IsDead())
        {
            currentHealth -= Damage * (1 - deffense);
            if (!IsDead())
            {
                CurrentState = EnemyState.stagger;
                StartCoroutine(GetDamage());
                StartCoroutine(KnockCo(KnockTime));
            }
            else
            {
                Die();
            }
        }
    }

    public IEnumerator GetDamage()
    {
        Anim.SetBool("IsGettingDamage", true);
        yield return new WaitForSeconds(0.3f);
        Anim.SetBool("IsGettingDamage", false);
    }

    public IEnumerator KnockCo(float KnockTime)
    {
        if (Body != null)
        {
            yield return new WaitForSeconds(KnockTime);
            Body.velocity = Vector2.zero;
            CurrentState = EnemyState.walk;
            Body.velocity = Vector2.zero; // Prevent unstopable impulse
        }
    }

    public virtual void Die()
    {
        Body.constraints = RigidbodyConstraints2D.FreezeAll;
        SpawnCoins();
        StartCoroutine(DieCo());
        BoxCollider2D[] temp = gameObject.GetComponents<BoxCollider2D>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].enabled = false;
        }
        if (EnemyDied)
        {
            EnemyDied.Raise();
        }
    }

    public virtual void SpawnCoins()
    {
        int rand = Random.Range(-3, 3);
        for (int i = 0; i < baseDroppedCoins + rand; i++)
        {
            Rigidbody2D temp = Instantiate(GoldenCoin, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            temp.AddForce(new Vector2(Mathf.Sin(i), Mathf.Cos(i)) * i / 10f, ForceMode2D.Impulse);
            StartCoroutine(CoinSpawnCo(temp));
        }
    }

    IEnumerator CoinSpawnCo(Rigidbody2D rig)
    {
        yield return new WaitForSeconds(0.2f);
        if (rig)
        {
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    public IEnumerator DieCo()
    {
        Anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(DeadAnimTime);
        Anim.enabled = false;
        Anim.SetBool("IsDead", false);
        CurrentState = EnemyState.die;
    }

    public void FlipSprite(bool value)
    {
        Sprite.flipX = value;
    }

    public void WalkToTarget(Vector3 newPos)
    {
        FlipSprite();
        Anim.SetBool("IsWalking", true);
        transform.position = newPos;
    }

    public void FlipSprite()
    {
        if (transform.position.x > Target.transform.position.x)
        {
            FlipSprite(true);
        }
        else
        {
            FlipSprite(false);
        }
    }
}
