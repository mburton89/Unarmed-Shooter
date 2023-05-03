using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    [HideInInspector] public Transform target;
    public bool doesFollowPlayer = true;
    public bool canFireAtPlayer;
    public int rammerDamage = 1;

    public float turnSpeed = 200f;

    [HideInInspector] public bool playerNear = false;

    void Start()
    {
        turnSpeed += Random.Range(-10f, 10f);
        
        StartCoroutine(FireRateBuffer()); // So enemies can't shoot when spawning
        

        if(target != null)
        {
            Vector2 directionToFace = new Vector2(
            target.position.x - transform.position.x, target.position.y - transform.position.y);
            transform.up = directionToFace;
        }
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
        FlyTowardPlayer();

        if (canFireAtPlayer && canShoot)
        {
            FireProjectile();
        }
    }

    public void PlayerInRange()
    {
        if (doesFollowPlayer && FindObjectOfType<PlayerShip>() != null)
        {
            target = FindObjectOfType<PlayerShip>().transform;
        }
    }

    void FlyTowardPlayer()
    {
        if(target != null && canMove)
        {
            var rotSpeed = turnSpeed * Time.deltaTime;
            Vector2 directionToFace = new Vector2(
            target.position.x - transform.position.x, target.position.y - transform.position.y);

            //transform.up = directionToFace;
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, directionToFace);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotSpeed);

            if (!doesFollowPlayer || (doesFollowPlayer && playerNear))
            {
                Thrust();
            }
        }
    }
}
