using UnityEngine;
using UnityEngine.AI;


public class AIPlayerController : MonoBehaviour
{
    public NavMeshAgent agent;

    [SerializeField] private Transform playerlocation;

    public Rigidbody rb;


    private void Start()
    {
        agent.updateRotation = false;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        agent.SetDestination(playerlocation.position);
        /*if(Input.GetMouseButtonDown(0))
        {
            Ray ray = playerlocation.
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }*/

        
        if(agent.remainingDistance > agent.stoppingDistance)
        {
            rb.Move(agent.desiredVelocity, false;
        } else
        {
            rb.Move(Vector3.zero);
        }
    }
}
