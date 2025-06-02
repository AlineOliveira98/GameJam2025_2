using UnityEngine;

public class NPC : MonoBehaviour, ICollectable, IDamageable
{
    [SerializeField] private float rangeToAskHelp;
    [SerializeField] private float rateToAskHelp;
    [SerializeField] private GameObject[] helpBaloonsPrefab;

    private float lastCallHelp = -Mathf.Infinity;
    private Camera cam;

    public bool IsDead { get; set; }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
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

        int randomBallon = Random.Range(0, helpBaloonsPrefab.Length);
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
        gameObject.SetActive(false);

        GameController.Instance.SaveVictim();
    }

    public void TakeDamage(float damage)
    {
        IsDead = true;
        gameObject.SetActive(false);

        GameController.Instance.KillVictim();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeToAskHelp);
    }
}
