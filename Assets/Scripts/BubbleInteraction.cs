using UnityEngine;

public class BubbleInteraction : MonoBehaviour
{
    public Animator playerAnimator;
    public string breakBubbleAnimation = "Cut";
    public KeyCode interactionKey = KeyCode.K;
    public Cutscene cutsceneManager;
    public StoryScene[] bubbleDialogs;
    private int currentBubbleIndex = 0;

    private bool isNearBubble = false;
    private GameObject currentBubble;
    private Movement playerMovement; // Tambahkan referensi Movement

    private void Start()
    {
        // Mendapatkan referensi ke komponen Movement
        playerMovement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (isNearBubble && Input.GetKeyDown(interactionKey))
        {
            InteractWithBubble();
        }
    }

    private void InteractWithBubble()
    {
        if (playerAnimator != null && currentBubble != null)
        {
            playerMovement.enabled = false; // Menonaktifkan movement saat dialog dimulai
            playerAnimator.SetTrigger(breakBubbleAnimation);
            ReleaseNPC(currentBubble);
            Destroy(currentBubble);

            if (cutsceneManager != null && currentBubbleIndex < bubbleDialogs.Length)
            {
                bool isLastScene = currentBubbleIndex == bubbleDialogs.Length - 1; // Cek apakah ini adalah scene terakhir
                cutsceneManager.StartDialog(bubbleDialogs[currentBubbleIndex], () =>
                {
                    // Mengaktifkan kembali movement setelah dialog selesai
                    playerMovement.enabled = true;
                }, isLastScene);

                currentBubbleIndex++;
            }
        }
    }

    private void ReleaseNPC(GameObject bubble)
    {
        Transform npc = bubble.transform.Find("NPC");
        if (npc != null)
        {
            npc.SetParent(null);
            npc.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bubble"))
        {
            isNearBubble = true;
            currentBubble = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bubble"))
        {
            isNearBubble = false;
            currentBubble = null;
        }
    }
}
