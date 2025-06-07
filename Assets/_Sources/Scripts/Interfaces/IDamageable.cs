public interface IDamageable
{
    void TakeDamage(float damage, float damageDelay);
    bool IsDead { get; set; }
}