using UnityEngine;

// This basically tells Unity that this script requires a different component in order to operate correctly. 
// A required component is automatically added to GameObject if such script is attached to it
[RequireComponent(typeof(LocomotionController))]

// This script handles user's input and controls the player
public class PlayerController : MonoBehaviour
{
    private LocomotionController _controller;
    private JumpController _jumpController;

    private void Awake()
    {
        _controller = GetComponent<LocomotionController>();
        _jumpController = GetComponent<JumpController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpController.Jump();             // Make the player jump when "Space" is pressed
        }

        _controller.IsRunning = Input.GetKey(KeyCode.LeftShift);            // Make player run when "Left Shift" is being held

        // Get the movement direction from WASD and Arrow keys
        Vector2 MoveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _controller.Move(MoveDir);              // move player in a specified direction
        _controller.Rotate(MoveDir);            // make player look in a specified direction
    }
}