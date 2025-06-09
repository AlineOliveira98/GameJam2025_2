using TMPro;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    void OnEnable()
    {
        YggDrasil.OnTreeCanGrow += LastAnimalWarning;
    }

    void OnDisable()
    {
        YggDrasil.OnTreeCanGrow -= LastAnimalWarning;
    }

    private void LastAnimalWarning()
    {
        UIController.Instance.OpenPanel(PanelType.Victory);
    }

    public void ReturnGameplay()
    {
        UIController.Instance.OpenPanel(PanelType.Gameplay);
        GameController.Instance.PauseGame(false);
        GameController.Instance.EnableLastAnimal();
    }

    public void ChooseSadEnding()
    {
        CameraController.Instance.SetCamera(CameraType.GoodEndGame);
    }
}
