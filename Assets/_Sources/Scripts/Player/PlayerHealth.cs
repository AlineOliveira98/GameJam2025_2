using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth = 0;
    [SerializeField] private bool receiveDamage = true;

    public bool IsDead { get; set; }
    public bool IsInvincible { get; private set; } = false;

    public static Action<float, float> OnPlayerHealthChanged;
    public static Action OnPlayerDied;

    void Start()
    {
        RecoverHealth(1f);
    }

    public void RecoverHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (!receiveDamage) return;
        if (IsDead || IsInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            IsDead = true;
            OnPlayerDied?.Invoke();
        }
    }

    public async void SetInvincibility(float duration)
    {
        IsInvincible = true;
        await Task.Delay(TimeSpan.FromSeconds(duration));
        IsInvincible = false;
    }
}
