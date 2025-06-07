using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += PlayerDied;
        GameController.OnDeadAnimalLimitReached += ManyAnimalsDead;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= PlayerDied;
        GameController.OnDeadAnimalLimitReached -= ManyAnimalsDead;
    }

    private void Open()
    {
        UIController.Instance.OpenPanel(PanelType.Defeat);
    }

    private void PlayerDied()
    {
        messageText.text = $"You Died!";
        Open();
    }

    private void ManyAnimalsDead()
    {
        messageText.text = $"Many animals died!";
        Open();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
