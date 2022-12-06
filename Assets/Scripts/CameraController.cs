using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0f)] private float sensitivityDefault = 240f;
    [SerializeField, Min(0f)] private float sensitivityAim = 120f;
    [SerializeField, Min(1f)] private float defaultFOV = 45f;
    [SerializeField, Min(1f)] private float zoomedFOV = 25;

    [Header("Prefab Parts")]
    [SerializeField] private Camera cameraFPS;

    private Rigidbody rb;

    private float sensitivityCurrent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        sensitivityCurrent = sensitivityDefault;
        cameraFPS.fieldOfView = defaultFOV;
    }

    private void Update()
    {
        UpdateRotation();
    }

    public void SetDefaultSensitivity()
    {
        sensitivityCurrent = sensitivityDefault;
    }

    public void SetAimSensitivity()
    {

        sensitivityCurrent = sensitivityAim;
    }

    public void ZoomInCamera()
    {
        cameraFPS.fieldOfView = defaultFOV;
    }

    public void ZoomOutCamera()
    {
        cameraFPS.fieldOfView = zoomedFOV;
    }

    private void UpdateRotation()
    {
        // CAMERA GE TURNED UPSIDE-DOWN SOMETIMES
        var movementHorizontal = Input.GetAxis("Mouse X");
        var movementVertical = Input.GetAxis("Mouse Y");

        var rotationHorizontal = movementHorizontal * sensitivityCurrent * Time.deltaTime * transform.up;
        var rotationVertical = -movementVertical * sensitivityCurrent * Time.deltaTime;
        rotationVertical = Mathf.Clamp(rotationVertical, -45f, 45f);
        var rotationVerticalNew = Quaternion.Euler(cameraFPS.transform.rotation.eulerAngles + new Vector3(rotationVertical, 0f, 0f));

        // order is important!
        cameraFPS.transform.rotation = rotationVerticalNew;
        transform.Rotate(rotationHorizontal);
    }
}
