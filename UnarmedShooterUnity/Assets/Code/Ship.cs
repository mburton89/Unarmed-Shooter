using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Ship : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public Vector2 lastVelocity;

    public float acceleration;
    public float maxSpeed;
    public int maxArmor;
    public int maxHealth;

    public float fireRate;
    public float projectileSpeed;
    public int maxBoost;
    [HideInInspector] public int currentBoostEnergy;
    public int boostSpeed;

    public bool canMove;

    [HideInInspector] public float currentSpeed;
    [HideInInspector] public int currentArmor;
    [HideInInspector] public int currentHealth;
    [HideInInspector] public bool canShoot;
    public bool incrementBoost;
    public bool shieldDeployed;
    public bool trackVelocity;

    public static GameObject[] powerUpsPrefabs;
    public float dropChance;
    [HideInInspector] public bool boostUpSent;

    [SerializeField] ParticleSystem thrustParticles;
    private void Awake()
    {
        currentArmor = maxArmor;
        canShoot = false;

        rb = GetComponent<Rigidbody2D>();
        currentBoostEnergy = maxBoost;
        incrementBoost = false;
        shieldDeployed = false;
        boostUpSent = true;
        currentHealth = maxHealth;

        if (thrustParticles == null && GetComponent<Dragonfly>() == null) {
            thrustParticles = GetComponentInChildren<ParticleSystem>();
        }
        powerUpsPrefabs = Resources.LoadAll<GameObject>("PowerUps");

    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
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
        if (canMove)
        {
            rb.AddForce(transform.up * acceleration);
            if (thrustParticles != null) { thrustParticles.Emit(1); }
            
        }  

    }
    public void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed);
        projectile.GetComponent<Projectile>().GetFired(gameObject);
        Destroy(projectile, 4);
        StartCoroutine(FireRateBuffer());
    }

    public IEnumerator FireRateBuffer()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate); 
        canShoot = true;
    }

    public void TakeDamage(int damageToGive)
    {
        //TODO: play getHitSound
        
        if(currentArmor>0)
        {
            currentArmor = currentArmor - damageToGive;
        }
        else
        {
            currentHealth = currentHealth - damageToGive;
        }

        Debug.Log("Armor : " + currentArmor + " -  Health : " + currentHealth);


        if (currentHealth <= 0)
        {
            Explode();
        }

        if (GetComponent<PlayerShip>())
        {
            HUD.Instance.DisplayHealth(currentArmor, currentHealth);
            DoomguyHealthManager.Instance.ShowCorrectHealhPortait(currentArmor, currentHealth);
            DoomguyHealthManager.Instance.ShowSad();
            GetComponent<PlayerShip>().TakePlayerDamage();
        }
    }
    public void Explode()
    {
        ScreenShakeManager.Instance.ShakeScreen();
        Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);
        Destroy(gameObject);

        if (tag == "Enemy")
        {
            if (Random.Range(0f, 1f) <= dropChance)
            {
                int rand = Random.Range(0, powerUpsPrefabs.Length);
                Instantiate(powerUpsPrefabs[rand], transform.position, transform.rotation);
            }
        }

        FindObjectOfType<EnemyShipSpawner>().CountEnemyShips();
    }
}
