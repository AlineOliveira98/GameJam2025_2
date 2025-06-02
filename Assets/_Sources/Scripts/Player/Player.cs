using System;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private TargetIndicator targetIndicator;
    public Rigidbody2D rb { get; private set; }
    public Vector2 input { get; private set; }

    public bool IsDead { get; set; }

    public float speedMultiplier { get; private set; } = 1f;
    public bool IsInvincible { get; private set; } = false;

    [SerializeField] private float baseSpeed = 5f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input * (baseSpeed * speedMultiplier);

    }


    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || IsInvincible) return;

        Debug.Log("Taking Damage");
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();
        }
    }

    public async void SetSpeedMultiplier(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        await Task.Delay(TimeSpan.FromSeconds(duration));
        speedMultiplier = 1f;
    }

    public async void SetInvincibility(float duration)
    {
        IsInvincible = true;
        await Task.Delay(TimeSpan.FromSeconds(duration));
        IsInvincible = false;
    }

}
