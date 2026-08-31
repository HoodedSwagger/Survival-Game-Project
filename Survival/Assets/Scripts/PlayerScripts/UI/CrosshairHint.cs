using UnityEngine;
using TMPro;
public class CrosshairHint : MonoBehaviour
{
    private TMP_Text _text;
    private void Start()
    {
        EventBus<AimedAtItemEvent>.Subscribe(UpdateHint);

        _text = GetComponent<TMP_Text>();
    }
    public void UpdateHint(AimedAtItemEvent updateInfo)
    {
        _text.SetText(updateInfo.text);
    }

    private void OnDestroy()
    {
        EventBus<AimedAtItemEvent>.Unsubscribe(UpdateHint);
    }
}
