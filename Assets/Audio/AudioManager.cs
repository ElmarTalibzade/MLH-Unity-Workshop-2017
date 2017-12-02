using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages all audio in the game. The way it works is that it subscribes to objects and plays sounds when necessary.
public class AudioManager : MonoBehaviour
{
    private AudioSource audio;                  // Audio Source component

    public AudioClip keyPickup;                 // Audio clip which will be played when key is picked up

    private KeyItem[] keys;                     // Used to store all keys that exist within scene

    void Awake()
    {
        audio = GetComponent<AudioSource>();    // Finds an Audio Source
        keys = FindObjectsOfType<KeyItem>();    // Finds all keys in the scene. Avoid using this method often as large scene will make such operations slower

        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].KeyPickup += OnKeyPickup;   // Subscribe to the pickup event of all keys
        }
    }

    private void OnKeyPickup(KeyItem item)
    {
        item.KeyPickup -= OnKeyPickup;          // You must always unsubscribe from methods whose objects are about to be destroyed
        audio.PlayOneShot(keyPickup);           // Play an audio clip of pickup sound
    }
}