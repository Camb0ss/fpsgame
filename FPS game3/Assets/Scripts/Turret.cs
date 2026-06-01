using UnityEngine;

public class Turret : MonoBehaviour
{
    //Adding mortar mode
    public enum FiringMode { Cannon, Mortar }
    [SerializeField] private FiringMode shootingMode;
    [SerializeField] private float mortarHeight = 10f; // How high the shell arches

    //Instance Variables Drag and drop elements
    [SerializeField] private GameObject turretBase; //we will put the rotating base here
    [SerializeField] private GameObject turretHead; //this will spin with the base and also look up and down
    [SerializeField] private ParticleSystem muzzleFlash; //we can use our own or the built in one
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private ParticleSystem damage;
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private Transform bulletStartPoint; //where we will instantiate our bullet prefab
    [SerializeField] private GameObject bulletPrefab; //The object we are shooting
    [SerializeField] private LayerMask ignoreLayer; //used to prevent us from hitting ourself with raycast

    [SerializeField] private int health = 5; //how many hits it takes to destroy
    [Range(0f, 1f)][SerializeField] private float hitAccuracy = 0.5f; //odds of bullet hitting us
    [SerializeField] private float shootingDelay = 1f; //number of seconds between shots
    [SerializeField] private float bulletSpeed = 30f; //Just don't go too fast or the bullet will phase through us
    [SerializeField] private float attackDistance = 15f; //how far until it shoots at us

    [SerializeField] private bool turretenabled = true;
    private bool readyToShoot = true; //make true to start or it will never shoot
    private bool allowReset = true; //also needs to be true
    private bool isDead; //needed to delay death so disappear isn't instant
    private GameObject target; //Who the turret is attacking

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player"); //this line makes us target the player
    }

    private void Update()
    {
        if (isDead) //if turret is dead, don't do anything
        {
            return;
        }
        if (health <= 0)
        {
            isDead = true;
            explosion.Play(); //play explosion
            fire.Play();
            Destroy(gameObject, 5f);
        }

        RaycastHit hit; //what the ray from the enemy is hitting. We use this to help our player hide behind walls
        Vector3 distance = target.transform.position - transform.position; //create a vector from turret to target

        if (Physics.Raycast(transform.position, distance, out hit, 100f, ~ignoreLayer))
        {
            if (hit.transform.tag == "Player") //if the raycast hits the player
            {
                if (Vector3.Distance(transform.position, target.transform.position) < attackDistance) //check the distance from player
                {
                    //rotate the base to face the player, but not change up and down. Change head to look directly at player
                    //the top line prevents the Michael Jackson lean
                    turretBase.transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                    if (shootingMode == FiringMode.Cannon)
                    {
                        // Look directly at the player
                        turretHead.transform.LookAt(target.transform.position);
                    }
                    else
                    {
                        // Look in the direction the mortar will actually fire
                        Vector3 launchDirection = CalculateMortarVelocity();
                        if (launchDirection != Vector3.zero)
                        {
                            turretHead.transform.rotation = Quaternion.LookRotation(launchDirection);
                        }
                    }
                    Shoot();
                }
            }
        }
    }

    private void Shoot()
    {
        if(turretenabled == false)
        {
            return;
        }
        if (!readyToShoot) //if not ready to shoot, then don't shoot
        {
            return;
        }

        muzzleFlash.Play();
        readyToShoot = false; //prevent framerate rapid fire

        GameObject bullet = Instantiate(bulletPrefab, bulletStartPoint.position, bulletStartPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (shootingMode == FiringMode.Mortar)
        {
            // Use the parabola math
            rb.linearVelocity = CalculateMortarVelocity();
            rb.useGravity = true; //forces gravity to be on
            Destroy(bullet, 10.0f); // Give mortars a bit more time to land
        }
        else
        {
            // Use your original straight shooting logic
            Vector3 shootingDirection = (target.transform.position - bulletStartPoint.position).normalized;
            rb.linearVelocity = Quaternion.AngleAxis(Random.Range(-4f, 4f) * hitAccuracy, Vector3.up) * shootingDirection * bulletSpeed;
            Destroy(bullet, 3.0f);
        }



        if (allowReset) //reset mode for autofire
        {
            Invoke(nameof(ResetShot), shootingDelay); //Invoke means call method later (at shooting delay time in seconds)
            allowReset = false;
        }
    }

    private Vector3 CalculateMortarVelocity()
    {
        Vector3 targetPos = target.transform.position;
        Vector3 startPos = bulletStartPoint.position;

        // Calculate the distance on the ground (X and Z)
        float displacementY = targetPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);

        // Calculate how much vertical speed we need to reach the height
        float gravity = Physics.gravity.y;
        float velocityY = Mathf.Sqrt(-2 * gravity * mortarHeight);

        // Calculate how much time it takes to go up and back down to the player
        float timeUp = velocityY / -gravity;
        float timeDown = Mathf.Sqrt(2 * (displacementY - mortarHeight) / gravity);
        float totalTime = timeUp + timeDown;

        // Calculate horizontal speed
        Vector3 velocityXZ = displacementXZ / totalTime;

        return velocityXZ + Vector3.up * velocityY;
    }

    private void ResetShot() //called after shooting delay
    {
        readyToShoot = true;
        allowReset = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }
    }
    public void TakeDamage(int damageAmount)
    {
        damage.Play();
        health -= damageAmount;
    }
}
