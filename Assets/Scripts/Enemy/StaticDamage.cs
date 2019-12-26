using UnityEngine;

public class StaticDamage : Enemy
{
    public Rigidbody2D projectTile;
    public float tileSpeed;
    public bool isWaitingToHit;
    public bool isTileSpanwed;
    public GameObject target;
    public GameObject hitBox;
    private float timer = 0f;

    void Update()
    {
        bool isShootAllowed;
        if (isWaitingToHit)
        {
            isShootAllowed = !isTileSpanwed;
        }
        else
        {
            isShootAllowed = timer <= 0;
            if (isShootAllowed == false)
            {
                timer -= Time.deltaTime;
            }
        }
        if (isShootAllowed)
        {
            Vector3 way = (target.transform.position - transform.position).normalized;
            SpawntProjectTile(way);
            Attack();
            isTileSpanwed = true;
            timer = AttackKD;
        }
    }

    private void SpawntProjectTile(Vector3 way)
    {
        Rigidbody2D ArrowClone;
        float Angle = Vector3.Angle(new Vector3(1f, 0f, 0f), way);
        if (way.y < 0)
        {
            Angle *= -1f;
        }
        ArrowClone = (Rigidbody2D)Instantiate(projectTile, transform.position, Quaternion.Euler(0f, 0f, Angle));
        ArrowClone.gameObject.GetComponent<EnemyProjectTile>().parent = this;
        ArrowClone.AddForce(way.normalized * 5f, ForceMode2D.Impulse);
    }
}
