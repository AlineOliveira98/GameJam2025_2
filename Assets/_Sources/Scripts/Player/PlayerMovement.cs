using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool movementByClick;
    [SerializeField] private float moveSpeed = 5f;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerVisual visual;

    private Vector2 target;
    private Vector2 lastDirection = Vector2.down;
    private Player player;
    private Camera mainCamera;

    public bool MovementLocked;

    void OnEnable()
    {
        DialogueService.OnDialogueStarted += Lock;
        DialogueService.OnDialogueFinished += Unlock;
    }

    void OnDisable()
    {
        DialogueService.OnDialogueStarted -= Lock;
        DialogueService.OnDialogueFinished -= Unlock;
    }

    private void Lock(DialogueSO dialogueSO) => MovementLocked = true;
    private void Unlock() => MovementLocked = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        mainCamera = Camera.main;

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = moveSpeed;
    }

    private void Update()
    {
        if(MovementLocked) return;
        MoveClick();
    }

    private void FixedUpdate()
    {
        if(MovementLocked) return;
        MoveInput();
    }

    private void MoveInput()
    {
        if (movementByClick) return;

        Vector2 movement = player.input * moveSpeed;
        player.rb.linearVelocity = movement;

        if (player.input != Vector2.zero)
            lastDirection = player.input.normalized;

        visual.SetDirection(lastDirection);
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

                lastDirection = (target - (Vector2) transform.position).normalized;
            }
        }

        if (target == Vector2.zero) return;

        bool isRunning = !agent.pathPending && (agent.remainingDistance > agent.stoppingDistance);
        var dir = isRunning ? (target - (Vector2) transform.position).normalized : lastDirection;

        visual.SetDirection(dir);
        visual.SetRunning(isRunning);        
    }
}
