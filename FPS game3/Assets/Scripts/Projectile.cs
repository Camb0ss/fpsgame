using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) //when the projectile hits something
    {
        Destroy(gameObject); //destroy game object
    }
}