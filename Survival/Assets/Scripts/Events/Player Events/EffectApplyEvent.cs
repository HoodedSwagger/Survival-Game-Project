using UnityEngine;

public struct EffectApplyEvent
{
    public Vector3 _hitPoint;
    public GameObject _hitEffect;

    public int _ToolDamage;
    public IDamageable _damageable;
    public SoundDefinition _soundDefinition;
}
