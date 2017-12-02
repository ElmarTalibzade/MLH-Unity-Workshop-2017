using UnityEngine;

// Handles the pickup of a Key
public class KeyItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        KeyCollector key = other.gameObject.GetComponent<KeyCollector>();

        if (key)
        {
            if (key.GiveKey())
            {
                OnKeyPickup(this);
                Destroy(this.gameObject);
            }
        }
    }

    public delegate void OnKeyPickupEvent(KeyItem key);

    public event OnKeyPickupEvent KeyPickup;

    protected void OnKeyPickup(KeyItem key)
    {
        if (KeyPickup != null)
        {
            KeyPickup.Invoke(key);
        }
    }
}