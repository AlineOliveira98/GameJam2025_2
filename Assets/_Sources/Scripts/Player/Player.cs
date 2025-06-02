using System;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TargetIndicator targetIndicator;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerVisual visual;

    public Rigidbody2D rb { get; private set; }
    public Vector2 input { get; private set; }
    public PlayerHealth Health => health;
    public PlayerVisual Visual => visual;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        YggDrasil.OnTreeWatered += WateringAnimation;
    }

    void OnDisable()
    {
        YggDrasil.OnTreeWatered -= WateringAnimation;
    }

    private void WateringAnimation()
    {
        Visual.SetWatering();
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
}
