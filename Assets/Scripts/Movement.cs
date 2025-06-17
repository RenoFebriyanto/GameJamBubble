using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;

    public float speed = 5f;
    public float gravity = -25f;
    public float maxFallSpeed = -20f;
    public float baseJumpForce = 5f;
    public float maxJumpForce = 8f;
    public float maxJumpTime = 0.2f;

    private bool isGrounded = false;
    private bool isJumping = false;
    private bool canDoubleJump = false;
    private float jumpTime = 0f;

    private Movement movementScript;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimeLeft;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 3f;

    public float freezeDuration = 2f;
    private bool isFreezing = false;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float range = 8f;

    private bool canMove = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
        movementScript = GetComponent<Movement>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!canMove) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        bool isMoving = Mathf.Abs(body.linearVelocity.x) > 0.01f;

        // Set parameter isRunning untuk animasi
        animator.SetBool("Run", isMoving);

        // Mengubah arah tampilan player
        if (horizontalInput > 0) // Bergerak ke kanan
        {
            transform.localScale = new Vector3(5f, 5f, 5f); // Menghadap kanan
        }
        else if (horizontalInput < 0) // Bergerak ke kiri
        {
            transform.localScale = new Vector3(-5f, 5f, 5f); // Menghadap kiri
        }

        if (Input.GetKey(KeyCode.Q) && canDash && isMoving && !isDashing)
        {
            StartDash();
        }

        if (isDashing)
        {
            DashMovement();
        }

        if (!isGrounded && !isJumping)
        {
            float newVerticalVelocity = body.linearVelocity.y + gravity * Time.deltaTime;
            body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Max(newVerticalVelocity, maxFallSpeed));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                StartJump();
            }
            else if (!canDoubleJump && !isGrounded)
            {
                PerformDoubleJump();
            }
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            ContinueJump();
        }

        if (Input.GetKeyUp(KeyCode.Space) || jumpTime >= maxJumpTime)
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FreezeTime());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptTeleport();
        }

        // Update animasi lompat dan jatuh
        if (body.linearVelocity.y > 0.1f && !isGrounded)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Fall", false);
        }
        else if (body.linearVelocity.y < -0.1f && !isGrounded)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", false);
        }

        if (!canMove)
        {
            body.linearVelocity = Vector2.zero; // Hentikan gerakan
            animator.SetBool("Run", false); // Nonaktifkan animasi lari
            return;
        }
    }

    private void StartJump()
    {
        isJumping = true;
        isGrounded = false;
        jumpTime = 0f;
        body.linearVelocity = new Vector2(body.linearVelocity.x, baseJumpForce);
    }

    private void ContinueJump()
    {
        jumpTime += Time.deltaTime;
        if (jumpTime < maxJumpTime)
        {
            float extraJumpForce = Mathf.Lerp(baseJumpForce, maxJumpForce, jumpTime / maxJumpTime);
            body.linearVelocity = new Vector2(body.linearVelocity.x, extraJumpForce);
        }
        else
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, maxJumpForce);
        }
    }

    private void PerformDoubleJump()
    {
        canDoubleJump = true;
        isJumping = true;
        body.linearVelocity = new Vector2(body.linearVelocity.x, baseJumpForce);
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        body.gravityScale = 0;
        animator.SetTrigger("Dash");
        movementScript.enabled = false;
        Invoke(nameof(EndDash), dashDuration);
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void DashMovement()
    {
        if (dashTimeLeft > 0)
        {
            float dashDirection = Mathf.Sign(body.linearVelocity.x);
            body.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);
            dashTimeLeft -= Time.deltaTime;
        }
    }

    private void EndDash()
    {
        isDashing = false;
        body.gravityScale = movementScript.gravity / Physics2D.gravity.y;
        movementScript.enabled = true;
        body.linearVelocity = Vector2.zero;
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void AttemptTeleport()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range);

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag(enemyTag))
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            TeleportToTarget(closestEnemy);
        }
        else
        {
            Debug.Log("No enemies in range!");
        }
    }

    private void TeleportToTarget(Transform target)
    {
        transform.position = target.position;
        Destroy(target.gameObject);
        Debug.Log("Teleported to enemy and destroyed it!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            isJumping = false;
            canDoubleJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private IEnumerator FreezeTime()
    {
        if (isFreezing) yield break;
        isFreezing = true;
        Debug.Log("Time has stopped");

        Time.timeScale = 0.1f;

        yield return new WaitForSecondsRealtime(freezeDuration);

        Time.timeScale = 1f;
        isFreezing = false;

        canDoubleJump = false;
        canDash = true;

        Debug.Log("Reset ability");
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    // Fungsi untuk mengaktifkan kembali pergerakan
    public void EnableMovement()
    {
        canMove = true;
    }
}
