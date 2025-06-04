using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    void OnEnable()
    {
        GameController.OnSavedAnimalAmountReached += Open;
    }

    void OnDisable()
    {
        GameController.OnSavedAnimalAmountReached -= Open;
    }

    private void Open()
    {
        UIController.Instance.OpenPanel(PanelType.Victory);
    }
}
