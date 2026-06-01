using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; //needed for images
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text ammoText; //this will give a null referance error if we dont put it in our inspector
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text mainMessage;

    [SerializeField] private Image healthBar;//need using UnityEngine.UI at top for Image to work
    [SerializeField] private Image bloodOverlay;
    [SerializeField] private Image deathOverlay;

    [SerializeField] private bool menuVisible = false;
    [SerializeField] private GameObject ingameMenu;

    //needed for slow fade with overlay
    [SerializeField] private float damageDuration = 1.5f; //how long it takes for blood to dissapear
    private float fadeTime; //this will be out float timer (used ot check when damage happens and long since it happened
    private int previousHealth; //check if my health changed

    private List<GameObject> ingameMenuChildren = new List<GameObject>();

    private void Start()
    {
        foreach (Transform child in ingameMenu.transform)
        {
            ingameMenuChildren.Add(child.gameObject);
        }

        //thisd line of code below make sur bloodoverlay totally transparent
        bloodOverlay.color = new Color(bloodOverlay.color.r, bloodOverlay.color.g, bloodOverlay.color.b, 0);
        deathOverlay.color = new Color(deathOverlay.color.r, deathOverlay.color.g, deathOverlay.color.b, 0);
       // ingameMenu.SetActive(false);
        previousHealth = GameManager.health; //make the previous health our starting health
        mainMessage.text = GameManager.mainMessage;

        Invoke(nameof(ClearMessage), 2f);
        MenuToggle();
    }
    private void Update()
    {
        if (GameManager.loseGame)
        {
            //the line of code below makes our blood overlay totally opaque
            deathOverlay.color = new Color(deathOverlay.color.r, deathOverlay.color.g, deathOverlay.color.b, 1);
            mainMessage.text = "You were defeated";
        }

        if (GameManager.winGame)
        {
            mainMessage.text = "You Won";
        }

        if (GameManager.health < previousHealth && GameManager.loseGame == false)
        {
            bloodOverlay.color = new Color(bloodOverlay.color.r, bloodOverlay.color.g, bloodOverlay.color.b, 1);
            previousHealth = GameManager.health; //reset our health
            fadeTime = damageDuration; //start the fade timer
        }
        if(fadeTime >= 0)
        {
            fadeTime = fadeTime - Time.deltaTime; //this will subtract from the current fade time in seconds
            float alphaPercentage = fadeTime / damageDuration; //how opaque transparent our image is (negatives default to 0)
            bloodOverlay.color = new Color(bloodOverlay.color.r, bloodOverlay.color.g, bloodOverlay.color.b, alphaPercentage);
        }

        healthText.text = GameManager.health + " / " + GameManager.maxHealth;
        healthBar.fillAmount = (float)GameManager.health / GameManager.maxHealth;
        ammoText.text = "Ammo: " + ProjectileWeapon.ammo + "/" + ProjectileWeapon.maxAmmo;
    }

    private void ClearMessage()
    {
        mainMessage.text = ""; //blank out the main message
    }

    

    public void MenuToggle()
    {
        
        Debug.Log(ingameMenuChildren.Count);
        if (menuVisible == true)
        {
            Cursor.lockState = CursorLockMode.None;
            for(int i=0; i<ingameMenuChildren.Count; i++)
            {
                ingameMenuChildren[i].SetActive(true);
            }
            ProjectileWeapon.readyToShoot = false;
            PlayerLook.mouseSensitivity = 0;
            //ingameMenu.SetActive(true);
            // menuVisible = !menuVisible;
        }
        if (menuVisible == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            for (int i = 0; i < ingameMenuChildren.Count; i++)
            {
                ingameMenuChildren[i].SetActive(false);
            }
            ProjectileWeapon.readyToShoot = true;
            PlayerLook.mouseSensitivity = PlayerLook.oldMouseSensitivity;
            //ingameMenu.SetActive(false);
            // menuVisible = !menuVisible;
        }
        menuVisible = !menuVisible;
    }
}
