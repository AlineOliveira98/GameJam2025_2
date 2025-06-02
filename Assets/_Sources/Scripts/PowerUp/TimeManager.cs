using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RestoreTimeAfterDelay(float delay)
    {
        StartCoroutine(RestoreRoutine(delay));
    }

    private IEnumerator RestoreRoutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        Debug.Log("Time restaurado!");
    }
}
