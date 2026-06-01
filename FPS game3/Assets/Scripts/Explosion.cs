using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab; // boom
    [SerializeField] private int explosionDamage = 20;
    [SerializeField] private float blastRadius = 5f; //boom radius
    [SerializeField] private float lifeSpan = 3f; //needed so the particle system doesn't just disappear 

    //physics force
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float upwardsForce = 2f;

    private Rigidbody rb; //used for trajectory

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //Get the rigidbody off of the bullet
    }

    private void Update()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.1) //if the square root is not zero or negative
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity); //make bullet face direction it is 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, lifeSpan); //destory the explosion after the set time
        }

        //the Blast zone logic
        //creates and invisible sphere and returns everything inside it
        Collider[] objectsInBlast = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider hit in objectsInBlast) //foreach goes through each object in the blast
        {
            //check if we hit the player
            if(hit.CompareTag("Player"))
            {
                GameManager.health -= explosionDamage;
                Debug.Log("Health: " + GameManager.health);
            }

            //check if we hit a turret
            if(hit.CompareTag("Enemy")) //make sure your turret is tagged enemy
            {
                Turret turret = hit.GetComponent<Turret>();
                if(turret != null)
                {
                    turret.TakeDamage(1);
                }
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //(force amount, blast center, blast radius, upwards lift
                rb.AddExplosionForce(explosionForce, transform.position, blastRadius, upwardsForce);
            }
        }
        Destroy(gameObject);
    }
}
