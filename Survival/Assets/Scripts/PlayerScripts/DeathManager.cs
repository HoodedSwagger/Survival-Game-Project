using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public GameObject deathScreen;
    private void OnEnable()
    {
        EventBus<PlayerDeathEvent>.Subscribe(SetUpDeathScreen);
    }
    private void OnDisable()
    {
        EventBus<PlayerDeathEvent>.Unsubscribe(SetUpDeathScreen);
    }

    private void SetUpDeathScreen(PlayerDeathEvent evt)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deathScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
