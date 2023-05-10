using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damageToGive;
    public bool canPierce = false;
    GameObject firingShip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ship>() && collision.gameObject != firingShip)
        {
            collision.GetComponent<Ship>().TakeDamage(damageToGive);

            if (!canPierce) { Destroy(gameObject); }
        }
    }

    public void GetFired(GameObject firer)
    {
        firingShip = firer;
    }
}
