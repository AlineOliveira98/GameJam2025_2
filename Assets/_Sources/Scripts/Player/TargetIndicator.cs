using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private float distanceToHide = 10f;
    [SerializeField] private GameObject indicator;
    private Transform target;

    void Start()
    {
        ShowIndicator(false);
    }

    void Update()
    {
        if (target == null) return;

        var dir = target.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (dir.sqrMagnitude < distanceToHide)
        {
            ShowIndicator(false);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;

        ShowIndicator(target != null);
    }

    public void ShowIndicator(bool show) => indicator.SetActive(show);
}
