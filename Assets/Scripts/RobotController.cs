using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LocomotionController))]
[RequireComponent(typeof(NavMeshAgent))]

// Handles the Robot AI
public class RobotController : MonoBehaviour
{
    public Transform FollowTarget;              // target which it will follow and attack
    private NavMeshAgent _agent;                // NavMeshAgent of this component
    private LocomotionController _controller;   // Robot's controller class
    private HealthController targetHealth;      // Target's HealthController class.

    public float MaxFollowDistance = 15;        // Specifies how far away should the target be to stop pursuing it

    public float DamageAmount = 5;              // Specifies how much damage will this robot deal to its target

    // Returns the distance which remains between itself and target
    private float RemainingDistance
    {
        get { return (FollowTarget.position - transform.position).magnitude; }
    }

    // Finds components and attaches to them. Basically, setting up. Better instead of manually attaching each component
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.isStopped = true;
        _controller = GetComponent<LocomotionController>();

        targetHealth = FollowTarget.gameObject.GetComponent<HealthController>();

        path = new NavMeshPath();
    }

    private NavMeshPath path;               // the path this robot will take (being recalculated every frame)
    private Vector3 moveDir;                // direction in which this robot currently moves

    public float AttackCooldown = 5;        // cooldown (in seconds) between attacks
    private float currentCooldown = 0;      // current cooldown (updates every frame) 

    private bool IsTargeting                // Returns true if this robot is currently in pursue of a target
    {
        get
        {
            if (TooFar())
            {
                return false;
            }
            else if (DestinationReached())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    // Returns true if this robot can attack its target
    private bool CanAttack
    {
        get { return DestinationReached() && currentCooldown >= AttackCooldown; }
    }

    // Returns true if this robot is too far away from its target
    private bool TooFar()
    {
        return RemainingDistance >= MaxFollowDistance;
    }

    // Returns true if this robot has reached its target and is within its range
    private bool DestinationReached()
    {
        return RemainingDistance < _agent.stoppingDistance;
    }

    // Executed each frame
    private void Update()
    {
        _controller.CanMove = (IsTargeting);            // robot can move unless it's not pursing its target

        // calculate a path base don target's current position
        _agent.CalculatePath(new Vector3(FollowTarget.position.x, 0, FollowTarget.position.z), path);

        if (path.corners.Length == 1)           // if robot can walk up to its target with no obstacles to avoid
        {
            // set its move direction towards its target
            moveDir = (FollowTarget.position - transform.position).normalized;          
        }
        else if (path.corners.Length > 1)       // if robot has not pass at least one abstracle
        {
            // set its move direction towards next path corner
            moveDir = (path.corners[1] - transform.position).normalized;
        }

        // move and rotate robot based on directions calculated
        _controller.Move(new Vector2(moveDir.x, moveDir.z));
        _controller.Rotate(new Vector2(moveDir.x, moveDir.z));

        AttackTarget();     // attacks target if within its reach
    }

    private void AttackTarget()
    {
        if (CanAttack)  
        {
            targetHealth.TakeDamage(DamageAmount);          // deal damage to its target if within its reach
            currentCooldown = 0;                            // reset cooldown to 0
        }
        else
        {
            if (currentCooldown < AttackCooldown)           // if attack is on cooldown...
            {
                currentCooldown += Time.deltaTime;          // ...then increase it
            }
        }
    }

    // This is used entirely for debugging purposes only. Useful if you want to see which path will this robot take.
    // Only visible if Robot is currently pursuing its target
    private void OnDrawGizmos()
    {
        if (_controller == null || path == null)
            return;

        if (!IsTargeting)
            return;

        for (int i = 0; i < (path.corners.Length - 1); i++)
        {
            if (i == 0)
            {
                Gizmos.color = Color.red;
            }
            else if (i == path.corners.Length - 1)
            {
                Gizmos.color = Color.magenta;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }

            Gizmos.DrawSphere(path.corners[i], 0.05f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
        }

        if (path.corners.Length > 1)
        {
            Vector3 startPos = new Vector3(transform.position.x, 1.5f, transform.position.z);
            Vector3 endPos = startPos + (FollowTarget.position - path.corners[0]);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPos, endPos);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 10);
        }
    }
}