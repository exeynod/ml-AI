using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject initiator;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.isTrigger)
        {
            if (collision.gameObject != initiator)
            {
                initiator.GetComponent<BarbarianAgent>().IncScore();
                initiator.GetComponent<BarbarianAgent>().SetReward(10f);
                initiator.GetComponent<BarbarianAgent>().Done();
            }
        }
    }
}
