using UnityEngine;

// Handles the movement and rotation of an entity
internal class LocomotionController : MonoBehaviour
{
    public float walkSpeed = 5;         // speed when an entity walks
    public float sprintSpeed = 5;       // speed when an entity runs

    public float lookSpeed = 5;         // speed at which entity rotates to a given direction

    private Vector3 moveDirection;      // current move direction
    private Vector3 finalVelocity;      // calculated velocity of this entity
    private Quaternion _targetRot = new Quaternion();           // calculated final rotation of this entity

    // Assumed that the child object is an animated mesh
    public Transform childObj;          // child object of this entity

    private Rigidbody _body;            // rigidbody component of this entity

    public bool IsRunning;              // returns true if this entity is currently running

    public AnimatorController _anim;        // AnimatorController
    private JumpController _jumpControl;    // Jump component

    public bool CanMove = true;             // Returns true if this entity can move. Overradable by other components

    private float TargetSpeed               // Speed at which this entity should be moving
    {
        get
        {
            if (IsRunning)
            {
                return sprintSpeed * _jumpControl.JumpSpeedFactor;
            }

            return walkSpeed * _jumpControl.JumpSpeedFactor;
        }
    }

    private float CurrentSpeedClamped   // Returns this entity's speed which is clamped between 0 and 1
    {
        get { return Mathf.Clamp(_body.velocity.magnitude, 0, TargetSpeed); }
    }

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _jumpControl = GetComponent<JumpController>();
    }

    private void FixedUpdate()
    {
        // This is what actually moves the object.
        _body.velocity = Vector3.Lerp(_body.velocity, finalVelocity, Time.deltaTime * 5f);
    }

    public void Rotate(Vector2 dir)
    {
        // Get the horizontal and vertical movements from input

        Vector3 movement = new Vector3(dir.x, 0, dir.y);

        // Calculate a final rotation based on inputs that player enters.
        if (movement != Vector3.zero)
        {
            _targetRot = Quaternion.LookRotation(movement);
        }

        // Smoothly change the rotation of the child object
        childObj.rotation = Quaternion.Slerp(childObj.rotation, _targetRot, Time.deltaTime * lookSpeed);
    }

    // Moves the player in a specified direction.
    public void Move(Vector2 DIR)
    {
        // Set the direction in which this entity will move
        moveDirection = new Vector3(DIR.x * TargetSpeed, 0,
            DIR.y * TargetSpeed);

        if (CanMove)        // if this entity can move...
        {
            finalVelocity = moveDirection;      // set its final velocity

            if (_anim != null)
            {
                _anim.SetMovement(CurrentSpeedClamped);         // update entity animation
            }
        }
        else
        {
            finalVelocity = Vector3.zero;           // set the final velocity to 0

            if (_anim != null)
            {
                _anim.SetMovement(0);               // update entity animation to stop moving
            }
        }
    }
}