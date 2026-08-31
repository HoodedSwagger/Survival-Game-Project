using UnityEngine;

public class TitanfallMovment : MonoBehaviour
{
    [Header("Прочие параметры")]
    CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundLayer;

    Vector3 move;
    Vector3 input;//stores player input
    Vector3 YVelicity;//moves player to ground
    Vector3 forwardDirection;

    //player movement states
     bool isGrounded;
    bool isSprinting;
    bool isCrouching;
    bool isHitTheRoof;

    [Header("Скорость")]
    //speed variables
    [HideInInspector] 
    public float speed { get; set; }
    public float runSpeed = 8;
    public float sprintSpeed  = 12;
    public float crouchSpeed = 4;
    public float airSpeed = 0.3f;

    [Header("Прыжок")]
    public float jumpHeight = 5;

    [Header("Гравитация")]
    float gravity;
    public float normalGravity = -13;

    //crouch variables
    float startHeight;
    float crouchHeight = 0.5f;
    Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    Vector3 standingCenter = new Vector3(0, 0, 0);

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        startHeight = transform.localScale.y;
    }

    private void Update()
    {
        HandleInput();
        if (isGrounded)
        {
            GroundedMovement(); 
        }
        else if(!isGrounded)
        {
            AirMovement();
        } 
        if(input == Vector3.zero)
        {
            isSprinting = false;
        }

        CheckGround();
        controller.Move(move * Time.deltaTime);
        ApplyGravity();
    }

    void HandleInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        input = transform.TransformDirection(input);
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            PlayerJump();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && !isHitTheRoof)
        {
            ExitCrouch();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && isGrounded )
        {
            isSprinting = !isSprinting;
            ExitCrouch();
        }
    }

    void GroundedMovement()
    {
        if (!Input.GetKey(KeyCode.Space))
            YVelicity.y = gravity;

        speed = isSprinting ? sprintSpeed : isCrouching ? crouchSpeed : runSpeed;
        if (input.x != 0)
        {
            move.x += input.x * speed;
        }
        else
        {
            move.x = 0;
        }
        if (input.z != 0)
        {
            move.z += input.z * speed;
        }
        else
        {
            move.z = 0;
        }
        move = Vector3.ClampMagnitude(move, speed);
    }

    void AirMovement()
    {
        move.x += input.x * airSpeed;
        move.z += input.z * airSpeed;

        move = Vector3.ClampMagnitude(move, speed);
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.25f, groundLayer);
    }   

    void ApplyGravity()
    {
        gravity = normalGravity;
        YVelicity.y += gravity * Time.deltaTime;
        controller.Move(YVelicity * Time.deltaTime);
    }    

    void PlayerJump()
    {
        YVelicity.y = Mathf.Sqrt(jumpHeight * -2 * normalGravity);
    }

    void Crouch()
    {
        controller.height = crouchHeight;
        controller.center = crouchingCenter;
        groundCheck.localPosition = new Vector3(0, 0, 0); 

        isCrouching = true;
        isSprinting = false;
    }

    void ExitCrouch()
    {
        groundCheck.localPosition = new Vector3(0, -1, 0);
        controller.height = (startHeight * 2);
        controller.center = standingCenter;
        transform.localScale = new Vector3(transform.localScale.x, startHeight, transform.localScale.z);

        isCrouching = false;
    }
}
