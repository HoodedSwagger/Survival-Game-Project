using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Start()
    {
        continueButton.interactable = SaveSystem.Instance.HasSave();
    }
    public void OnNewGameClicked(int index)
    {
        bool deleted = SaveSystem.Instance.DeleteSave();

        if (deleted)
        {
            EventBus<SceneLoadEvent>.Raise(new SceneLoadEvent { sceneIndex = index });
        }
    }
    public void Continue(int index)
    {
        EventBus<SceneLoadEvent>.Raise(new SceneLoadEvent { sceneIndex = index });
    }
}
