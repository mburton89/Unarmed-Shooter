using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Ship : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    public float acceleration;
    public float maxSpeed;
    public int maxArmor;
    public int maxHealth;

    public float fireRate;
    public float projectileSpeed;
    public int maxBoost;
    public int currentBoostEnergy;
    public int boostSpeed;

    [HideInInspector] public float currentSpeed;
    [HideInInspector] public int currentArmor;
    [HideInInspector] public int currentHealth;
    [HideInInspector] public bool canShoot;
    public bool incrementBoost;
    public bool shieldDeployed;
    public static GameObject[] powerUpsPrefabs;
    public float dropChance;

    [HideInInspector] ParticleSystem thrustParticles;
    private void Awake()
    {
        currentArmor = maxArmor;
        rb = GetComponent<Rigidbody2D>();
        currentBoostEnergy = maxBoost;
        incrementBoost = false;
        shieldDeployed = false;
        currentHealth = maxHealth;
        canShoot = true;
        thrustParticles = GetComponentInChildren<ParticleSystem>();
        powerUpsPrefabs = Resources.LoadAll<GameObject>("PowerUps");
    }

    public void Thrust()
    {
        rb.AddForce(transform.up * acceleration);
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
        
        if(GetComponent<PlayerShip>())
        {
            if(shieldDeployed)
            {
                currentArmor = currentArmor - damageToGive;
            }
            else
            {
                currentHealth = currentHealth - damageToGive;
            }
        }
        else
        {
            if (currentArmor > 0)
            {
                currentArmor = currentArmor - damageToGive;
            }
            else
            {
                currentHealth = currentHealth - damageToGive;
            }
        }
        

        //Debug.Log("Armor : " + currentArmor + " -  Health : " + currentHealth);


        if (currentHealth <= 0)
        {
            Explode();
        }

        if (GetComponent<PlayerShip>())
        {
            HUD.Instance.DisplayHealth(currentArmor, currentHealth);
        }
    }
    public void Explode()
    {
        ScreenShakeManager.Instance.ShakeScreen();
        Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);

        if(GetComponent<EnemyShip>())
        {
            if (Random.Range(0f, 1f) <= dropChance)
            {
                int rand = Random.Range(0, powerUpsPrefabs.Length);
                Instantiate(powerUpsPrefabs[rand],transform.position, transform.rotation);
            }
        }

        Destroy(gameObject);

        FindObjectOfType<EnemyShipSpawner>().CountEnemyShips();
    }
}
