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