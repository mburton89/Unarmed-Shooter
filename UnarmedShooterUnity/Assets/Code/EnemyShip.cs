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
        trackVelocity = true;
    }

    void Update()
    {
        if (target != null) FlyTowardPlayer();
        if (canFireAtPlayer && canShoot) FireProjectile();
        if (trackVelocity) lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerShip>())
        {
            // calculate damage to deal based on velocity vector
            var damageToDeal = (int)Mathf.Round(rb.velocity.x + rb.velocity.y);
            if (damageToDeal < 0)
            {
                // make the damage value positive if it's not (flying downward)
                damageToDeal *= -1;
            }
            collision.gameObject.GetComponent<PlayerShip>().TakeDamage(damageToDeal);
            print("Damage dealt by enemy: " + damageToDeal);

            // take & apply knockback when hitting stuff, unless last hit kills the enemy
            // apply knockback based on (1) angle of collision with target (2) velocity right before collision
            trackVelocity = false;

            Vector2 directionToBounce = (Vector2)transform.position - collision.contacts[0].point;
            rb.AddForce((directionToBounce * lastVelocity.magnitude) * 2, ForceMode2D.Impulse);

            trackVelocity = true;
        }
    }

    void FlyTowardPlayer()
    {
        Vector2 directionToFace = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
        transform.up = directionToFace;
        Thrust();
    }
}
