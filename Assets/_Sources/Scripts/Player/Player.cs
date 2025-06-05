using System;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float rangePushEnemies;
    [SerializeField] private float forcePush;
    [SerializeField] private float durationPush;

    [SerializeField] private TargetIndicator targetIndicator;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerVisual visual;

    private Vector2 lastMoveDir;

    public Rigidbody2D rb { get; private set; }
    public Vector2 input { get; private set; }
    public PlayerHealth Health => health;
    public PlayerVisual Visual => visual;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (visual == null)
            visual = GetComponent<PlayerVisual>();
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

        if (input != Vector2.zero)
            lastMoveDir = input;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!SkillController.Instance.HasSkill(SkillType.Push)) return;
            PushEnemies();
        }
    }

    private void PushEnemies()
    {
        var enemiesInRange = Physics2D.OverlapCircleAll(transform.position, rangePushEnemies, 1 << 9);

        if (enemiesInRange.Length <= 0) return;

        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            if (enemiesInRange[i].TryGetComponent(out Enemy enemy))
            {
                enemy.KnockBack(transform.position, forcePush, durationPush);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangePushEnemies);
    }
}
