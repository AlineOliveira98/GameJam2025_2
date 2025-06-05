using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

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
    private bool CanMove => GameController.GameStarted && !GameController.GameIsOver;

    private IDash dash;

    public float speedMultiplier { get; private set; } = 1f;

    private void OnEnable()
    {
        WaterWell.OnTeleportActivated += Teleport;
    }

    private void OnDisable()
    {
        WaterWell.OnTeleportActivated -= Teleport;
    }

    private void Start()
    {
        dash = new BasicDash(player.rb, 50f, 0.03f, 0.5f, agent);
    }

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
        if (movementLocked || !CanMove) return;

        MoveClick();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!dash.CanDash) return;
            dash.TryDash(lastDirection);
        }
    }

    private void FixedUpdate()
    {
        if (movementLocked || !CanMove) return;
        MoveInput();
    }

    private void Teleport(WaterWell well)
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.Warp(well.spawnArea.position);
        agent.isStopped = false;
    }

    private void MoveInput()
    {
        if (movementByClick) return;
        if (!dash.CanDash) return;

        Vector2 movement = player.input * (moveSpeed * speedMultiplier);
        player.rb.linearVelocity = movement;

        if (player.input != Vector2.zero)
            lastDirection = player.input.normalized;

        player.Visual?.SetDirection(lastDirection);
        player.Visual?.SetRunning(player.rb.linearVelocity != Vector2.zero);
    }

    private void MoveClick()
    {
        if (!movementByClick || agent == null || !agent.enabled || !dash.CanDash) return;

        if (Input.GetMouseButton(0))
        {
            var mousePos = Input.mousePosition;
            var worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            if (NavMesh.SamplePosition(worldPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                target = hit.position;
                agent.SetDestination(target);
                if (dash is BasicDash basicDash)
                {
                    basicDash.SetDestinationAfterDash(target); // garante que o destino ser� reaplicado depois do dash
                }

                lastDirection = ((Vector2)agent.velocity).normalized;
            }
        }

        if (target == Vector2.zero) return;

        bool isRunning = agent.enabled && !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
        var dir = isRunning
            ? ((Vector2)agent.velocity).normalized
            : lastDirection;

        player.Visual?.SetDirection(dir);
        player.Visual?.SetRunning(isRunning);
    }

    public async void SetSpeedMultiplier(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        await Task.Delay(TimeSpan.FromSeconds(duration));
        speedMultiplier = 1f;
    }

    
}
