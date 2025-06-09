using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnableColliderWithDelay : MonoBehaviour
{
    [SerializeField] private float delayInSeconds = 2f;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private async void OnEnable()
    {
        if (col == null)
            col = GetComponent<Collider2D>();

        if (col != null)
            col.enabled = false;

        await UniTask.Delay((int)(delayInSeconds * 1000));

        if (col != null)
            col.enabled = true;

        Debug.Log($"Collider ativado após {delayInSeconds} segundos.");
    }
}
