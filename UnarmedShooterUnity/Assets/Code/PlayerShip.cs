using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerShip : Ship
{
    bool isBoostingFromKill;
    bool trackVelocity;
    Vector2 lastVelocity;


    public Image boostImage;
    public Sprite boostReadySprite;
    public Sprite boostNotReadySprite;
    public Image masterUI;
    public GameObject levelWinUIManager;
    public GameObject levelLoseUIManager;


    private void Start()
    {
        isBoostingFromKill = false;
        trackVelocity = true;
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

        if (trackVelocity)
        {
            lastVelocity = rb.velocity;
        }

        float speed = rb.velocity.magnitude * 100f;
        string speedString = speed.ToString("F");
        SpeedGauge.Instance.speedtext.SetText(speedString + " MPH");

        if (FindObjectsOfType<EnemyShip>().Length == 0)
        {
            levelWinUIManager.SetActive(true);
        }
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
        Collider2D myCollider = collision.GetContact(0).collider;
        // collision with wall (layer ID is 6)
        if (myCollider.gameObject.layer == 6)
        {
            trackVelocity = false;

            // screen shake & knockback when hitting a wall
            // multiply default screenshake values by speed, which should max out at 10
            ScreenShakeManager.Instance.transform.DOShakePosition((float)(0.04 * (lastVelocity.magnitude / 2)), (float)(0.3 * (lastVelocity.magnitude / 2)), (int)(9 * (lastVelocity.magnitude / 2)), 1 * (lastVelocity.magnitude / 2), false, true);

            // check for which velocity vector is greater, then apply knockback to only that vector
            Vector2 directionToBounce = (Vector2)transform.position - collision.contacts[0].point;
            rb.AddForce((directionToBounce * lastVelocity.magnitude) * 2, ForceMode2D.Impulse);

            trackVelocity = true;
            return;
        }

        // collision with enemy
        if (myCollider.gameObject.GetComponent<EnemyShip>())
        {

            // calculate damage to deal based on velocity vector
            var damageToDeal = (int)Mathf.Round(rb.velocity.x + rb.velocity.y) * 10;
            if (damageToDeal < 0)
            {
                // make the damage value positive if it's not (flying downward)
                damageToDeal *= -1;
            }
            collision.gameObject.GetComponent<EnemyShip>().TakeDamage(damageToDeal);
            print("Damage dealt by player: " + damageToDeal);


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

    private void OnParticleCollision(GameObject other)
    {
        print("OnParticleCollision");
        TakeDamage(1);
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

    public void TakePlayerDamage()
    {
        if (!isUIMoving)
        {
            isUIMoving = true;
            StartCoroutine(UIonDamage());
            StartCoroutine(UIShake());
        }

        DoomguyHealthManager.Instance.ShowCorrectHealhPortait(currentHealth, maxHealth);

        // enable UI for loss
        if (currentHealth <= 0) levelLoseUIManager.SetActive(true);
    }


    private IEnumerator UIonDamage()
    {

        masterUI.color = Color.red;
        yield return new WaitForSeconds(.25f);
        masterUI.color = Color.white;

    }

    private IEnumerator UIShake()
    {

        float shakeTimer =0;
        Vector3 originalPosition = masterUI.rectTransform.localPosition;


        while(shakeTimer<.25f)
        {
            float randomX=Random.Range(originalPosition.x - shakeAmount, originalPosition.x);

            float randomY = Random.Range(originalPosition.y, originalPosition.y + shakeAmount);



            //set the position of masterUI to random position
            Vector3 randomPosition=new Vector3(randomX,randomY);

            masterUI.rectTransform.localPosition = randomPosition;


            shakeTimer = shakeTimer + Time.deltaTime;
            yield return new WaitForEndOfFrame();


        }

        masterUI.rectTransform.localPosition = originalPosition;
        //set the position of th emasterUI to the original position
        isUIMoving = false;
    }
}
