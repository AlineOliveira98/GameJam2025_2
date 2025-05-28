using UnityEngine;

public class NPC : MonoBehaviour, ICollectable, IDamageable
{
    public bool IsDead { get; set; }

    public void Collect()
    {
        Debug.Log("NPC Collected");
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        IsDead = true;
        gameObject.SetActive(false);
        Debug.Log("NPC Dead");
    }
}
