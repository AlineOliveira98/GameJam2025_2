using UnityEngine;
using UnityEngine.SceneManagement;

public class BadEnding : MonoBehaviour
{
    [SerializeField] private AudioSource monologueAudio;

    void OnEnable()
    {
        GameController.OnGameEnding += AnimEndGame;
    }

    void OnDisable()
    {
        GameController.OnGameEnding -= AnimEndGame;
    }

    public void AnimEndGame()
    {
        monologueAudio.Play();
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(0);
    }
}
