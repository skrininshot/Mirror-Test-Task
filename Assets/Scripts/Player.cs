using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Player : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    private float previousFrameMagnitude;

    [Header("Strafe")]
    [SerializeField] private float strafeDistance;
    [SerializeField] private float strafingSpeed;
    private Vector3 strafingPoint;
    private bool isStrafing;

    [Header("Components")]
    [SerializeField] private FollowCamera cam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    void Start()
    {
        if (!isLocalPlayer) return;

        cam = Instantiate(cam);
        cam.Host = transform;
        animator.speed = movementSpeed / 6f;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        
        if (isStrafing)
        {
            Strafe();
        }
        else
        {
            Move();
            if (Input.GetMouseButtonDown(0))
            {
                isStrafing = true;
                SetForwardByCamera();
                strafingPoint = transform.position + transform.forward * strafeDistance;
            }
        }
        previousFrameMagnitude = rb.velocity.magnitude;
    }

    private void Move()
    {
        float horziontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");
        animator.SetFloat("vertical", verticalSpeed);
        animator.SetFloat("horizontal", horziontalSpeed);

        if (horziontalSpeed + verticalSpeed != 0)
        {
            SetForwardByCamera();
        }

        Vector3 move = (horziontalSpeed * transform.right) + (verticalSpeed * transform.forward);
        Vector3 velocity = Vector3.ClampMagnitude(move, 1) * movementSpeed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    private void Strafe()
    {
        rb.velocity = Vector3.ClampMagnitude(strafingPoint - transform.position, strafeDistance) * strafingSpeed;

        if (Mathf.Abs(rb.velocity.magnitude - previousFrameMagnitude) < 0.2f)
        {
            isStrafing = false;
        }
    }

    private void SetForwardByCamera()
    {
        transform.forward = Vector3.forward;
        transform.localEulerAngles = new Vector3(0, cam.transform.localEulerAngles.y, 0);
    }
}
