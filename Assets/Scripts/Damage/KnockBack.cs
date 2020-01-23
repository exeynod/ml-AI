using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private DefaultKnock ThisKnockParams; //Scriptable object with parametrs on KnockBack

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") && collision.isTrigger))
        {
            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if (collision.CompareTag("Enemy") && !this.CompareTag("Damage"))
                {
                    AddForce(hit);
                    hit.GetComponent<Enemy>().CurrentState = EnemyState.stagger;
                    collision.GetComponent<Enemy>().Knock(ThisKnockParams.KnockTime, ThisKnockParams.Damage);
                }
            }
        }
    }

    private void AddForce(Rigidbody2D hit)
    {
        Vector2 difference = hit.transform.position - transform.position;
        difference = difference.normalized * ThisKnockParams.Thrust;
        hit.AddForce(difference, ForceMode2D.Impulse);
    }

}
