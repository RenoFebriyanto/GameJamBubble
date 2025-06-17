using UnityEngine;
using System.Collections;

public class TimeFreeze : MonoBehaviour
{
    public float freezeDuration = 2f;
    public float cooldown = 5f;
    private float cooldownTimer = 0f;
    private bool isFreezing = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && cooldownTimer <= 0)
        {
            StartCoroutine(FreezeTime());
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private IEnumerator FreezeTime()
    {
        if (isFreezing) yield break;
        isFreezing = true;
        Debug.Log("TIme has stoped");

        Time.timeScale = 0.1f;
        cooldownTimer = cooldown;

        yield return new WaitForSecondsRealtime(freezeDuration);

        Time.timeScale = 1f;
        isFreezing = false;
    }
}
