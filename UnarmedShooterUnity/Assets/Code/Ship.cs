using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    public float acceleration;
    public float maxSpeed;
    public int maxArmor;
    public float fireRate;
    public float projectileSpeed;
    public int maxBoost;
    [HideInInspector] public int currentBoostEnergy;
    public int boostSpeed;

    [HideInInspector] public float currentSpeed;
    [HideInInspector] public int currentArmor;

    [HideInInspector] public bool canShoot;
    [HideInInspector] public bool boostUpSent;

    [HideInInspector] ParticleSystem thrustParticles;
    private void Awake()
    {
        currentArmor = maxArmor;
        currentBoostEnergy = maxBoost;
        boostUpSent = true;
        canShoot = true;
        thrustParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (rigidbody2D.velocity.magnitude > maxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxSpeed;
        }
        //increments up current boost energy if not yet full
        if (currentBoostEnergy < maxBoost)
        {
            currentBoostEnergy++;
        }
        //Sends a message when boost gauge is full and makes sure it hasnt already sent the boost ready message
        if ((currentBoostEnergy == maxBoost) && (!boostUpSent))
        {
            print("Boost now ready");
            boostUpSent = true;
        }
    }

    public void Thrust()
    {
        rigidbody2D.AddForce(transform.up * acceleration);
        thrustParticles.Emit(1);
    }
    public void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed);
        projectile.GetComponent<Projectile>().GetFired(gameObject);
        Destroy(projectile, 4);
        StartCoroutine(FireRateBuffer());
    }

    private IEnumerator FireRateBuffer()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate); 
        canShoot = true;
    }

    public void TakeDamage(int damageToGive)
    {
        //TODO: play getHitSound
        currentArmor -= damageToGive;
        if (currentArmor <= 0)
        {
            Explode();
        }

        if (GetComponent<PlayerShip>())
        {
            HUD.Instance.DisplayHealth(currentArmor, maxArmor);
        }
    }
    public void Explode()
    {
        ScreenShakeManager.Instance.ShakeScreen();
        Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);
        Destroy(gameObject);

        FindObjectOfType<EnemyShipSpawner>().CountEnemyShips();
    }
}
