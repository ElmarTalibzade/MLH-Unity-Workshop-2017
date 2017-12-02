using UnityEngine;

// Handles picking up apples
public class AppleItem : MonoBehaviour
{
    // Amount of health it restores
    public float HealthValue = 10;

    // Call this event when it enters other collider
    private void OnTriggerEnter(Collider other)
    {
        // Find collider's health controller
        HealthController health = other.gameObject.GetComponent<HealthController>();

        // If it's not null, this means that the entity collided with an apple is Player
        if (health)
        {
            // Give it health if possible
            if (health.GiveHealth(10))
            {
                // Destroy itself if giving health was successful
                Destroy(this.gameObject);
            }
        }
    }
}