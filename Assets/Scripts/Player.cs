using UnityEngine;
using Mirror;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Player : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float strafeDistance = 2f;
    [SerializeField] private float confusedTime = 3f;
    [SerializeField] private float movementSpeed = 5f;
    private int confuseAmount = 0;

    [Header("Components")]
    [SerializeField] private FollowCamera cam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    void Awake()
    {
        cam = Instantiate(cam);
        cam.Host = transform;
        animator.speed = movementSpeed /5f;
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
        animator.SetFloat("vertical", verticalSpeed);
        animator.SetFloat("horizontal", horziontalSpeed);

        Vector3 move = (horziontalSpeed * cam.transform.right) + (verticalSpeed * cam.transform.forward);
        Vector3 velocity = Vector3.ClampMagnitude(move, 1) * movementSpeed;
        if (velocity != Vector3.zero)
        {
            transform.forward = cam.transform.forward;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }
}
