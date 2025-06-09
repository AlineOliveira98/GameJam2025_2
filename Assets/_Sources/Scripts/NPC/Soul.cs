using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] private GameObject objectToEnableAfterHelp;
    [Header("Som ao doar vida")]
    [SerializeField] private AudioClip healingSFX;
    [SerializeField] private AudioSource audioSource;

    private bool isHelped = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isHelped) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.CurrentHealth >= 2f)
            {
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

                gameObject.SetActive(false);
                isHelped = true;
            }
        }
    }
}
