using UnityEngine;

public class JumpController : MonoBehaviour
{
    public float jumpForce = 2.5f;          // specifies the jump strength

    // specifies how much will this speed be affected while airborne
    public float AirSpeedFactor = 0.2f;

    // used by other controllers to have its speed affected by jumping
    public float JumpSpeedFactor
    {
        get { return IsGrounded() ? 1 : AirSpeedFactor; }
    }

    private Rigidbody _body;            // rigidbody of this entity
    private Collider _collider;         // collider of this entity

    public AnimatorController _anim;    // handles animation

    public bool grounded;       // returns true if the entity is grounded


    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        //grounded = IsGrounded();
        _anim.SetGrounded(IsGrounded());
    }

    // Makes this instance jump
    public void Jump()
    {
        if (IsGrounded())
        {
            _body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _anim.SetJump();
        }
    }

    // how long is the trigger which checks if entity is grounded or not. the longer the ray the earlier this entity becomes "grounded"
    public float isGroundedRayLength = 0.1f;

    public bool IsGrounded()            // checks if thsi entity is grounded
    {
        Vector3 originPos = transform.position;
        originPos.y = _collider.bounds.min.y + 0.1f;         // draws a raycast from the collider of this entity
        float rayLength = isGroundedRayLength + 0.1f;

        bool v = Physics.Raycast(originPos, Vector3.down, rayLength);

        return v;
    }
}