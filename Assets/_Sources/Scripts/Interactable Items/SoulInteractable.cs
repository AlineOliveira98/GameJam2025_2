using UnityEngine;

public class SoulInteractable : Interactable
{
    [SerializeField] private GameObject objectToEnableAfterHelp;
    [SerializeField] private NPC npcToRestore;
    [SerializeField] private float colliderDelay = 2f;

    [Header("Som de cura")]
    [SerializeField] private AudioClip healingSFX;
    [SerializeField] private AudioSource audioSource;

    private bool isHelped = false;

    public override void Interact()
    {
        if (isHelped) return;
        if (!PlayerIsClose()) return;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;

        PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
        if (playerHealth == null) return;

        if (playerHealth.CurrentHealth < 2f) return;


        playerHealth.TakeDamage(1f);


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

        isHelped = true;
        gameObject.SetActive(false);
    }
}
