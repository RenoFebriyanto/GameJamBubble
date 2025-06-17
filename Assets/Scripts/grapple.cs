using System.Collections;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public GameObject target;
    public GameObject player;
    public float grappleSpeed = 5f;
    private Rigidbody2D playerRigidbody;

    public float normalGravity = 1f;
    public float grappleGravity = 0f;

    private bool isGrappling = false;

    private void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isGrappling) // Movement hanya berjalan jika tidak grappling
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            playerRigidbody.linearVelocity = new Vector2(horizontalInput * 5f, playerRigidbody.linearVelocity.y);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E) && !isGrappling)
        {
            StartCoroutine(Grappling());
        }
    }

    private IEnumerator Grappling()
    {
        isGrappling = true;
        playerRigidbody.gravityScale = grappleGravity;

        while (Vector2.Distance(playerRigidbody.position, target.transform.position) > 0.1f)
        {
            Vector2 direction = (target.transform.position - player.transform.position).normalized;
            Vector2 grappleVelocity = direction * grappleSpeed;
            playerRigidbody.linearVelocity = grappleVelocity;

            yield return null;
        }

        playerRigidbody.position = target.transform.position;
        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.gravityScale = normalGravity;
        isGrappling = false;
    }
}