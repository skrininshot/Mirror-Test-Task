using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private SkinnedMeshRenderer meshBody;
    [SerializeField] private SkinnedMeshRenderer meshHelmet;

    [Header("Confusing")]
    [SerializeField] private Material confusedMaterial;
    [SerializeField] private float confusingDuration;
    private int confusesAmount = 0;
    private Material defaultMaterial;
    private bool isConfused;

    void Start()
    {
        animator.speed = movementSpeed / 6f;
        defaultMaterial = meshBody.material;

        if (!isLocalPlayer) return;
        
        cam = Instantiate(cam);
        cam.Host = transform;
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

    private void Strafe()
    {
        rb.velocity = Vector3.ClampMagnitude(strafingPoint - transform.position, strafeDistance) * strafingSpeed;

        if (Mathf.Abs(rb.velocity.magnitude - previousFrameMagnitude) < 0.2f)
        {
            isStrafing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isStrafing) return;

        Player another = collision.gameObject.GetComponent<Player>();
        if (another == null) return;
        ConfuseAnotherPlayer(another);
    }

    [Command]
    private void ConfuseAnotherPlayer(Player player)
    {
        player.GetConfused(this);
    }

    [ClientRpc]
    public void GetConfused(Player strafingPlayer)
    {
        if (isConfused) return;

        StartCoroutine(Confused());
        SendConfuseCuccess(strafingPlayer);
    }

    private IEnumerator Confused()
    {
        isConfused = true;
        meshBody.material = confusedMaterial;
        meshHelmet.material = confusedMaterial;

        yield return new WaitForSeconds(confusingDuration);

        isConfused = false;
        meshBody.material = defaultMaterial;
        meshHelmet.material = defaultMaterial;
    }

    public void SendConfuseCuccess(Player strafingPlayer)
    {
        strafingPlayer.ConfuseSuccess();
    }

    [ClientRpc]
    public void ConfuseSuccess()
    {
        confusesAmount++;
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

    private void SetForwardByCamera()
    {
        transform.forward = Vector3.forward;
        transform.localEulerAngles = new Vector3(0, cam.transform.localEulerAngles.y, 0);
    }
}