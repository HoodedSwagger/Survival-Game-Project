using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public void Close()
    {
        UIManager.Instance.Close();
    }
}
