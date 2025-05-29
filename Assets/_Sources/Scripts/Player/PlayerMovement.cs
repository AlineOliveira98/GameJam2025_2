using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        Vector2 movement = player.input * moveSpeed;
        player.rb.linearVelocity = movement;
    }
}
