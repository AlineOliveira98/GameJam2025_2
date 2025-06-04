using UnityEngine;

public interface IDash
{

    bool CanDash { get; }
    bool IsDashing { get; }
    void TryDash(Vector2 direction);

}
