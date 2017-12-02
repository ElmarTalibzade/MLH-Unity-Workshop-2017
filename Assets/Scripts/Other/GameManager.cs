using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the whole game. In this case, it simply restarts the scene when player dies.
public class GameManager : MonoBehaviour
{
    public HealthController PlayerHealth;           // Player's health

    private void Awake()
    {
        PlayerHealth.EntityDead += OnPlayerDead;    // Subscribe to player's OnDeadEvent
    }

    // This code will be called once player dies
    private void OnPlayerDead(HealthController player)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);       // Reload a current scene
    }
}