using System;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TargetIndicator targetIndicator;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private float baseSpeed = 5f;

    public Rigidbody2D rb { get; private set; }
    public Vector2 input { get; private set; }
    public PlayerHealth Health => health;

    public float speedMultiplier { get; private set; } = 1f;

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
}
