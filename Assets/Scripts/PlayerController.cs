using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    Vector2 moveInput;

    [Header("Jump")]
    public float jumpForce = 7f;
    bool jumpInput;

    [Header("Ground Check")]
    public Transform grdChecker;
    public LayerMask ground;
    public float rayLength = 0.6f;
    bool grounded;

    [Header("Z Limit")]
    public float minZ = -2f;
    public float maxZ = 2f;

    Rigidbody rb;
    RaycastHit groundHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            jumpInput = true;
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move();
        JumpCheck();
        ClampZPosition();
    }

    void GroundCheck()
    {
        grounded = Physics.Raycast(
            grdChecker.position,
            Vector3.down,
            out groundHit,
            rayLength,
            ground
        );

        Debug.DrawRay(grdChecker.position, Vector3.down * rayLength, Color.red);
    }

    void Move()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (grounded)
        {
            // Gerak mengikuti slope terrain
            Vector3 slopeDir = Vector3.ProjectOnPlane(inputDir, groundHit.normal);
            rb.velocity = new Vector3(
                slopeDir.x * moveSpeed,
                rb.velocity.y,
                slopeDir.z * moveSpeed
            );
        }
        else
        {
            rb.velocity = new Vector3(
                inputDir.x * moveSpeed,
                rb.velocity.y,
                inputDir.z * moveSpeed
            );
        }
    }

    void JumpCheck()
    {
        if (jumpInput && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpInput = false;
        }
    }

    void ClampZPosition()
    {
        Vector3 pos = rb.position;
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        rb.position = pos;
    }
}
