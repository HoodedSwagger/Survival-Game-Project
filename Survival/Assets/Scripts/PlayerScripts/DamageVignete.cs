using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageVignete : MonoBehaviour
{
    private Volume volume;

    [SerializeField] private float targetWeight = 1;
    [SerializeField] private float decaySpeed = 0.5f;

    private void OnEnable() => EventBus<PlayerDamageTakenEvent>.Subscribe(ActivateVignette);
    private void OnDisable() => EventBus<PlayerDamageTakenEvent>.Unsubscribe(ActivateVignette);

    private void Start()
    {
        volume = GetComponent<Volume>();
    }
    private void ActivateVignette(PlayerDamageTakenEvent evt)
    {
        StartCoroutine(LerpWeight());
    }
    private IEnumerator LerpWeight()
    {
        volume.weight = targetWeight;

        while (volume.weight > 0.01f)
        {
            volume.weight = Mathf.Lerp(volume.weight, 0, decaySpeed * Time.deltaTime);
            yield return null;
        }
        volume.weight = 0;
    }
}
