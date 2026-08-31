using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HungerUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text textDisplay;

    private void OnEnable()
    {
        EventBus<HungerUpdateEvent>.Subscribe(UpdateHungerUI);
    }
    private void OnDisable()
    {
        EventBus<HungerUpdateEvent>.Unsubscribe(UpdateHungerUI);
    }
    public void UpdateHungerUI(HungerUpdateEvent hungerUpdateInfo)
    {
        float newFillAmount = (float)hungerUpdateInfo.Hunger / (float)hungerUpdateInfo.MaxHunger;
        healthBar.fillAmount = newFillAmount;

        textDisplay.SetText(hungerUpdateInfo.Hunger.ToString());
    }
}
