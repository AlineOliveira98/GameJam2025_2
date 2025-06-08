using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float rangePushEnemies;
    [SerializeField] private float forcePush;
    [SerializeField] private float durationPush;
    [SerializeField] private float cooldownPush;
    [SerializeField] private GameObject vfxPush;

    [Header("Invisible Settings")]
    [SerializeField] private float durationInvisible;
    [SerializeField] private float cooldownInvisible;
    [SerializeField] private AudioClip sfxInvisible;

    [Header("Clone Settings")]
    [SerializeField] private float durationClone;
    [SerializeField] private float cooldownClone;
    [SerializeField] private PlayerClone playerClone;
    [SerializeField] private AudioClip sfxClone;

    [Header("References")]
    [SerializeField] private TargetIndicator targetIndicator;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerVisual visual;

    private bool canPush = true;
    private bool canStayInvisible = true;
    private bool canClone = true;
    private Camera cam;

    public bool IsInvisible { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Vector2 input { get; private set; }
    public PlayerHealth Health => health;
    public PlayerVisual Visual => visual;
    public PlayerMovement Movement { get; private set; }

    public static Action<SkillType, float> OnSkillUsed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (visual == null)
            visual = GetComponent<PlayerVisual>();

        Movement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        cam = Camera.main;
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

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!SkillController.Instance.HasSkill(SkillType.Push)) return;
            PushEnemies();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!SkillController.Instance.HasSkill(SkillType.Invisible)) return;
            Invisible();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!SkillController.Instance.HasSkill(SkillType.Clone)) return;
            Clone();
        }
    }

    private async UniTask PushEnemies()
    {
        if (!canPush)
        {
            Debug.LogError("Not can push!");
            return;
        }

        canPush = false;
        OnSkillUsed?.Invoke(SkillType.Push, cooldownPush);

        var enemiesInRange = Physics2D.OverlapCircleAll(transform.position, rangePushEnemies, 1 << 9);

        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            if (enemiesInRange[i].TryGetComponent(out Enemy enemy))
            {
                enemy.KnockBack(transform.position, forcePush, durationPush);
            }
        }

        var vfx = Instantiate(vfxPush, transform.position, Quaternion.identity);

        await UniTask.Delay(TimeSpan.FromSeconds(cooldownPush));
        canPush = true;
        Destroy(vfx);
    }

    private async UniTask Invisible()
    {
        if (!canStayInvisible) return;
        canStayInvisible = false;
        OnSkillUsed?.Invoke(SkillType.Invisible, cooldownInvisible);

        IsInvisible = true;
        visual.SetInvisible(true);
        Health.SetInvincibility(durationInvisible);
        AudioController.PlaySFX(sfxInvisible);
        
        await UniTask.Delay(TimeSpan.FromSeconds(durationInvisible));

        IsInvisible = false;
        visual.SetInvisible(false);

        await UniTask.Delay(TimeSpan.FromSeconds(cooldownInvisible));
        canStayInvisible = true;
    }

    private async UniTask Clone()
    {
        if (!canClone) return;
        canClone = false;
        OnSkillUsed?.Invoke(SkillType.Clone, cooldownClone);

        Vector3 startPos = transform.position;
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (mouseWorld - (Vector2)startPos).normalized;

        playerClone.gameObject.SetActive(true);
        playerClone.Initialize(transform.position, mouseDirection);
        AudioController.PlaySFX(sfxClone);

        await UniTask.Delay(TimeSpan.FromSeconds(durationClone));

        playerClone.Disable();
        
        await UniTask.Delay(TimeSpan.FromSeconds(cooldownClone));
        canClone = true;
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
