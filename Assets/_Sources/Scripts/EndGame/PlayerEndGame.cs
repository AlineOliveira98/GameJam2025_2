using UnityEngine;

public class PlayerEndGame : MonoBehaviour
{
    [SerializeField] private PlayerVisual playerVisual;

    void Start()
    {
        playerVisual.SetDirection(Vector2.up);
    }

    void OnEnable()
    {
        GameController.OnGameEnding += GameEnd;
    }

    void OnDisable()
    {
        GameController.OnGameEnding -= GameEnd;
    }

    private void GameEnd()
    {
        
    }
}
