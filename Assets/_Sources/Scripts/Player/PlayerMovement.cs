using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool movementByClick;
    [SerializeField] private float moveSpeed = 5f;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerVisual visual;

    private Vector2 target;
    private Player player;
    private Camera mainCamera;

    private void Awake()
    {
        player = GetComponent<Player>();
        mainCamera = Camera.main;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        MoveClick();
    }

    private void FixedUpdate()
    {
        MoveInput();
    }

    private void MoveInput()
    {
        if (movementByClick) return;

        Vector2 movement = player.input * moveSpeed;
        player.rb.linearVelocity = movement;

        visual.SetDirection(player.input);
        visual.SetRunning(player.rb.linearVelocity != Vector2.zero);
    }

    private void MoveClick()
    {
        if (!movementByClick) return;

        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            var worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(worldPos, out hit, 1.0f, NavMesh.AllAreas))
            {
                target = hit.position;
                agent.SetDestination(target);
            }
        }

        if (target == Vector2.zero) return;

        var dir = (target - (Vector2) transform.position).normalized;
        bool isRunning = !agent.pathPending && (agent.remainingDistance > agent.stoppingDistance);

        visual.SetDirection(dir);
        visual.SetRunning(isRunning);        
    }
}
