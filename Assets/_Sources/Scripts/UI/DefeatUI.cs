using UnityEngine;

public class DefeatUI : MonoBehaviour
{
    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += Open;
        GameController.OnDeadAnimalLimitReached += Open;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= Open;
        GameController.OnDeadAnimalLimitReached -= Open;
    }

    private void Open()
    {
        UIController.Instance.OpenPanel(PanelType.Defeat);
    }
}
