using UnityEngine;

public class MoveWall : MonoBehaviour
{
    [SerializeField]private GameObject wall;
    private Transform wallLocation;
    private Renderer rend;
    private BoxCollider boxColl;

    //private Rigidbody rb;

    private bool wallVisible = false;

    private void Start()
    {
        boxColl = wall.GetComponent<BoxCollider>();
        rend = GetComponent<Renderer>();
        //wall = GetComponent<GameObject>();
    }
    private void OnTriggerExit(Collider other)
    {
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("Trigger Entered");
            //rb = GetComponent<Rigidbody>();
            //wallLocation.transform.position = new Vector3();
            if (wallVisible == false)
            {
                if (rend != null && boxColl != null)
                {
                    boxColl = wall.AddComponent<BoxCollider>();
                    rend.enabled = true;
                    wallVisible = true;
                }
            }
            else return;
            
        }
    }
}
