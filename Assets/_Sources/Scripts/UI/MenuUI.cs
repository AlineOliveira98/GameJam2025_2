using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void StartGameplay()
    {
        GameController.Instance.StartGameplay();
        UIController.Instance.OpenPanel(PanelType.Gameplay);
    }

    public void OpenSettings()
    {
        UIController.Instance.OpenPanel(PanelType.Settings);
    }
}
