using UnityEngine;
using Mirror;

[RequireComponent(typeof(Camera))]
public class FollowCamera :MonoBehaviour
{
    public Transform Host { get; set; }
    [Header("Movement")]
    [SerializeField] private Vector3 localPivotPoint;
    [SerializeField] private float distanceToHost = 5f;

    [Header("View")]
    [SerializeField] private float verticalArc = 85f;
    [SerializeField] private float mouseSensitivity = 50f;

    private Vector3 camRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera.SetupCurrent(GetComponent<Camera>());
    }

    private void LateUpdate()
    {
        if (Host == null) return;

        camRotation.y += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        camRotation.x += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        camRotation.x = Mathf.Clamp(camRotation.x, -verticalArc, verticalArc);
        transform.localEulerAngles = new(360 - camRotation.x, camRotation.y, 0);

        Vector3 pivotPosition = Host.position + localPivotPoint;
        Vector3 boundPosition = pivotPosition - transform.forward * distanceToHost;
        transform.position = boundPosition;
        transform.LookAt(pivotPosition);

    }
}
