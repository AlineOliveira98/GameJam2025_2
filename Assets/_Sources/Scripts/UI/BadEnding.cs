using UnityEngine;
using UnityEngine.SceneManagement;

public class BadEnding : MonoBehaviour
{
    public void TryAgain()
    {
        SceneManager.LoadScene(0);
    }
}
