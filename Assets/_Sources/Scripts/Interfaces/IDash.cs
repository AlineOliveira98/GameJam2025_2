using UnityEngine;

public interface IDash
{

    bool CanDash { get; }
    void TryDash(Vector2 direction);

}
