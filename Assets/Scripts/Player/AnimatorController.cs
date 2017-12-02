using UnityEngine;

[RequireComponent(typeof(Animator))]
// Acts as an abstraction between controllers and animations
public class AnimatorController : MonoBehaviour
{
    // An animator component
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Sets the movement value which will be smoothyl interpolated over time
    public void SetMovement(float val)
    {
        float newVal = Mathf.Lerp(animator.GetFloat("speed_mov"), val, Time.deltaTime * 5);
        animator.SetFloat("speed_mov", newVal);
    }

    // Sets the rotation value which will be smoothyl interpolated over time
    public void SetRotation(float val)
    {
        animator.SetFloat("speed_rot", val);
    }

    // Triggers a jump animation
    public void SetJump()
    {
        animator.SetTrigger("jump");
    }

    // Updates the grounded animation based on a given value
    public void SetGrounded(bool val)
    {
        animator.SetBool("grounded", val);
    }
}