using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text textDisplay;
 
    private void OnEnable()
    {
        EventBus<HealthUpdateEvent>.Subscribe(UpdateUI);
    }
    private void OnDisable()
    {
        EventBus<HealthUpdateEvent>.Unsubscribe(UpdateUI);
    }
    public void UpdateUI(HealthUpdateEvent updateInfo)
    {
        float fillAmount = (float)updateInfo.Health / (float)updateInfo.MaxHealth;
        healthBar.fillAmount = fillAmount;
        textDisplay.SetText(updateInfo.Health.ToString());
    }

}
