using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    [HideInInspector] public Transform target;
    public bool doesFollowPlayer = true;
    public bool canFireAtPlayer;
    public int damage = 1;

    void Start()
    {
        if (doesFollowPlayer && FindObjectOfType<PlayerShip>() != null)
        {
            target = FindObjectOfType<PlayerShip>().transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerShip>() && !canFireAtPlayer)
        {
            collision.gameObject.GetComponent<PlayerShip>().TakeDamage(damage);
            Explode();
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

    void FlyTowardPlayer()
    {
        if(target != null && canMove)
        {
            Vector2 directionToFace = new Vector2(
            target.position.x - transform.position.x, target.position.y - transform.position.y);
            transform.up = directionToFace;
            Thrust();
        }
    }
}
