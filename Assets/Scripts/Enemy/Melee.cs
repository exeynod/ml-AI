using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Enemy
{
    [Header("Melee enemy parametrs")]
    [SerializeField] private float ChaseRadius;
    [SerializeField] private float AttackRadius;
    // public Transform HomePosition; planning to introduce it later
    private bool MoveCondition;
    private bool AttackCondition;

    private void Update()
    {
        if (Time.timeScale == 0 || IsDead() || !isActive)
        {
            return;
        }
        if (Target)
        {
            Action();
        }
    }

    private void Action()
    {
        AttackCondition = Vector3.Distance(Target.transform.position, transform.position) < AttackRadius;
        MoveCondition = Vector3.Distance(Target.transform.position, transform.position) <= ChaseRadius && CurrentState == EnemyState.walk;
        if (AttackCondition)
        {
            Anim.SetBool("IsWalking", false);
            Attack();
        }
        if (MoveCondition)
        {
            WalkToTarget(Vector3.MoveTowards(transform.position,
                            Target.transform.position, MoveSpeed * Time.deltaTime));
        }
        else
        {
            Anim.SetBool("IsWalking", false);
        }
    }

    override public void Knock(float KnockTime, float Damage)
    {
        if (!IsDead())
        {
            currentHealth -= Damage * (1 - deffense);
            if (!IsDead())
            {
                ChaseRadius = 15f; // After getting Damage melee will chase
                CurrentState = EnemyState.stagger;
                StartCoroutine(GetDamage());
                StartCoroutine(KnockCo(KnockTime));
            }
            else
            {
                Body.constraints = RigidbodyConstraints2D.FreezeAll;
                Die();
            }
        }
    }
}

