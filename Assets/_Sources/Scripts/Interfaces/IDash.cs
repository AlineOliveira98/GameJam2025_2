using UnityEngine;

public interface IDash
{

    bool CanDash { get; set; }
    bool IsDashing { get; }
    void TryDash(Vector2 direction);
    void CooldownDash();

}
