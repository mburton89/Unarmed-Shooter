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

    void Start()
    {
        turnSpeed += Random.Range(-10f, 10f);
        
        StartCoroutine(FireRateBuffer()); // So enemies can't shoot when spawning
        if (doesFollowPlayer && FindObjectOfType<PlayerShip>() != null)
        {
            target = FindObjectOfType<PlayerShip>().transform;
        }

        if(target != null)
        {
            Vector2 directionToFace = new Vector2(
            target.position.x - transform.position.x, target.position.y - transform.position.y);
            transform.up = directionToFace;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerShip>() && !canFireAtPlayer && rammerDamage>0)
        {
            collision.gameObject.GetComponent<PlayerShip>().TakeDamage(rammerDamage);
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
            var rotSpeed = turnSpeed * Time.deltaTime;
            Vector2 directionToFace = new Vector2(
            target.position.x - transform.position.x, target.position.y - transform.position.y);

            //transform.up = directionToFace;
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, directionToFace);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotSpeed);

            Thrust();
        }
    }
}
