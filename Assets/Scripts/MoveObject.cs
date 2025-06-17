using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour
{
    public Transform targetPosition;  // Titik tujuan (GameObject kosong)
    public float moveDuration = 3f;   // Durasi pergerakan
    public float waitTime = 5f;       // Waktu tunggu sebelum mulai bergerak

    private bool isMoving = false;

    void Start()
    {
        // Mulai coroutine untuk menunggu 5 detik dan kemudian memindahkan objek
        StartCoroutine(MoveAfterDelay(waitTime));
    }

    // Coroutine untuk menunggu waktu sebelum mulai bergerak
    private IEnumerator MoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // Menunggu waktu yang ditentukan
        isMoving = true;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;  // Posisi awal GameObject
        Vector3 endPosition = targetPosition.position;  // Posisi tujuan (target)

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Setelah selesai, pastikan objek berada di posisi akhir
        transform.position = endPosition;
    }
}
