using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
