using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public BarbarianAgent initiator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.isTrigger)
        {
            if (initiator.thisCharacter.team != collision.GetComponent<Enemy>().team)
            {
                initiator.KillEnemy();
            }
        }
    }
}
