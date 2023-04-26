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
        powerUpsPrefabs = Resources.LoadAll<GameObject>("PowerUps");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerShip>())
        {
            // calculate damage to deal based on velocity vector
            var damageToDeal = (int)Mathf.Round(rb.velocity.x + rb.velocity.y);
            if (damageToDeal < 0) damageToDeal *= -1; // make the damage value positive if it's not (flying downward)
            collision.gameObject.GetComponent<PlayerShip>().TakeDamage(damageToDeal);
            print("Damage dealt by enemy: " + damageToDeal);


            // take & apply knockback when hitting stuff, unless last hit kills the enemy
            if (collision.gameObject.GetComponent<PlayerShip>().currentArmor > 0)
            {
                trackVelocity = false;

                // check for which velocity vector is greater, then apply knockback to only that vector
                Vector2 directionToBounce = (Vector2)transform.position - collision.contacts[0].point;
                rb.AddForce((directionToBounce * lastVelocity.magnitude) * 2, ForceMode2D.Impulse);

                trackVelocity = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) FlyTowardPlayer();

        if (canFireAtPlayer && canShoot) FireProjectile();
    }

    void FlyTowardPlayer()
    {
        Vector2 directionToFace = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
        transform.up = directionToFace;
        Thrust();
    }
}
