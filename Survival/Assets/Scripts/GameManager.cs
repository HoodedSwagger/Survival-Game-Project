using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        SaveSystem.Instance.Load();
    }
    public void SaveAndQuit()
    {
        bool saved = SaveSystem.Instance.Save();

        if (saved)
        {
            Application.Quit();
        }
        else
        {
            Debug.LogError("Ошибка сохранения и выхода");
        }
    }
}
