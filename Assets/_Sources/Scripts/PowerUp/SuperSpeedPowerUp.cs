using UnityEngine;

public class SuperSpeedPowerUp : MonoBehaviour, ICollectable
{
    public float timeScaleBoost = 2f;
    public float duration = 5f;

    public void Collect()
    {
        var player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player não encontrado!");
            return;
        }

        Time.timeScale = timeScaleBoost;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        player.SetInvincibility(duration);

        Debug.Log("Super Speed (TimeScale) ativado!");

        Destroy(gameObject);

        TimeManager.Instance.RestoreTimeAfterDelay(duration);
    }
}
