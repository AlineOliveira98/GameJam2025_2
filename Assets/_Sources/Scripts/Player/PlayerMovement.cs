using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool movementByClick;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AudioClip dashAudio;
    [SerializeField] private ParticleSystem dustEffect;

    private Vector2 target;
    private Vector2 lastDirection = Vector2.down;
    private Player player;
    private Camera mainCamera;
    private bool movementLocked;
    private bool wasRunning = false;

    [SerializeField] private TrailRenderer trailRenderer;

    private bool CanMove => GameController.GameStarted && !GameController.GameIsOver;
    public NavMeshAgent Agent => agent;

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
        dash = new BasicDash(player.rb, dashSpeed, dashDuration, dashCooldown, agent, trailRenderer);
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
        if (movementLocked || !CanMove || player.Health.IsDead) return;

        MoveClick();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!dash.CanDash) return;
            dash.TryDash(lastDirection);
            AudioController.PlaySFX(dashAudio);
        }

        UpdateDustEffect(!Agent.isStopped);
    }

    private void FixedUpdate()
    {
        if (movementLocked || !CanMove || player.Health.IsDead) return;
        MoveInput();
    }

    public void Stop()
    {
        Agent.isStopped = true;
        agent.ResetPath();
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
        if (!movementByClick || agent == null || !agent.enabled) return;

        if (Input.GetMouseButton(0))
        {
            var mousePos = Input.mousePosition;
            var worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Interactable>()) return;

            if (NavMesh.SamplePosition(worldPos, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                target = navHit.position;
                agent.SetDestination(target);

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
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        speedMultiplier = 1f;
    }

    private void UpdateDustEffect(bool isRunning)
    {
        if (isRunning && !wasRunning)
            dustEffect.Play();
        else if (!isRunning && wasRunning)
            dustEffect.Stop();

        wasRunning = isRunning;
    }
}
