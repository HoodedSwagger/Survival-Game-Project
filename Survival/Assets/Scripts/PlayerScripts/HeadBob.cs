using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private bool isEnabled = true;

    [SerializeField] private Transform playerCamera;

    [SerializeField] private float bobSpeed = 14f;
    [SerializeField] private float bobAmount = 0.5f;
    private CharacterController controller;
    private float defaultYPos = 0f;
    private float timer;

    private void Awake()
    {
        defaultYPos = playerCamera.transform.localPosition.y;
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (isEnabled)
        {
            HandleHeadBob();
        }
    }

    private void HandleHeadBob()
    {
        if (!controller.isGrounded) return;

        if (controller.velocity.x != 0 || controller.velocity.y != 0)
        {
            timer += Time.deltaTime * (bobSpeed);

            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * bobAmount, playerCamera.transform.localPosition.z);
        }
    }
}
