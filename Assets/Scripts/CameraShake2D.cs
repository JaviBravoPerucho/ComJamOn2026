using UnityEngine;
using System.Collections;

public class CameraShake2D : MonoBehaviour
{
    private Coroutine shakeCoroutine;

    Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
    }

    public void Shake(float duration, float magnitude)
    {
        Debug.Log("SHAKE ACTIVADO");
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            transform.position = originalPos;
        }
            

        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}