using UnityEngine;

public class SwordInteraction : MonoBehaviour
{
    public Animator playerAnimator; // Animator untuk pemain
    public string pickUpSwordAnimation = "Interact"; // Nama trigger animasi
    public KeyCode interactionKey = KeyCode.J; // Tombol interaksi yang diubah ke J
    private bool isNearSword = false; // Status pemain dekat dengan pedang
    private GameObject currentSword; // Pedang yang sedang berinteraksi

    private void Update()
    {
        if (isNearSword && Input.GetKeyDown(interactionKey))
        {
            InteractWithSword();
        }
    }

    private void InteractWithSword()
    {
        if (playerAnimator != null && currentSword != null)
        {
            // Pastikan animasi mengambil pedang yang benar dipicu
            playerAnimator.SetTrigger(pickUpSwordAnimation);

            // Opsional: Memberikan pedang ke pemain (misalnya menonaktifkan objek pedang)
            PickUpSword(currentSword);

            // Hancurkan pedang setelah animasi selesai
            Destroy(currentSword);
        }
    }

    private void PickUpSword(GameObject sword)
    {
        // Menonaktifkan pedang dari scene setelah diambil
        sword.SetActive(false); // Menonaktifkan pedang setelah diambil
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            isNearSword = true;
            currentSword = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            isNearSword = false;
            currentSword = null;
        }
    }
}
