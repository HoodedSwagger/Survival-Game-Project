using System.ComponentModel;
using UnityEngine;
[CreateAssetMenu(fileName = "New Meele Weapon", menuName = "Weapon/Create New Weapon")]
public class MeeleWeaponItem : Item, IUsable
{
    [SerializeField] private string useAnimName;
    public string UseAnimName { get => useAnimName; set => useAnimName = value; }

    [SerializeField] private int weaponDamage = 1;

    [SerializeField] private float range = 3f;
    [SerializeField] private GameObject hitEffect;

    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private SoundDefinition attackSoundDef;

    public int WeaponDamage => weaponDamage;
    public float Range => range;
    public LayerMask AttackLayer => attackLayer;
    public void Use(InventorySlot slot, Inventory inventory)
    {
        Transform playerCamera = Camera.main.transform;
        RaycastHit hit;
        Debug.DrawRay(playerCamera.position, playerCamera.forward * range, Color.red, 5f);
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range, attackLayer))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                EffectApplyEvent effectApplyEvent = new EffectApplyEvent()
                {
                    _damageable = damageable,
                    _ToolDamage = weaponDamage,
                    _hitEffect = hitEffect,
                    _hitPoint = hit.point,
                    _soundDefinition = attackSoundDef,
                };

                EventBus<EffectApplyEvent>.Raise(effectApplyEvent);
            }
        }
        else
        {
            EffectApplyEvent effectApplyEvent = new EffectApplyEvent()
            {
                _soundDefinition = attackSoundDef,
            };

            EventBus<EffectApplyEvent>.Raise(effectApplyEvent);
        }
    }
}
