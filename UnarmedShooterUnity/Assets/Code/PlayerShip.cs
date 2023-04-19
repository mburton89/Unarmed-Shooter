using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerShip : Ship
{
    bool isBoostingFromKill;
    public int shieldPipCharge;
    public int maxPipCharge;

    private void Start()
    {
        isBoostingFromKill = false;
        trackVelocity = true;
        shieldDeployed = false;
        shieldPipCharge = 0;
    }
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed && !isBoostingFromKill) rb.velocity = rb.velocity.normalized * maxSpeed;

        // increments up current boost energy if not yet full
        if (currentBoostEnergy < maxBoost) currentBoostEnergy++;

        // Sends a message when boost gauge is full and makes sure it hasnt already sent the boost ready message
        if (currentBoostEnergy >= maxBoost && incrementBoost)
        {
            currentBoostEnergy = maxBoost;
            print("Boost now ready");
            incrementBoost = false;
        }

        // Checks to see if there is still missing shield and whether or not the shield is not deployed if both is true the ship shield charges
        if (currentArmor < maxArmor && !shieldDeployed) shieldPipCharge++;

        // When a pip reaches full charge, a pip of shield health is regained, the HUD is updated, and the pip charge is reset
        if (shieldPipCharge >= maxPipCharge)
        {
            currentArmor++;
            HUD.Instance.DisplayHealth(currentArmor, currentHealth);
            shieldPipCharge = 0;
        }

        // Makes sure the shield doesn't charge while full
        if (currentArmor >= maxArmor) shieldPipCharge = 0;
    }

    void Update()
    {
        FollowMouse();
        HandleInput();

        if (trackVelocity) lastVelocity = rb.velocity;
    }

    void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {   
            if (currentArmor > 0)
            {
                // Resets the shield charge so that the shield must be down for the full time to get single pip back
                shieldPipCharge = 0;
                shieldDeployed = true;
            }
            else shieldDeployed = false;
        }

        if (Input.GetMouseButtonUp(0)) shieldDeployed = false;

        if (Input.GetMouseButton(1)) Thrust();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // checks to see if boost is available
            if (currentBoostEnergy == maxBoost) StartCoroutine(Boost());
            else print("Boost not ready"); //replace with some sort of UI interaction/sound alert
        }
    }
    void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        Vector2 directionToFace = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = directionToFace;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // collision with enemy
        if (collision.gameObject.GetComponent<EnemyShip>())
        {
            // calculate damage to deal based on velocity vector
            var damageToDeal = (int)Mathf.Round(rb.velocity.x + rb.velocity.y);
            if (damageToDeal < 0) damageToDeal *= -1; // make the damage value positive if it's not (flying downward)
            collision.gameObject.GetComponent<EnemyShip>().TakeDamage(damageToDeal);
            print("Damage dealt by player: " + damageToDeal);


            // take & apply knockback when hitting stuff, unless last hit kills the enemy
            if (collision.gameObject.GetComponent<EnemyShip>().currentArmor > 0)
            {
                trackVelocity = false;

                // screen shake & knockback when hitting a wall
                // multiply default screenshake values by speed, which should max out at 10
                ScreenShakeManager.Instance.transform.DOShakePosition((float)(0.04 * (lastVelocity.magnitude / 2)), (float)(0.3 * (lastVelocity.magnitude / 2)), (int)(9 * (lastVelocity.magnitude / 2)), 1 * (lastVelocity.magnitude / 2), false, true);

                // check for which velocity vector is greater, then apply knockback to only that vector
                Vector2 directionToBounce = (Vector2)transform.position - collision.contacts[0].point;
                rb.AddForce((directionToBounce * lastVelocity.magnitude) * 2, ForceMode2D.Impulse);

                trackVelocity = true;
            }

            // apply speed boost on kill
            if (collision.gameObject.GetComponent<EnemyShip>().currentArmor <= 0)
            {
                var timeToApplyBoost = 0.25f;
                isBoostingFromKill = true;
                StartCoroutine(ApplySpeedBoostOnKill(timeToApplyBoost));
            }

            //collision.gameObject.GetComponent<EnemyShip>().rb.AddForce(rb.velocity * -5, ForceMode2D.Impulse);
        }

        // collision with wall (layer ID is 6)
        if (collision.gameObject.layer == 6)
        {
            trackVelocity = false;

            // screen shake & knockback when hitting a wall
            // multiply default screenshake values by speed, which should max out at 10
            ScreenShakeManager.Instance.transform.DOShakePosition((float)(0.04 * (lastVelocity.magnitude / 2)), (float)(0.3 * (lastVelocity.magnitude / 2)), (int)(9 * (lastVelocity.magnitude / 2)), 1 * (lastVelocity.magnitude / 2), false, true);

            // check for which velocity vector is greater, then apply knockback to only that vector
            Vector2 directionToBounce = (Vector2)transform.position - collision.contacts[0].point;
            rb.AddForce((directionToBounce * lastVelocity.magnitude) * 2, ForceMode2D.Impulse);

            trackVelocity = true;
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

    private IEnumerator Boost()
    {
        maxSpeed += boostSpeed;
        acceleration += boostSpeed;
        currentBoostEnergy = 0;
        incrementBoost = true;

        yield return new WaitForSeconds(0.3f);

        maxSpeed -= boostSpeed;
        acceleration -= boostSpeed;
        yield break;
    }
}
