using UnityEngine;

public class OrcFast : Enemy
{
    [SerializeField] private EnemyMovement movement;

    private Vector2 direction;

    //TODO: Define when start dash and your direction
    public void Dash()
    {
        movement.Dash(direction);
    }
}