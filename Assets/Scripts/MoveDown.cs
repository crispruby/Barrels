using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
  public float speed = 5.0f;
  private float zAxisDestroy = -10.0f;
  private Rigidbody objectRb;
  public float speedMultiplier = 1.0f;
  private GameManager gameManager;

  void Start()
  {
    objectRb = GetComponent<Rigidbody>();
    objectRb.useGravity = true;
    // Freeze rotation along the X and Y axes to prevent wobbling
    objectRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
    if (gameManager == null)
    {
      Debug.LogError("GameManager not found! Please ensure it exists in the scene.");
    }
  }
using UnityEngine;

public class MoveDown : FallingObject
{
    public float speedMultiplier = 1.0f;
    private GameManager gameManager;

    protected override void Start()
    {
        base.Start();
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found! Please ensure it exists in the scene.");
        }
    }

    protected override void Update()
    {
        if (!gameManager?.isGameActive ?? false)
        {
            objectRb.velocity = Vector3.zero;
            return;
        }

        objectRb.velocity = new Vector3(0, 0, -speed * speedMultiplier);

        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        objectRb.AddForce(Vector3.down * GameConfig.GravityForce);
        StabilizeVelocity();
        RotateBarrel();
    }

    private void StabilizeVelocity()
    {
        Vector3 currentVelocity = objectRb.velocity;
        currentVelocity.x = 0;
        objectRb.velocity = currentVelocity;
    }

    private void RotateBarrel()
    {
        float rotationAmount = (speed / 4f) * speedMultiplier * Time.fixedDeltaTime * GameConfig.BarrelRotationFactor;
        transform.Rotate(Vector3.up, rotationAmount);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagNames.Player))
        {
            Destroy(gameObject);
        }
    }
}

  void Update()
  {
    if (!gameManager.isGameActive)
    {
      objectRb.velocity = Vector3.zero; // Stop barrel movement
      return;
    }
    // Directly control the velocity to ensure the barrels move only along the Z-axis
    objectRb.velocity = new Vector3(0, 0, -speed);

    // Destroy the barrel once it passes below the defined Z-axis threshold
    if (transform.position.z < zAxisDestroy)
    {
      Destroy(gameObject);
    }
  }

  void FixedUpdate()
  { 
    objectRb.AddForce(Vector3.down* 50f);
    // Stabilize the barrel's X-axis velocity to ensure it stays centered
    Vector3 currentVelocity = objectRb.velocity;
    currentVelocity.x = 0; // Zero out any sideways movement
    objectRb.velocity = currentVelocity;
    float rotationAmount = (speed / 4) * speedMultiplier * Time.fixedDeltaTime * 360f; // Reduced rotation speed by half
    transform.Rotate(Vector3.up, rotationAmount);
  }
  void OnCollisionEnter(Collision collision)
  {
    // Check if the barrel collides with the player
    if (collision.gameObject.CompareTag("Player")) // Ensure the player has the "Player" tag
    {
      Destroy(gameObject); // Destroy the barrel
    }
  }
}
