using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{

    [SerializeField] private float AttackRadius;
    [SerializeField] private float ChaseRadius;
    [SerializeField] private Rigidbody2D ProjectTile;
    private float Angle;
    private bool AttackCondition;
    private bool MoveCondition;
    private Vector3 way;
    private Vector3 AdditionalWay;

    void Update()
    {
        if (Time.timeScale == 0 || IsDead() || !isActive)
        {
            return;
        }
        if (Target)
        {
            FlipSprite(transform.position.x > Target.transform.position.x);
            Action();
        }
    }

    private void Action()
    {
        AttackCondition = Vector3.Distance(Target.transform.position, transform.position) <= AttackRadius && CurrentState == EnemyState.walk;
        MoveCondition = Vector3.Distance(Target.transform.position, transform.position) > AttackRadius
            && Vector3.Distance(Target.transform.position, transform.position) <= ChaseRadius
            && CurrentState == EnemyState.walk;
        way = Target.transform.position - transform.position;
        if (AttackCondition)
        {
            Anim.SetBool("IsWalking", false);
            if (TimeKd <= 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, way, AttackRadius);
                if (hit)
                {
                    if (hit.collider.gameObject == Target)
                    {
                        AttackAction();
                    }
                    else
                    {
                        AdditionalWay *= 0.8f;
                        RaycastHit2D tryHit = Physics2D.Raycast(transform.position,
                            AdditionalWay, Vector3.Distance(transform.position, AdditionalWay));
                        if (!tryHit && AdditionalWay != Vector3.zero)
                        {
                            transform.position = AdditionalWay;
                            MoveCondition = false;
                        }
                    }
                }
                else
                {
                    AttackAction();
                }
            }
            else
            {
                TimeKd -= Time.deltaTime;
            }
        }
        else
        {
            TimeKd += Time.deltaTime;
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


    private void AttackAction()
    {
        MoveCondition = false;
        Debug.DrawRay(transform.position, way * AttackRadius);
        Attack();
        SpawnProjectTile(way);
        AdditionalWay = Vector3.Cross(way, new Vector3(0f, 0f, 1f)) - transform.position;
    }
    private void SpawnProjectTile(Vector3 Buf)
    {
        Rigidbody2D ArrowClone;
        Angle = Vector3.Angle(new Vector3(1f, 0f, 0f), Buf);
        if (Buf.y < 0)
        {
            Angle *= -1f;
        }
        ArrowClone = Instantiate(ProjectTile, transform.position, Quaternion.Euler(0f, 0f, Angle));
        ArrowClone.AddForce(Buf.normalized * 5f, ForceMode2D.Impulse);
    
    }
}
