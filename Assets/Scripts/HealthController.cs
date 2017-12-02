using UnityEngine;

public class HealthController : MonoBehaviour
{
    // Returns true if this entity is alive
    public bool IsAlive
    {
        get { return Health > 0; }
    }

    [Header("Health")]
    public float Health = 100;          // Current health
    public float MaxHealth = 100;       // Max Health

    [Header("Armor")] public float Armor = 0;       // Current armor
    public float MaxArmor = 100;                    // Max Armor

    // Specifies how much damage does it block. E.g, if real damage is 10 and ArmorBlockFactor is 0.5, then resultant damage will be 5.
    public float ArmorBlockFactor = 0.85f;

    // Make this entity take damage. This is where it decides whether or not reduce armor or Health
    public void TakeDamage(float amount)
    {
        float newDamage = DamageArmor(amount);          // calculate the leftover damage
        DamageHealth(newDamage);                        // reduce players' health with leftover damage
    }

    private void Start()
    {
        // Call these events in order to update the UI right when the game starts.
        OnHealthChanged(Health);
        OnArmorChanged(Armor);
    }

    // Grants health to an entity. Returns false if its health is full
    public bool GiveHealth(float amount)
    {
        if (Health < MaxHealth)                     // if the health is less than its max health
        {
            float nHealth = Health + amount;

            if (nHealth > MaxHealth)                // if the new health is higher than max health
            {
                Health = MaxHealth;                 // set health to max health to avoid overheal
            }
            else
            {
                Health = nHealth;                   // update entity health
            }

            OnHealthChanged(Health);                // tell subscribers that health has changed

            return true;
        }
        else            
        {
            // return false to say that health was not needed. 
            // this is useful in cases where healthpacks will not be destroyed if entity has full health
            return false;                           
        }
    }

    // Damages armor. Returns the damage which is left after armor has been withstood damage
    private float DamageArmor(float amount)
    {
        // Damage which wil be dealt to the player's health after block factor is done
        float leftOverDamage = amount;

        if (Armor > 0)          // if armor is not broken
        {
            float nArmor = Armor;       // place current armor amount to temp variable
            nArmor -= amount;           // deduct amount from that armor

            if (nArmor >= 0)                    // if recent damages did not break the armor
            {
                Armor = nArmor;                 // update the actual Armor amount
                leftOverDamage = amount - (amount * ArmorBlockFactor);          // calculate the leftover damage
            }   
            else if (nArmor < 0)                // if the new amount is less than 0
            {
                Armor = 0;                      // set armor to 0 to avoid negative numbers
            }

            OnArmorChanged(Armor);          // call the event to make subscribers aware of changes
        }

        // If armor was broken then full amount of damage is give to player
        return leftOverDamage;
    }

    // Damages the health of the entity by a given amount
    private void DamageHealth(float amount)
    {
        float nHealth = Health - amount;        // stores the new health amount in a temp variable          

        if (nHealth > 0)                        // if entity is still alive after the damage given
        {
            Health = nHealth;                   // update the health
            OnHealthChanged(Health);            // call the event to make subscribers aware of changes
        }
        else                                    // if entity doesn't survive the damage
        {
            Health = 0;                         // set health to 0 in to avoid negative amount

            OnHealthChanged(Health);
            OnEntityDead(this);                 // tell subscribers that this entity just died
        }
    }

    // These are used to handle various events like health changes
    public delegate void EntityDeadEvent(HealthController controller);
    public delegate void HealthChangedEvent(float newHealth);
    public delegate void ArmorChangedEvent(float newArmor);
    public delegate void DamageTakenEvent(float amount);

    public event EntityDeadEvent EntityDead;
    public event HealthChangedEvent HealthChanged;
    public event ArmorChangedEvent ArmorChanged;

    private void OnEntityDead(HealthController controller)
    {
        if (EntityDead != null)
        {
            EntityDead.Invoke(controller);
        }
    }

    private void OnHealthChanged(float newHealth)
    {
        if (HealthChanged != null)
        {
            HealthChanged.Invoke(newHealth);
        }
    }

    private void OnArmorChanged(float newArmor)
    {
        if (ArmorChanged != null)
        {
            ArmorChanged.Invoke(newArmor);
        }
    }
}