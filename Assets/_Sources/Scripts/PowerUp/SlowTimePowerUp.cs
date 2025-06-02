using UnityEngine;

public class SlowTimePowerUp : MonoBehaviour, ICollectable
{
    public float slowFactor = 0.2f;
    public float duration = 3f;

    public void Collect()
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        Debug.Log("Slow time activated!");

        Destroy(gameObject);
        TimeManager.Instance.RestoreTimeAfterDelay(duration);
    }
}
