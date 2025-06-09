using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float delayAnimTakeDamage = 0.5f;
    [SerializeField] private float invulnerableDurationWhenDamaged = 1f;
    [SerializeField] private float delayToDie = 0.5f;
    [SerializeField] private bool receiveDamage = true;

    private float currentHealth = 0;
    private Player player;

    public float CurrentHealth => currentHealth;


    public bool IsDead { get; set; }
    public bool IsInvincible { get; private set; } = false;

    public static Action<float, float> OnPlayerHealthChanged;
    public static Action OnPlayerDied;

    void Start()
    {
        RecoverHealth(1f);

        player = GetComponent<Player>();
    }

    public void RecoverHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public async void TakeDamage(float damage)
    {
        if (IsDead || IsInvincible) return;

        if (receiveDamage) currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        player.Visual.TakeDamage();
        if (receiveDamage) SetInvincibility(invulnerableDurationWhenDamaged);
        OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

        await UniTask.Delay(TimeSpan.FromSeconds(delayAnimTakeDamage));

        if (currentHealth <= 0f)
        {
            player.Visual.SetDeath();
            IsDead = true;
            player.Movement.Stop();
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayToDie));

            IsDead = true;
            GameController.Instance.GameOver();
            OnPlayerDied?.Invoke();
        }
    }

    public async void SetInvincibility(float duration)
    {
        IsInvincible = true;
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        IsInvincible = false;
    }
}
