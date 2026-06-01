using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerLook : MonoBehaviour
{
    //[SerializeField] public static int FOV = 60;
    [SerializeField] public static int mouseSensitivity = 50;
    public static int oldMouseSensitivity = 50;
    private Transform cam; //dont name it camera because that variable alreayd exists
    private Camera camStats;

    private float xRotation; //this will rotate our player
    private Vector2 lookInput; //this will connect to the new input system's look input

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //lock our cursor, mainly to prevent clicking on other things while the game is playing
        cam = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        //camStats = GetComponent<Camera>();
        //camStats.fieldOfView = FOV;
;
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime; //time.deltatime makes this variable update in seconds instead of frame rate
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; //the up and down camera rotation
        xRotation = Mathf.Clamp(xRotation, -90, 90); //prevent Y look from going upside down too far up or down

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //only rotate camera around the x-axis

        transform.Rotate(Vector3.up * mouseX); //player rotates around vertical axis when mouse goes left or right
    }

    private void OnLook(InputValue inputValue)
    {
        lookInput = inputValue.Get<Vector2>(); //make lok input equal the vector2 input from the new input system
    }
}
