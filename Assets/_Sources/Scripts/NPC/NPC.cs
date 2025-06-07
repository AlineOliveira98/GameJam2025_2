using System;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;


public class NPC : MonoBehaviour, ICollectable, IDamageable
{
    [SerializeField] private float rangeToAskHelp;
    [SerializeField] private float rateToAskHelp;
    [SerializeField] private GameObject[] helpBaloonsPrefab;

    [SerializeField] private float dieAnimDuration = 1.5f;

    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController overrideController;

    private float lastCallHelp;
    private Camera cam;

    public bool IsDead { get; set; }
    public bool IsSaved { get; set; }
    public bool LockedInteraction { get; set; } = false;

    public Animator Animator => animator;

    void Start()
    {
        cam = Camera.main;
        if(overrideController != null) animator.runtimeAnimatorController = overrideController;
    }

    void Update()
    {
        if (!GameController.GameStarted || GameController.GameIsOver) return;
        
        if (IsDead || IsSaved) return;
        
        CheckAreInDanger();
    }

    private void CheckAreInDanger()
    {
        var enemyInRange = Physics2D.OverlapCircle(transform.position, rangeToAskHelp, 1 << 9);

        if (enemyInRange != null)
        {

            if (Time.time >= lastCallHelp + rateToAskHelp)
            {
                AskForHelp();
                lastCallHelp = Time.time;
            }
        }
    }

    private void AskForHelp()
    {
        if (IsVisibleToCamera()) return;

        int randomBallon = UnityEngine.Random.Range(0, helpBaloonsPrefab.Length);
        Vector3 spawnPos = GetScreenEdgePosition();
        Instantiate(helpBaloonsPrefab[randomBallon], spawnPos, Quaternion.identity);
    }

    private Vector3 GetScreenEdgePosition()
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);
        viewportPos.z = Mathf.Abs(cam.transform.position.z); 

        viewportPos.x = Mathf.Clamp01(viewportPos.x);
        viewportPos.y = Mathf.Clamp01(viewportPos.y);

        if (viewportPos.x <= 0) viewportPos.x = 0;
        if (viewportPos.x >= 1) viewportPos.x = 1;
        if (viewportPos.y <= 0) viewportPos.y = 0;
        if (viewportPos.y >= 1) viewportPos.y = 1;

        Vector3 screenPos = cam.ViewportToScreenPoint(viewportPos);
        return cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, viewportPos.z));
    }

    private bool IsVisibleToCamera()
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0;
    }

    public void Collect()
    {
        if (IsDead || IsSaved || LockedInteraction) return;

        IsSaved = true;
        gameObject.SetActive(false);

        GameController.Instance.SaveAnimal(this);
    }

    public async void TakeDamage(float damage, float damageDelay)
    {
        if (IsDead || IsSaved) return;

        IsDead = true;

        if (animator != null)
        {
            animator.SetBool("IsDead", true);
            await UniTask.Delay((int)(dieAnimDuration * 1000));
        }

        gameObject.SetActive(false);
        GameController.Instance.KillAnimal(this);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeToAskHelp);
    }
}
