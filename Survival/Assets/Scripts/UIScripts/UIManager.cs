using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject craftPanel;
    [SerializeField] private GameObject pausePanel;

    [Header("References")]
    [SerializeField] private CameraControll cam;

    private GameObject _activePanel;

    private void Awake()
    {
        if(Instance != null) { Destroy(Instance); return; }
        Instance = this;
    }

    private void Update()
    {
        if (InputService.CraftPanelButtonPressed)
            Toggle(craftPanel);
        if(InputService.PauseButtonPressed)
            Toggle(pausePanel, true);

    }

    public void Toggle(GameObject panel, bool setPause = false)
    {
        if (_activePanel == panel)
            Close();
        else
            Open(panel,setPause);
    }
    public void Open(GameObject panel, bool setPause = false)
    {
        if (_activePanel != null)
        {
            _activePanel.SetActive(false);
        }
        _activePanel = panel;
        panel.SetActive(true);
        SetGameplayState(false,setPause);
    }
    public void Close()
    {
        if(_activePanel != null)
            _activePanel.SetActive(false);
        _activePanel = null;
        SetGameplayState(true);
    }

    public void SetGameplayState(bool gameplay, bool setPause = false)
    {
        Cursor.lockState = gameplay ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !gameplay;
        if (cam != null) cam.canRotate = gameplay;
        Time.timeScale = setPause ? 0 : 1;
    }
}
