using UnityEngine;

public class SettingsPanelOpen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private bool isActive = false;

    public void Open()
    {
        if ((panel == null)) return;
        panel.SetActive(true);
        isActive = true;

    }
    public void Close()
    {
        if (panel == null) return;
        panel.SetActive(false);
        isActive = false;
    }
}
