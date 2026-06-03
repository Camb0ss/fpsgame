using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private bool green;
    [SerializeField] private GameObject wall;

    private void Start()
    {
        wall =  GetComponent<GameObject>();
    }
    public void Interact()
    {
        Debug.Log(Random.Range(0, 100));
        if(green == true)
        {
            GameManager.health = 0;
        }
        if(green == false)
        {
            wall.SetActive(false);
        }
    }
    
}
