using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
  private float strafeSpeed = 3f;
  private int playerLives = 3;
  private Rigidbody rb;
  private int health = 100;
  private Animator animator;
  private float initialZPosition;
  private Quaternion uprightRotation;
  private Vector3 lastPosition;
  public GameObject SmokeyExplosion;
  private BoxCollider boxCollider;
  private float XaxisLimit = 5;
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

  void Start()
  {
    points = 0;
    gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
    rb = rb = GetComponentInParent<Rigidbody>();
    animator = GetComponent<Animator>();
    boxCollider = GetComponent<BoxCollider>();
    audioSource = Camera.main.GetComponent<AudioSource>();
    initialZPosition = transform.position.z; // Store the initial Z position
    uprightRotation = Quaternion.Euler(0, 0, 0); // Store the upright rotation
    animator.SetFloat("Speed", 0f);
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
    Move();
    UpdateAnimation();
    LockZAxisPosition();
    LimitXAxis();
  }
  void Move()
  {
    if (!gameManager.isGameActive)
    {
      return; // Stop processing movement
    }
    float moveHorizontal = 0f;
    float moveVertical = 0f;

    if (Input.GetKey(KeyCode.UpArrow))
    {
      moveVertical = 1f; // Run animation
      animator.SetFloat("Speed", 1f);
      SetBarrelSpeedMultiplier(4f / 3f);
    }
    else if (Input.GetKeyUp(KeyCode.UpArrow))
    {
      SetBarrelSpeedMultiplier(3f / 4f); // Reset to default speed
    }
    else
    {
      animator.SetFloat("Speed", 0f); // Idle animation
    }

    if (Input.GetKey(KeyCode.LeftArrow))
    {
      moveHorizontal = -1f; // Strafe left animation
      transform.Translate(Vector3.left * strafeSpeed * Time.deltaTime); // Move left
    }
    else if (Input.GetKey(KeyCode.RightArrow))
    {
      moveHorizontal = 1f; // Strafe right animation
      transform.Translate(Vector3.right * strafeSpeed * Time.deltaTime); // Move right
    }

    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
    // No need to move the character forward or backward since the environment moves
    lastPosition = transform.position;
  }
  void SetBarrelSpeedMultiplier(float multiplier)
  {
    MoveDown[] barrels = FindObjectsOfType<MoveDown>(); // Find all active barrels
    foreach (MoveDown barrel in barrels)
    {
      barrel.speedMultiplier = multiplier; // Update multiplier on each barrel
    }
  }
  void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Ground"))
    {
    }
    else if (other.gameObject.CompareTag("Powerup"))
    {
      Destroy(other.gameObject);
      points += 2;
      health += 20;
      UpdateHealthUI();
      UpdatePointsUI();
      GameObject spark = Instantiate(redPowerupEffect, transform.position, Quaternion.identity);
      Destroy(spark, 1.5f);
      if (audioSource && redPowerupSound)
      {
        audioSource.PlayOneShot(redPowerupSound);
      }
      transform.position = new Vector3(lastPosition.x, 0, initialZPosition); // Reset to a safe position
      transform.rotation = Quaternion.Euler(0, 0, 0); // Reset to standing and facing forward
      rb.velocity = Vector3.zero; // Reset Rigidbody's velocity
      rb.angularVelocity = Vector3.zero; // Reset angular velocity
      Destroy(other.gameObject);
    }
    else if (other.gameObject.CompareTag("Power1"))
    {
      Destroy(other.gameObject);
      health += 20;
      points += 2;
      UpdateHealthUI();
      UpdatePointsUI();
      GameObject spark = Instantiate(greenPowerupEffect, transform.position, Quaternion.identity);
      Destroy(spark, 1.5f);
      if (audioSource && greenPowerupSound)
      {
        audioSource.PlayOneShot(greenPowerupSound);
      }
      transform.position = new Vector3(lastPosition.x, 0, initialZPosition); // Reset to a safe position
      transform.rotation = Quaternion.Euler(0, 0, 0); // Reset to standing and facing forward
      rb.velocity = Vector3.zero; // Reset Rigidbody's velocity
      rb.angularVelocity = Vector3.zero; // Reset angular velocity
      Destroy(other.gameObject);
    }
    else if (other.gameObject.CompareTag("RustyBarrel"))
    {
      GameObject spark = Instantiate(rustyBarrelEffect, transform.position, Quaternion.identity);
      Destroy(spark, 2);
      if (audioSource && rustyBarrelSound)
      {
        audioSource.PlayOneShot(rustyBarrelSound);
      }
      calculateHealth(5);
      Destroy(other.gameObject);
    }
    else if (other.gameObject.CompareTag("GasBarrel"))
    {
      GameObject spark = Instantiate(gasBarrelEffect, transform.position, Quaternion.identity);
      Destroy(spark, 2);
      if (audioSource && gasBarrelSound)
      {
        audioSource.PlayOneShot(gasBarrelSound);
      }
      calculateHealth(10);
      Destroy(other.gameObject);
    }
    else if (other.gameObject.CompareTag("ToxicBarrel"))
    {
      GameObject spark = Instantiate(metalBarrelEffect, transform.position, Quaternion.identity);
      Destroy(spark, 2);
      if (audioSource && metalBarrelSound)
      {
        audioSource.PlayOneShot(metalBarrelSound);
      }
      calculateHealth(5);
      Destroy(other.gameObject);
    }
    else if (other.gameObject.CompareTag("StripedBarrel"))
    {
      GameObject spark = Instantiate(stripedBarrelEffect, transform.position, Quaternion.identity);
      Destroy(spark, 2);
      if (audioSource && stripedBarrelSound)
      {
        audioSource.PlayOneShot(stripedBarrelSound);
      }
      calculateHealth(10);
      Destroy(other.gameObject);
    }
  }

  private void calculateHealth(int damage)
  {
    health -= damage;
    UpdateHealthUI();
    if (health <= 0)
    {
      gameManager.ClearObjects();
      playerLives--;
      UpdateLivesUI();
      health = 100;
      if (playerLives > 0)
      {
        UpdateHealthUI();
        RespawnPlayer();
      }
      else
      {
        UpdateHealthUI();
        RespawnPlayer();
        TriggerGameOver();
      }
    }
    else
    {
      // Reposition and reset rotation
      transform.position = new Vector3(lastPosition.x, 0, initialZPosition); // Ensure initialZPosition is used
      transform.rotation = Quaternion.Euler(0, 0, 0); // Reset to standing and facing forward
      rb.velocity = Vector3.zero; // Reset Rigidbody's velocity
      rb.angularVelocity = Vector3.zero; // Reset angular velocity
      animator.SetFloat("Speed", 0f); // Idle animation
    }
  }
  void RespawnPlayer()
  {
    GameObject smoke = Instantiate(SmokeyExplosion, transform.position, Quaternion.identity);
    Destroy(smoke, 2f);
    transform.position = new Vector3(lastPosition.x, 0, initialZPosition);
    transform.rotation = Quaternion.Euler(0, 0, 0);
    rb.velocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
  }
  void UpdateAnimation()
  {
    if (Input.GetKey(KeyCode.UpArrow))
    {
      animator.SetFloat("Speed", 1f); // Run animation
    }
    else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
    {
      animator.SetFloat("Speed", 0.5f); // Strafe animation (adjust if you have specific strafe animations)
    }
    else
    {
      animator.SetFloat("Speed", 0f); // Idle animation
    }
  }
  void TriggerGameOver()
  {
    animator.SetFloat("Speed", 0f); // Idle animation
    rb.velocity = Vector3.zero; // Reset Rigidbody velocity
    rb.angularVelocity = Vector3.zero; // Reset Rigidbody angular velocity
    transform.position = new Vector3(transform.position.x, 0, initialZPosition); // Keep position in place
    transform.rotation = Quaternion.Euler(0, 0, 0);
    gameManager.GameOver();
  }
  void UpdatePointsUI()
  {
    pointsText.text = "Points: " + points;
    if (points % 10 == 0 && points > 0)
    {
      gameManager.spawnManager.AdjustEnemySpawnTime(-0.1f); // Reduce enemy spawn time by 0.01
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
    // Lock the character's Z position
    Vector3 position = transform.position;
    position.z = initialZPosition;
    transform.position = position;
  }
  void LimitXAxis()
  {
    Vector3 position = transform.position;
    position.x = Mathf.Clamp(position.x, -XaxisLimit, XaxisLimit);
    transform.position = position;
  }
}