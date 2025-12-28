using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 12f;
    public float deceleration = 16f;
    Vector2 moveInput;
    
    // --- [BARU] Variable untuk arah hadap ---
    private bool isFacingRight = true; 

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

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Mengambil input (Nilai -1 s/d 1)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            jumpInput = true;

        // --- [BARU] Logika Animasi Lari ---
        // Jika ada input gerakan (x atau y tidak 0), set isRunning true
        if (moveInput.sqrMagnitude > 0.1f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // --- [BARU] Logika Membalik Badan (Flip) ---
        // Jika gerak ke kanan (x > 0) dan sedang tidak menghadap kanan -> Flip
        if (moveInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        // Jika gerak ke kiri (x < 0) dan sedang menghadap kanan -> Flip
        else if (moveInput.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
        SmoothMove();
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
    }

    void SmoothMove()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        Vector3 targetVelocity;

        if (grounded)
        {
            Vector3 slopeDir = Vector3.ProjectOnPlane(inputDir, groundHit.normal);
            targetVelocity = slopeDir * moveSpeed;
        }
        else
        {
            targetVelocity = inputDir * (moveSpeed * 0.6f);
        }

        Vector3 currentVelocity = rb.velocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

        float accel = inputDir.magnitude > 0 ? acceleration : deceleration;

        Vector3 smoothVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetVelocity,
            accel * Time.fixedDeltaTime
        );

        rb.velocity = new Vector3(
            smoothVelocity.x,
            currentVelocity.y,
            smoothVelocity.z
        );
    }

    void JumpCheck()
    {
        if (jumpInput && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpInput = false;
            // Opsional: Tambahkan trigger animasi lompat disini jika ada
            // animator.SetTrigger("jump"); 
        }
    }

    void ClampZPosition()
    {
        Vector3 pos = rb.position;
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        rb.position = pos;
    }

    // --- [BARU] Fungsi Flip ---
    void Flip()
    {
        isFacingRight = !isFacingRight; // Balik status boolean

        // Ambil scale saat ini
        Vector3 currentScale = transform.localScale;
        
        // Kalikan sumbu X dengan -1 (membalik gambar)
        currentScale.x *= -1; 
        
        // Terapkan scale baru
        transform.localScale = currentScale;
    }
}