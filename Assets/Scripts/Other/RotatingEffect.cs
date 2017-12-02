using UnityEngine;

// Used to rotate a said GameObject in a given direction
public class RotatingEffect : MonoBehaviour
{
    public float Speed = 10f;       // rotation speed
    public Vector3 Directions;      // Directions in which it will rotate

    private void Update()
    {
        // Smoothyl rotate the GameObject 
        transform.Rotate(Directions, Speed * Time.deltaTime);
    }
}