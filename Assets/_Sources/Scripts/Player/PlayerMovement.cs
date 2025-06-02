using System;
using System.Threading.Tasks;
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

    private Vector2 target;
    private Vector2 lastDirection = Vector2.down;
    private Player player;
    private Camera mainCamera;
    private bool movementLocked;

    public float speedMultiplier { get; private set; } = 1f;

    void OnEnable()
    {
        DialogueService.OnDialogueStarted += Lock;
        DialogueService.OnDialogueFinished += Unlock;
        PlayerHealth.OnPlayerDied += Lock;
    }

    void OnDisable()
    {
        DialogueService.OnDialogueStarted -= Lock;
        DialogueService.OnDialogueFinished -= Unlock;
        PlayerHealth.OnPlayerDied -= Lock;
    }

    private void Lock() => movementLocked = true;
    private void Lock(DialogueSO dialogueSO) => movementLocked = true;
    private void Unlock() => movementLocked = false;

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
        if (movementLocked) return;
        MoveClick();
    }

    private void FixedUpdate()
    {
        if (movementLocked) return;
        MoveInput();
    }

    private void MoveInput()
    {
        if (movementByClick) return;

        Vector2 movement = player.input * (moveSpeed * speedMultiplier); ;
        player.rb.linearVelocity = movement;

        if (player.input != Vector2.zero)
            lastDirection = player.input.normalized;

        player.Visual.SetDirection(lastDirection);
        player.Visual.SetRunning(player.rb.linearVelocity != Vector2.zero);
    }

    private void MoveClick()
    {
        if (!movementByClick) return;

        if (Input.GetMouseButton(0))
        {
            var mousePos = Input.mousePosition;
            var worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(worldPos, out hit, 1.0f, NavMesh.AllAreas))
            {
                target = hit.position;
                agent.SetDestination(target);

                lastDirection = ((Vector2)agent.velocity).normalized;
            }
        }

        if (target == Vector2.zero) return;

        bool isRunning = !agent.pathPending && (agent.remainingDistance > agent.stoppingDistance);
        var dir = isRunning
            ? ((Vector2)agent.velocity).normalized
            : lastDirection;

        player.Visual.SetDirection(dir);
        player.Visual.SetRunning(isRunning);
    }
    
    public async void SetSpeedMultiplier(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        await Task.Delay(TimeSpan.FromSeconds(duration));
        speedMultiplier = 1f;
    }
}
