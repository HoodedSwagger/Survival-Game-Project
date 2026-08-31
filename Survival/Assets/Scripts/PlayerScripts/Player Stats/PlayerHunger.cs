using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    [SerializeField] private int maxHunger = 100;
    private int currentHunger;
    public int CurrentHunger => currentHunger;

    [SerializeField] private int starvationDamage = 5;

    [SerializeField] private float hungerLossInterval = 60f;
    private float hungerLossTimer = 0;


    private IDamageable health;

    private void OnEnable()
    {
        EventBus<FoodEatenEvent>.Subscribe(RestoreHunger);

    }
    private void OnDisable()
    {
        EventBus<FoodEatenEvent>.Unsubscribe(RestoreHunger);
    }
    private void Start()
    {
        currentHunger = maxHunger;
        if (gameObject.TryGetComponent(out IDamageable hp))
        {
            health = hp;
        }
        else
        {
            Debug.LogWarning($"Can't find IDamageable on {gameObject.name}");
        }
    }

    private void Update()
    {
        hungerLossTimer += Time.deltaTime;

        if (hungerLossTimer >= hungerLossInterval)
        {
            LossHunger();
            hungerLossTimer = 0;
        }
    }

    private void LossHunger()
    {
        if (currentHunger > 0)
        { 
            currentHunger--;
        }
        else
            health.TakeDamage(starvationDamage);

        UpdateUI();
    }
    public void RestoreHunger(FoodEatenEvent eatEvent)
    {
        currentHunger += eatEvent._hungerRestoreAmount;

        if (currentHunger > maxHunger)
            currentHunger = maxHunger;

        UpdateUI();
    }

    public void SetHunger(int value)
    {
        currentHunger = Mathf.Clamp(value, 0, maxHunger);
        UpdateUI();
    }

    public void UpdateUI()
    {
        HungerUpdateEvent hungerUpdateEvent = new HungerUpdateEvent()
        {
            Hunger = currentHunger,
            MaxHunger = maxHunger
        };
        EventBus<HungerUpdateEvent>.Raise(hungerUpdateEvent);
    }

}
