using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    private float strafeSpeed = GameConfig.PlayerStrafeSpeed;
    private int playerLives = GameConfig.PlayerInitialLives;
    private Rigidbody rb;
    private int health = GameConfig.PlayerInitialHealth;
    private Animator animator;
    private float initialZPosition;
    private Vector3 lastPosition;
    public GameObject SmokeyExplosion;
    private BoxCollider boxCollider;
    private float xAxisLimit = GameConfig.PlayerXAxisLimit;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI livesText;
    public AudioClip redPowerupSound;
    public GameObject redPowerupEffect;
    public AudioClip greenPowerupSound;
    public GameObject greenPowerupEffect;
    public AudioClip metalBarrelSound;
    public GameObject metalBarrelEffect;
    public AudioClip gasBarrelSound;
    public GameObject gasBarrelEffect;
    public AudioClip rustyBarrelSound;
    public GameObject rustyBarrelEffect;
    public AudioClip stripedBarrelSound;
    public GameObject stripedBarrelEffect;
    private int points;
    private AudioSource audioSource;
    private GameManager gameManager;
    private readonly int speedHash = AnimatorHashes.Speed;

    void Start()
    {
        points = 0;
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        audioSource = Camera.main?.GetComponent<AudioSource>();
        initialZPosition = transform.position.z;
        animator.SetFloat(speedHash, 0f);
        animator.Play("Idle");
        UpdateHealthUI();
        UpdateLivesUI();
        UpdatePointsUI();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found! Please ensure it exists in the scene.");
        }
    }
    void Update()
    {
        if (!gameManager?.isGameActive ?? false)
        {
            return;
        }
        Move();
        UpdateAnimation();
        LockZAxisPosition();
        LimitXAxis();
    }
    void Move()
    {
        float moveHorizontal = 0f;
        if (Input.GetKey(InputConfig.MoveForward))
        {
            SetBarrelSpeedMultiplier(GameConfig.PlayerForwardSpeedMultiplier);
        }
        else if (Input.GetKeyUp(InputConfig.MoveForward))
        {
            SetBarrelSpeedMultiplier(GameConfig.PlayerDefaultSpeedMultiplier);
        }
        if (Input.GetKey(InputConfig.MoveLeft))
        {
            moveHorizontal = -1f;
            transform.Translate(Vector3.left * strafeSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(InputConfig.MoveRight))
        {
            moveHorizontal = 1f;
            transform.Translate(Vector3.right * strafeSpeed * Time.deltaTime);
        }
        lastPosition = transform.position;
    }
    void SetBarrelSpeedMultiplier(float multiplier)
    {
        MoveDown[] barrels = FindObjectsOfType<MoveDown>();
        foreach (MoveDown barrel in barrels)
        {
            barrel.speedMultiplier = multiplier;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(TagNames.Ground))
        {
            return;
        }
        if (other.gameObject.CompareTag(TagNames.Powerup))
        {
            HandlePowerupCollision(other.gameObject, redPowerupEffect, redPowerupSound, 20, 2);
            return;
        }
        if (other.gameObject.CompareTag(TagNames.Power1))
        {
            HandlePowerupCollision(other.gameObject, greenPowerupEffect, greenPowerupSound, 20, 2);
            return;
        }
        if (other.gameObject.CompareTag(TagNames.RustyBarrel))
        {
            HandleBarrelCollision(other.gameObject, rustyBarrelEffect, rustyBarrelSound, 5);
            return;
        }
        if (other.gameObject.CompareTag(TagNames.GasBarrel))
        {
            HandleBarrelCollision(other.gameObject, gasBarrelEffect, gasBarrelSound, 10);
            return;
        }
        if (other.gameObject.CompareTag(TagNames.ToxicBarrel))
        {
            HandleBarrelCollision(other.gameObject, metalBarrelEffect, metalBarrelSound, 5);
            return;
        }
        if (other.gameObject.CompareTag(TagNames.StripedBarrel))
        {
            HandleBarrelCollision(other.gameObject, stripedBarrelEffect, stripedBarrelSound, 10);
            return;
        }
    }
    private void HandlePowerupCollision(GameObject powerup, GameObject effectPrefab, AudioClip soundClip, int healthGain, int pointsGain)
    {
        Destroy(powerup);
        points += pointsGain;
        health += healthGain;
        UpdateHealthUI();
        UpdatePointsUI();
        PlayEffect(effectPrefab, GameConfig.PowerupEffectDuration);
        PlaySound(soundClip);
        ResetPlayerAfterCollision();
    }
    private void HandleBarrelCollision(GameObject barrel, GameObject effectPrefab, AudioClip soundClip, int damage)
    {
        Destroy(barrel);
        PlayEffect(effectPrefab, GameConfig.BarrelEffectDuration);
        PlaySound(soundClip);
        AdjustHealth(damage);
    }
    private void AdjustHealth(int damage)
    {
        health -= damage;
        UpdateHealthUI();
        if (health <= 0)
        {
            gameManager.ClearObjects();
            playerLives--;
            UpdateLivesUI();
            health = GameConfig.PlayerInitialHealth;
            if (playerLives > 0)
            {
                RespawnPlayer();
            }
            else
            {
                RespawnPlayer();
                TriggerGameOver();
            }
            UpdateHealthUI();
            return;
        }
        ResetPlayerAfterCollision();
    }
    private void ResetPlayerAfterCollision()
    {
        animator.SetFloat(speedHash, 0f);
        ResetPlayerPosition();
    }
    private void ResetPlayerPosition()
    {
        transform.position = new Vector3(lastPosition.x, GameConfig.PlayerCollisionResetY, initialZPosition);
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    void RespawnPlayer()
    {
        GameObject smoke = Instantiate(SmokeyExplosion, transform.position, Quaternion.identity);
        Destroy(smoke, 2f);
        ResetPlayerPosition();
    }
    private void PlayEffect(GameObject effectPrefab, float duration)
    {
        if (effectPrefab == null)
        {
            return;
        }
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, duration);
    }
    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }
        audioSource.PlayOneShot(clip);
    }
    void UpdateAnimation()
    {
        if (Input.GetKey(InputConfig.MoveForward))
        {
            animator.SetFloat(speedHash, 1f);
        }
        else if (Input.GetKey(InputConfig.MoveLeft) || Input.GetKey(InputConfig.MoveRight))
        {
            animator.SetFloat(speedHash, 0.5f);
        }
        else
        {
            animator.SetFloat(speedHash, 0f);
        }
    }
    void TriggerGameOver()
    {
        animator.SetFloat(speedHash, 0f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, GameConfig.PlayerCollisionResetY, initialZPosition);
        transform.rotation = Quaternion.identity;
        gameManager.GameOver();
    }
    void UpdatePointsUI()
    {
        pointsText.text = "Points: " + points;
        if (points % 10 == 0 && points > 0)
        {
            gameManager.spawnManager.AdjustEnemySpawnTime(GameConfig.SpawnTimeAdjustment);
        }
    }
    void UpdateHealthUI()
    {
        healthText.text = "Health: " + health;
    }
    void UpdateLivesUI()
    {
        livesText.text = "Lives: " + playerLives;
    }
    void LockZAxisPosition()
    {
        Vector3 position = transform.position;
        position.z = initialZPosition;
        transform.position = position;
    }
    void LimitXAxis()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -xAxisLimit, xAxisLimit);
        transform.position = position;
    }
}
