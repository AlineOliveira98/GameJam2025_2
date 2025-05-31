using System;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private TargetIndicator targetIndicator;
    public Rigidbody2D rb { get; private set; }
    public Vector2 input { get; private set; }

    public bool IsDead { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PointerTest();
    }

    private async Task PointerTest()
    {
        await Task.Delay(3 * 1000);
        var npc = FindAnyObjectByType<NPC>();
        targetIndicator.SetTarget(npc.transform);
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Taking Damage");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();
        }
    }
}
