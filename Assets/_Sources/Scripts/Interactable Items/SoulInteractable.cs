using UnityEngine;
using Cysharp.Threading.Tasks;

public class SoulInteractable : Interactable
{
    [SerializeField] private GameObject objectToEnableAfterHelp;
    [SerializeField] private NPC npcToRestore;

    [Header("Som de cura")]
    [SerializeField] private AudioClip healingSFX;
    [SerializeField] private AudioSource audioSource;

    [Header("Vida Visual (em cena)")]
    [SerializeField] private Transform heartIconWorldObject;
    [SerializeField] private GameObject heartVisualPrefab; 
    [SerializeField] private float moveSpeed = 4f;

    private bool isHelped = false;

    public override void Interact()
    {
        if (isHelped) return;
        if (!PlayerIsClose()) return;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;

        PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
        if (playerHealth == null || playerHealth.CurrentHealth < 2f) return;

        playerHealth.TakeDamage(1f);
        isHelped = true;

        AnimateSpawnedHeartToSoul().Forget();
    }

    private async UniTaskVoid AnimateSpawnedHeartToSoul()
    {
        if (heartIconWorldObject == null || heartVisualPrefab == null)
        {
            FinalizeHealing();
            return;
        }

        GameObject heartInstance = Instantiate(heartVisualPrefab, heartIconWorldObject.position, Quaternion.identity);
        Transform heartTransform = heartInstance.transform;

        Vector3 start = heartTransform.position;
        Vector3 target = transform.position;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            heartTransform.position = Vector3.Lerp(start, target, EaseOutQuad(t));
            await UniTask.Yield();
        }

        await UniTask.Delay(200);
        Destroy(heartInstance);

        FinalizeHealing();
    }

    private void FinalizeHealing()
    {
        if (objectToEnableAfterHelp != null)
            objectToEnableAfterHelp.SetActive(true);

        if (healingSFX != null)
        {
            if (audioSource != null)
                audioSource.PlayOneShot(healingSFX);
            else
                AudioSource.PlayClipAtPoint(healingSFX, transform.position);
        }

        if (npcToRestore != null)
            npcToRestore.ReactivateAfterHealing();

        gameObject.SetActive(false);
    }

    private float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);
}
