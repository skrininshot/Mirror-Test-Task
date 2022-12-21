using UnityEngine;
using Mirror;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Player : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] [Range(1,5)] private float movementSpeed = 3f;
    [SerializeField] private float strafeDistance = 2f;
    [SerializeField] private float confusedTime = 3f;
    private int confuseAmount = 0;

    [Header("Components")]
    [SerializeField] private FollowCamera cam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    private Transform rotatingOrigin;

    void Awake()
    {
        cam = Instantiate(cam);
        cam.Host = transform;
        rotatingOrigin = Instantiate(new GameObject()).transform;
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        Move();
    }

    private void Move()
    {
        float horziontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");
        animator.SetFloat("speed", verticalSpeed != 0 ? verticalSpeed : Mathf.Abs(horziontalSpeed));

        Vector3 move = (horziontalSpeed * cam.transform.right) + (verticalSpeed * cam.transform.forward);
        Vector3 velocity = Vector3.ClampMagnitude(move, 1) * movementSpeed;
        if (velocity != Vector3.zero)
        {
            rotatingOrigin.transform.forward = velocity;
            transform.localEulerAngles = new Vector3(0, rotatingOrigin.transform.localEulerAngles.y);
        }
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }
}
