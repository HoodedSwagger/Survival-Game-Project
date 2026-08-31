using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public float Sensitivity;
    public Transform CameraRotationPoint;
    public bool canRotate = true;

    float rotY = 0f;
    float rotX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Time.timeScale == 1 && canRotate)
        {
            rotY += InputService.MouseDelta.x * Sensitivity;
            rotX += InputService.MouseDelta.y * Sensitivity;

            rotX = Mathf.Clamp(rotX, -90, 90);

            transform.localEulerAngles = new Vector3(0, rotY, 0);
            CameraRotationPoint.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
        }
    }
    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
