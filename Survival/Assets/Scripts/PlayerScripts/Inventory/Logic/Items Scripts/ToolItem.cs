using UnityEngine;
[CreateAssetMenu(fileName = "New Tool", menuName = "Item/Tools/Create New Tool")]
public class ToolItem : Item, IUsable
{
    [SerializeField] private string useAnimName;
    public string UseAnimName { get => useAnimName; set => useAnimName = value; }
    [SerializeField] private int toolDamage;

    [SerializeField] private LayerMask harvestLayer;

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private SoundDefinition hitSounds;
    [SerializeField] private SoundDefinition missSounds;
    public LayerMask HarvestLayer => harvestLayer;
    public int ToolDamage => toolDamage;
    public SoundDefinition Sounds => hitSounds;
    public SoundDefinition MissSounds => missSounds;

    public void Use(InventorySlot slot, Inventory inventory)
    {
        RaycastHit hit;
        Transform playerCamera = Camera.main.transform;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 3, harvestLayer))
        {
            if (hit.transform.TryGetComponent(out IDamageable component))
            {
                EffectApplyEvent effectApplyEvent = new EffectApplyEvent()
                {
                    _damageable = component,
                    _ToolDamage = toolDamage,
                    _hitEffect = hitEffect,
                    _hitPoint = hit.point,
                    _soundDefinition = hitSounds,
                };

                EventBus<EffectApplyEvent>.Raise(effectApplyEvent);
            }
        }
        else
        {
            EffectApplyEvent effectApplyEvent = new EffectApplyEvent()
            {
                _soundDefinition = missSounds,
            };

            EventBus<EffectApplyEvent>.Raise(effectApplyEvent);
        }
    }
}
