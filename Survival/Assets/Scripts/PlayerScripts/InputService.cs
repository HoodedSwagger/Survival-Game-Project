using UnityEngine;

public class InputService : MonoBehaviour
{
    private Vector3 movementDirection;
    public static Vector3 MovementDirection;

    private Vector2 mouseDelta;
    public static Vector2 MouseDelta;

     private bool jumpPressed;
    public static bool JumpPressed;

    private bool LMBPressed;
    public static bool _LMBPressed;

    private bool interactPressed;
    public static bool InteractPressed;

    private bool itemRemovePressed;
    public static bool ItemDropPressed;

    private bool crouchButtonPressed;
    public static bool CrouchButtonPressed;

    private bool sprintButtonPressed;
    public static bool SprintButtonPressed;

    private float mouseScrollY;
    public static float MouseScrollY;

    private bool pauseButtonPresssed;
    public static bool PauseButtonPressed;

    private bool craftPanelButtonPressed;
    public static bool CraftPanelButtonPressed;

    private void Update()
    {
        movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));
        MovementDirection = movementDirection;

        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        JumpPressed = jumpPressed;

        LMBPressed = Input.GetButtonDown("Fire1");
        _LMBPressed = LMBPressed;

        interactPressed = Input.GetKeyDown(KeyCode.E);
        InteractPressed = interactPressed;

        itemRemovePressed = Input.GetKeyDown(KeyCode.Q);
        ItemDropPressed = itemRemovePressed;

        crouchButtonPressed = Input.GetKeyDown(KeyCode.LeftControl);
        CrouchButtonPressed = crouchButtonPressed;

        sprintButtonPressed = Input.GetKeyDown(KeyCode.LeftShift);
        SprintButtonPressed = sprintButtonPressed;

        mouseScrollY = Input.mouseScrollDelta.y;
        MouseScrollY = mouseScrollY;

        mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        MouseDelta = mouseDelta;

        pauseButtonPresssed = Input.GetKeyDown(KeyCode.Escape);
        PauseButtonPressed = pauseButtonPresssed;

        craftPanelButtonPressed = Input.GetKeyDown(KeyCode.Tab);
        CraftPanelButtonPressed = craftPanelButtonPressed;
    }
}
