using UnityEngine;

public class KickOut : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<BarbarianAgent>())
            {
                // collision.gameObject.GetComponent<BarbarianAgent>().Punish(
                    // collision.gameObject.GetComponent<BarbarianAgent>().onExitZonePunishment);
            }
        }
    }
}