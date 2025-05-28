using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Vector2 input;

    public bool IsDead { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    
    public void TakeDamage(float damage)
    {
        
    }
}
