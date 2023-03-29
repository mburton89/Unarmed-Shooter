using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : Ship
{
    bool isBoostingFromKill;

    private void Start()
    {
        isBoostingFromKill = false;
    }
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed && !isBoostingFromKill)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Update()
    {
        FollowMouse();
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButton(1))
        {
            Thrust();
        }
    }
    void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        Vector2 directionToFace = new Vector2(
            mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = directionToFace;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyShip>())
        {
            // calculate damage to deal based on velocity vector
            var damageToDeal = (int)Mathf.Round(rb.velocity.x + rb.velocity.y) * 10;
            if (damageToDeal < 0)
            {
                // make the damage value positive if it's not (flying downward)
                damageToDeal *= -1;
            }
            collision.gameObject.GetComponent<EnemyShip>().TakeDamage(damageToDeal);


            // take & apply knockback when hitting stuff, unless last hit kills the enemy
            if (collision.gameObject.GetComponent<EnemyShip>().currentArmor > 0)
            {
                rb.AddForce(rb.velocity * -5, ForceMode2D.Impulse);
            }

            // apply speed boost on kill
            if (collision.gameObject.GetComponent<EnemyShip>().currentArmor <= 0)
            {
                var timeToApplyBoost = 0.25f;
                isBoostingFromKill = true;
                StartCoroutine(ApplySpeedBoostOnKill(timeToApplyBoost));
            }

            collision.gameObject.GetComponent<EnemyShip>().rb.AddForce(rb.velocity * -5, ForceMode2D.Impulse);
        }
    }

    IEnumerator ApplySpeedBoostOnKill(float timeToApplyBoost)
    {
        rb.velocity = rb.velocity.normalized * 20;
        Debug.Log("Got a kill, applying speed boost! New velocity: " + rb.velocity);
        yield return new WaitForSeconds(0.75f);
        isBoostingFromKill = false;
        Debug.Log("Boost has ended.");
    }
}
