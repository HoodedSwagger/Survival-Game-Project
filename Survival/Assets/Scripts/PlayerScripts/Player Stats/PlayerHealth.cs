using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private int accumulatedDamage = 0;
    [SerializeField] private int damageToHealthBuff = 10;
    [SerializeField] private int healthBuff = 2;
    private bool[] amulets = {false, false, false, false};
    public int CurrentHealth => currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        EventBus<FoodEatenEvent>.Subscribe(RestoreHPviaFood);
        EventBus<AmuletUsedEvent>.Subscribe(OnAmuletUsed);
    }
    private void OnDisable()
    {
        EventBus<FoodEatenEvent>.Unsubscribe(RestoreHPviaFood);
        EventBus<AmuletUsedEvent>.Unsubscribe(OnAmuletUsed);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        accumulatedDamage += damage;
        if (currentHealth <= 0)
        {
            Dead();
        }
        if (accumulatedDamage > damageToHealthBuff)
            ConvertDamageToMaxHP();

        EventBus<PlayerDamageTakenEvent>.Raise(new PlayerDamageTakenEvent());

        UpdateUI();
    }
    private void RestoreHPviaFood(FoodEatenEvent foodEatenEvent)
    {
        RestoreHealth(foodEatenEvent._healthRestoreAmount);
    }
    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            Dead();
        }

        UpdateUI();

    }

    private void UpdateUI()
    {
        HealthUpdateEvent healthInfo = new HealthUpdateEvent()
        {
            Health = currentHealth,
            MaxHealth = maxHealth
        };
        EventBus<HealthUpdateEvent>.Raise(healthInfo);
    }
    private void Dead()
    {
        EventBus<PlayerDeathEvent>.Raise(new PlayerDeathEvent());
    }

    public void ModifyMaxHeath(int amount)
    {
        maxHealth += amount;
        RestoreHealth(amount);
    }
    public void SetHealth(int value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);

        UpdateUI();
    }

    public void ConvertDamageToMaxHP()
    {
        while (accumulatedDamage >= 10)
        {
            accumulatedDamage -= damageToHealthBuff;
            maxHealth += healthBuff;
        }
    }

    private void OnAmuletUsed(AmuletUsedEvent evt)
    {
        if (amulets[evt.AmuletIndex] == false)
        {
            ModifyMaxHeath(evt.increase);
            amulets[evt.AmuletIndex] = true;
        }
    }
}
