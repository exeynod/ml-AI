using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectTile : MonoBehaviour
{
    [SerializeField] float destroyTime;
    public StaticDamage parent;
    private float timer;

    void Start()
    {
        timer = destroyTime;
    }

    void Update()
    {
        if (timer <= 0f)
        {
            Destroy(this.gameObject, 0.1f);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
        if (collision.CompareTag("Enemy") && collision.isTrigger)
        {
            if (parent.team != collision.gameObject.GetComponent<Enemy>().team)
            {
                parent.isTileSpanwed = false;
                Destroy(this.gameObject, 0.1f);
            }
        }
        else if (collision.CompareTag("Obstacle"))
        {
            parent.isTileSpanwed = false;
            Destroy(this.gameObject, 0.1f);
        }
    }
}
