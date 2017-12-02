using UnityEngine;

// This attribute tells Unity that this component requires a Camera component to be present.
[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    // Uncheck this if you don't want Camera to track a Transform target.
    public bool IsTracking = true;

    // Transform of a GameObject which a Camera will track.
    public Transform Target;

    // Determines how smooth the camera will transition to a new position
    public float SmoothValue = 2.5f;

    // Affects the final position of a camera
    public Vector3 CameraOffset = new Vector3(0, 10, -10);

    // Final position which is calculated at the end of this frame, this value is what camera is going to lerp to
    private Vector3 _targetPos;

    /*
     * LateUpdate() is called right after the Update().
     * Cases where code affects the position/rotation of a Camera should always be implemented in
     * LateUpdate() because it tracks objects that might have moved inside Update().
    */

    private void LateUpdate()
    {
        // An elegant way to say that Camera should not be affected if IsTracking is set to false.
        if (!IsTracking) return;

        // We determine a new Camera position by taking target's position and adding offset on top of it.
        _targetPos = new Vector3(
            Target.position.x + CameraOffset.x,
            CameraOffset.y,
            Target.position.z + CameraOffset.z
        );

        // We smoothly change Camera's position to a position we calculated earlier.
        transform.position = Vector3.Lerp(
            transform.position,
            _targetPos,
            Time.deltaTime * SmoothValue
        );
    }
}