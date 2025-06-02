using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ApplySuperSpeed(Player player, float multiplier, float duration)
    {
        StartCoroutine(SuperSpeedRoutine(player, multiplier, duration));
    }

    private IEnumerator SuperSpeedRoutine(Player player, float multiplier, float duration)
    {
        bool originalInvincible = player.IsDead; // Vamos usar como "pode tomar dano = false"
        float originalSpeed = player.rb.linearVelocity.magnitude;

        player.IsDead = false; // Simula invencibilidade

        // Aplica multiplicador de velocidade
        player.rb.linearVelocity *= multiplier;

        Debug.Log("Super Speed + Invencibilidade ativados!");

        yield return new WaitForSeconds(duration);

        player.IsDead = originalInvincible;
        Debug.Log("PowerUp acabou.");
    }
}
