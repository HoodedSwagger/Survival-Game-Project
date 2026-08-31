using System.Collections;
using UnityEngine;

public class SwingEffects : MonoBehaviour
{
    private void OnEnable()
    {
        EventBus<EffectApplyEvent>.Subscribe(ApplyEffects);
    }
    private void OnDisable()
    {
        EventBus<EffectApplyEvent>.Unsubscribe(ApplyEffects);
    }
    private void ApplyEffects(EffectApplyEvent evt)
    {
        StartCoroutine(Delay(evt));
    }

    private IEnumerator Delay(EffectApplyEvent evt)
    {
        yield return new WaitForSeconds(0.5f);

        if(evt._soundDefinition != null)
            AudioManager.Instance.Play(evt._soundDefinition);
        if (evt._hitPoint == null) yield return null;
        EventBus<CameraShakeEvent>.Raise(new CameraShakeEvent()
        {
            magnitude = 0.1f
        });

        if(evt._hitEffect != null) 
            Instantiate(evt._hitEffect, evt._hitPoint, Quaternion.identity);

        if (evt._damageable != null)
            evt._damageable.TakeDamage(evt._ToolDamage);

       
    }
}
