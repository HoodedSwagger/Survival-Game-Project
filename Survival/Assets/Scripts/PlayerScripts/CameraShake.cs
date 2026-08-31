using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isShaking = false; 

    void Start()
    {
        originalPosition = transform.localPosition;
    }
    private void OnEnable()
    {
        EventBus<CameraShakeEvent>.Subscribe(TriggerShake);
    }
    private void OnDisable()
    {
        EventBus<CameraShakeEvent>.Unsubscribe(TriggerShake);
    }

    public void TriggerShake(CameraShakeEvent evt)
    {
        if (!isShaking) 
        {
            StartCoroutine(Shake(evt.magnitude));
        }
    }

    private IEnumerator Shake(float magnitude)
    {
        isShaking = true;

        float shakeDuration = 0.2f; 
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
        isShaking = false; 
    }
}
