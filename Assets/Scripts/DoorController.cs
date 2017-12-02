using UnityEngine;

// Used to control doors
public class DoorController : MonoBehaviour
{
    public bool IsLocked = true;        // returns true if this door is locked

    private Rigidbody _body;            // rigidbody of this door

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    // Gets executed when the door opens
    private void OnTriggerEnter(Collider other)
    {
        if (IsLocked)
        {
            // if the door is locked, check if collided entity has a KeyCollector
            KeyCollector _collector = other.gameObject.GetComponent<KeyCollector>();

            if (_collector)
            {
                // try unlocking the door
                if (_collector.UnlockDoor())
                {
                    UnlockDoor();       // if that entity has a key then open the door
                }
            }
        }
    }

    // Unlocks the door
    private void UnlockDoor()
    {
        IsLocked = false;       // indicate that the door is now unlocked
        // Make the door non-kinematic, allowing entities to pass through
        _body.isKinematic = false;
    }
}