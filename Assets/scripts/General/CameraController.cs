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

    [SerializeField]
    private float defaultMaxDistance;

    [SerializeField]
    private float currentDistance;

    [SerializeField]
    private float aimDistance;

    private float sensitivity;

    public Transform player;

    public bool aim;

    private void Awake()
    {
        instance = this;

        currentDistance = defaultMaxDistance;
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
        float distance = defaultMaxDistance;
        Vector3[] points = GetCameraCollisionPoints(direction);

        foreach (Vector3 point in points)
        {
            if (Physics.Raycast(point, direction, out hit, defaultMaxDistance))
            {
                distance = Mathf.Min((hit.point - follow.position).magnitude, distance);
            }
        }

        transform.position = follow.position + direction * distance;
        transform.rotation = Quaternion.LookRotation(follow.position - transform.position);

        if (!aim)
        {
            defaultMaxDistance = currentDistance;
        }
        else
        {
            defaultMaxDistance = aimDistance;
        }
    }

    private void CalculateNearPlaneSize()
    {
        float height = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.nearClipPlane;
        float width = height * camera.aspect;

        nearPlaneSize = new Vector2(width, height);
    }

    private Vector3[] GetCameraCollisionPoints(Vector3 direction)
    {
        Vector3 position = follow.position;
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

    public void SetAimCamera(bool isAiming)
    {
        aim = isAiming;
    }
}
