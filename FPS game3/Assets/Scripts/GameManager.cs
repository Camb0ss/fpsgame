using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Static instance variables that will be sdhared across the game
    public static int lives = 1;
    public static int health = 100;
    public static int maxHealth = 100;
    public static string mainMessage = "Destroy all the turrets";
    public static bool loseGame = false;
    public static bool winGame = false;

    private int enemies;

    private void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemies <= 0)
        {
            Debug.Log("You win");
            if (winGame == false) //this it to prevent the game from calling wingame over and over
            {
            winGame = true;
            Invoke(nameof(Reset), 2f);
            }
        }

        if (health <= 0)
        {
            Debug.Log("You lose");
            health = 0;
            if(loseGame == false)
            {
                loseGame = true;
                Invoke(nameof(Reset), 4f);
            }
        }
    }

    public void Reset()
    {
        lives = 1;
        health = 100;
        maxHealth = 100;
        ProjectileWeapon.ammo = ProjectileWeapon.maxAmmo;
        loseGame = false;
        winGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
