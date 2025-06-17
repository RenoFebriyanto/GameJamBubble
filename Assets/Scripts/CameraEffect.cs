using UnityEngine;
using System.Collections;

public class CameraEffect : MonoBehaviour
{
    public float shakeDuration = 1f;  // Durasi gempa
    public float shakeMagnitude = 0.1f;  // Besar getaran
    public float shakeFrequency = 0.1f;  // Kecepatan getaran

    private Vector3 originalPosition;

    void Start()
    {
        // Menunggu 4 detik setelah scene dimulai sebelum memulai gempa
        StartCoroutine(WaitAndShake(4f));
    }

    // Fungsi untuk memulai gempa
    private IEnumerator WaitAndShake(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(ShakeCamera());
    }

    // Fungsi untuk melakukan shake pada kamera
    private IEnumerator ShakeCamera()
    {
        originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);

            transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(shakeFrequency);
        }

        // Mengembalikan posisi kamera ke posisi semula setelah gempa selesai
        transform.position = originalPosition;
    }
}
