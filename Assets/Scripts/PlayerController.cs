using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 12f;
    public float deceleration = 16f;
    Vector2 moveInput;
    
    private bool isFacingRight = true; 

    [Header("Jump")]
    public float jumpForce = 7f;
    bool jumpInput;

    [Header("States")]
    bool isAbsorbing;
    bool isSitting = false; // Default tidak duduk

    [Header("Absorb Ability")]
    public float absorbRadius = 3f;
    public LayerMask absorbLayer;   
    public Transform absorbPoint;   

    [Header("Ground Check")]
    public Transform grdChecker;
    public LayerMask ground;
    public float rayLength = 0.6f;
    bool grounded;

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
        if (absorbPoint == null) absorbPoint = transform;
    }

    void Update()
    {
        // --- 1. LOGIKA DUDUK (TOMBOL C) ---
        // Tekan C untuk duduk, Tekan C lagi untuk berdiri
        if (Input.GetKeyDown(KeyCode.C) && grounded && !isAbsorbing)
        {
            isSitting = !isSitting; // Membalik status (True <-> False)
            animator.SetBool("isSitting", isSitting);
        }

        // --- 2. LOGIKA ABSORB ---
        isAbsorbing = Input.GetKey(KeyCode.E);
        if (animator != null) animator.SetBool("isAbsorbing", isAbsorbing);

        // --- KONDISI PENGUNCI GERAKAN ---
        // Jika sedang Duduk ATAU Absorb, matikan gerakan
        if (isSitting || isAbsorbing)
        {
            moveInput = Vector2.zero; // Paksa diam
            if (animator != null) animator.SetBool("isRunning", false);

            // Jika sedang duduk tapi pemain memaksa Absorb, batalkan duduknya (Opsional)
            if (isAbsorbing && isSitting)
            {
                isSitting = false;
                animator.SetBool("isSitting", false);
            }
            
            if (isAbsorbing) HandleAbsorbAction();
        }
        else 
        {
            // --- 3. LOGIKA GERAK NORMAL ---
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space))
                jumpInput = true;

            if (animator != null)
                animator.SetBool("isRunning", moveInput.sqrMagnitude > 0.1f);

            if (moveInput.x > 0 && !isFacingRight) Flip();
            else if (moveInput.x < 0 && isFacingRight) Flip();
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
        SmoothMove(); 
        
        // Hanya bisa lompat jika TIDAK duduk dan TIDAK absorb
        if (!isAbsorbing && !isSitting)
        {
            JumpCheck();
        }
    }

    // --- BAGIAN BAWAH SAMA SEPERTI SEBELUMNYA ---
    void HandleAbsorbAction()
    {
        Collider[] items = Physics.OverlapSphere(transform.position, absorbRadius, absorbLayer);
        foreach (Collider item in items) { /* Logic Item */ }
    }

    void GroundCheck()
    {
        if (grdChecker != null)
            grounded = Physics.Raycast(grdChecker.position, Vector3.down, out groundHit, rayLength, ground);
    }

    void SmoothMove()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        Vector3 targetVelocity;

        if (grounded) {
            Vector3 slopeDir = Vector3.ProjectOnPlane(inputDir, groundHit.normal);
            targetVelocity = slopeDir * moveSpeed;
        } else {
            targetVelocity = inputDir * (moveSpeed * 0.6f);
        }

        Vector3 currentVelocity = rb.velocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

        // Deselerasi jika tidak ada input ATAU sedang duduk/absorb
        float accel = (inputDir.magnitude > 0 && !isAbsorbing && !isSitting) ? acceleration : deceleration;

        Vector3 smoothVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, accel * Time.fixedDeltaTime);
        rb.velocity = new Vector3(smoothVelocity.x, currentVelocity.y, smoothVelocity.z);
    }

    void JumpCheck()
    {
        if (jumpInput && grounded) {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpInput = false;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1; 
        transform.localScale = currentScale;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, absorbRadius);
    }
}