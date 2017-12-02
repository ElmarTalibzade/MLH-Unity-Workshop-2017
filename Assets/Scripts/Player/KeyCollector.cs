using UnityEngine;

// Handles the collection of keys and unlocking doors
public class KeyCollector : MonoBehaviour
{
    public bool HasKey = false;

    // Gives a key to this entity. Returns false if this entity already has a key
    public bool GiveKey()
    {
        if (!HasKey)
        {
            HasKey = true;
            return true;
        }

        return false;
    }

    // Unlocks a door. Returns false if it was unable to unlock a door
    public bool UnlockDoor()
    {
        if (HasKey)
        {
            HasKey = false;
            return true;
        }

        return false;
    }
}