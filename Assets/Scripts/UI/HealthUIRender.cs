using UnityEngine;
using UnityEngine.UI;

// Used to render the Health UI of the player.
public class HealthUIRender : MonoBehaviour
{
    public Text HealthLabel;            // Label text of Health
    public Text ArmorLabel;             // Label text of Armor

    public HealthController _health;    // HealthController of the player

    // Called when the game starts
    private void Awake()
    {
        // Subscribe to player's health and armor changes events
        _health.HealthChanged += SetHealthText;
        _health.ArmorChanged += SetArmorText;
    }

    // Updates player's Armor UI
    private void SetHealthText(float amount)
    {
        HealthLabel.text = string.Format("Health: {0}", (int)amount);
    }

    // Updates player's Armor UI
    private void SetArmorText(float amount)
    {
        ArmorLabel.text = string.Format("Armor: {0}", (int)amount);
    }
}