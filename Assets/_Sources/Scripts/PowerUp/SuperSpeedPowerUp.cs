using UnityEngine;

public class SuperSpeedPowerUp : MonoBehaviour, ICollectable
{
    public float timeScaleBoost = 2f;
    public float duration = 5f;

    public void Collect()
    {
        var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerHealth>();
        if (player == null)
        {
            Debug.LogError("Player n�o encontrado!");
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
