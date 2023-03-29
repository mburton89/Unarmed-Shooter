using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    Transform target;
    public bool canFireAtPlayer;

    void Start()
    {
        target = FindObjectOfType<PlayerShip>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }
        FlyTowardPlayer();

        if (canFireAtPlayer && canShoot)
        {
            FireProjectile();
        }
    }

    void FlyTowardPlayer()
    {
        Vector2 directionToFace = new Vector2(
            target.position.x - transform.position.x, target.position.y - transform.position.y);
        transform.up = directionToFace;
        Thrust();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerShip>())
        {
            // calculate damage to deal based on velocity vector
            var damageToDeal = (int)Mathf.Round(rb.velocity.x + rb.velocity.y) * 5;
            if (damageToDeal < 0)
            {
                // make the damage value positive if it's not (flying downward)
                damageToDeal *= -1;
            }
            collision.gameObject.GetComponent<PlayerShip>().TakeDamage(damageToDeal);
        }
    }
}
