using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Animation")]
    public Animator animator;

    [Header("Movement")]
    public InputActionReference move;
    public float moveSpeed = 4f;
    public float joshGravity = 35f;

    //this rigidbody rb
    Rigidbody rb;

    //cinemacam reference (set from playerSetup)
    [Header("CinemaCam (autoset)")]
    public Transform cam;

    [Header("Rotation")]
    public float rotationSpeed = 14f;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //dont tip the fuck over
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        if (transform.GetComponent<PlayerManager>().inGame)
        {
            movePlayer();
            limitVelocity();
            updateRotation(); //rotate based on where i look
            UpdateAnimation();
        }
    }

    private void movePlayer()
    {
        Vector2 moveVector = move.action.ReadValue<Vector2>();

        Vector3 moveDirFoward = cam.forward.normalized * moveVector.y;
        Vector3 moveDirSideways = cam.right.normalized * moveVector.x;
        Vector3 moveDirection = moveDirFoward + moveDirSideways;
        moveDirection.y = 0; //so i dont fly man

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.VelocityChange);

        //extra gravity maybe i found out why later
        rb.AddForce(Vector3.down * joshGravity, ForceMode.Acceleration);
    }

    void UpdateAnimation()
    {
        if(rb.linearVelocity.magnitude > 0.1f)
        {
            animator.Play("running");
        }
        else
        {
            animator.Play("Idle");
        }
    }

    private void limitVelocity()
    {
        Vector3 velocityVector = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (velocityVector.magnitude > moveSpeed)
        {
            Vector3 newVelocityVector = velocityVector.normalized * moveSpeed;
            //set second paramter below to 0 if wierd things happen
            rb.linearVelocity = new Vector3(newVelocityVector.x, rb.linearVelocity.y, newVelocityVector.z);
        }
    }

    private void updateRotation()
    {
        Vector3 lookForward = cam.forward;

        lookForward.y = Mathf.Clamp(lookForward.y, -.3f, .3f);

        Vector3 horizontalForward = new Vector3(lookForward.x, 0f, lookForward.z).normalized;

        Vector3 finalForward = horizontalForward * Mathf.Sqrt(1 - lookForward.y * lookForward.y) + Vector3.up * lookForward.y;

        if (lookForward.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(finalForward), rotationSpeed * Time.deltaTime);
        }
    }
}
