//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField]
    private Vector2 StartAngle = new Vector2(90 * Mathf.Deg2Rad, 0);

    private new Camera camera;
    private Vector2 nearPlaneSize;

    public Transform follow;
    public Transform crouchFollow;

    public Transform currentFollow;

    [SerializeField]
    private float defaultMaxDistance;

    [SerializeField]
    private float currentDistance;

    [SerializeField]
    private float aimDistance;

    [SerializeField]
    private float crouchDistance;

    [SerializeField]
    private float sprintDistance;

    private float sensitivity;

    public Transform player;

    private bool aim;

    private bool _sprint;

    private bool _crouch;

    private void Awake()
    {
        instance = this;

        currentDistance = defaultMaxDistance;

        currentFollow = follow;
    }

    void Start()
    {
        ChangeSensitivity();

        camera = GetComponent<Camera>();

        CalculateNearPlaneSize();
    }

    private void ChangeSensitivity()
    {
        sensitivity = PlayerPrefs.GetFloat("sensitivity");
        if (sensitivity <= 0)
            sensitivity = 1;
    }

    public void SetAngle(int newAngle)
    {
        StartAngle = new Vector2(newAngle * Mathf.Deg2Rad, 0);
    }



    void Update()
    {
        float horizontalCam = Input.GetAxis("Mouse X");

        if (_crouch)
        {
            Vector3.Lerp(follow.position, crouchFollow.position, 5);
            currentFollow = crouchFollow;
        }
        else
        {
            Vector3.Lerp( crouchFollow.position, follow.position, 5);
            currentFollow = follow;
        }

        if (horizontalCam != 0)
        {
            StartAngle.x += horizontalCam * Mathf.Deg2Rad * sensitivity;

            player.forward = transform.forward;
            player.localRotation = Quaternion.Euler(0, player.localEulerAngles.y, 0);
        }

        float verticalCam = Input.GetAxis("Mouse Y");

        if (verticalCam != 0)
        {
            StartAngle.y += verticalCam * Mathf.Deg2Rad * sensitivity;
            StartAngle.y = Mathf.Clamp(StartAngle.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);
        }
    }

    void LateUpdate()
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(StartAngle.x) * Mathf.Cos(StartAngle.y),
            -Mathf.Sin(StartAngle.y),
            -Mathf.Sin(StartAngle.x) * Mathf.Cos(StartAngle.y));

        RaycastHit hit;
        float distance = currentDistance;
        Vector3[] points = GetCameraCollisionPoints(direction);

        foreach (Vector3 point in points)
        {
            if (Physics.Raycast(point, direction, out hit, currentDistance))
                distance = Mathf.Min((hit.point - currentFollow.position).magnitude, distance);
        }

        transform.position = currentFollow.position + direction * distance;
        transform.rotation = Quaternion.LookRotation(currentFollow.position - transform.position);


        if (aim)
            currentDistance = aimDistance;
        else if(_sprint)
            currentDistance = sprintDistance;
        else if(_crouch)
            currentDistance = crouchDistance;
        else if(!_sprint || !aim)
            currentDistance = defaultMaxDistance;
    }

    private void CalculateNearPlaneSize()
    {
        float height = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.nearClipPlane;
        float width = height * camera.aspect;

        nearPlaneSize = new Vector2(width, height);
    }

    private Vector3[] GetCameraCollisionPoints(Vector3 direction)
    {
        Vector3 position = crouchFollow.position;

        Vector3 center = position + direction * (camera.nearClipPlane + 0.2f);

        Vector3 right = transform.right * nearPlaneSize.x;
        Vector3 up = transform.up * nearPlaneSize.y;

        return new Vector3[]
        {
            center - right + up,
            center + right + up,
            center - right - up,
            center + right - up
        };
    }

    public void SetAimCamera(bool isAiming) => aim = isAiming;

    public void SetSprintCamera(bool isSprinting) => _sprint = isSprinting;

    public void SetCrouchCamera(bool isCrouching) => _crouch = isCrouching;
}