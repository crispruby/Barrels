using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
  public float speed = 5.0f;
  private float zAxisDestroy = -10.0f;
  private Rigidbody objectRb;

  void Start()
  {
    objectRb = GetComponent<Rigidbody>();
    objectRb.useGravity = true;
    // Freeze rotation along the X and Y axes to prevent wobbling
    objectRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
  }

  void Update()
  {
    // Directly control the velocity to ensure the barrels move only along the Z-axis
    objectRb.velocity = new Vector3(0, 0, -speed);

    // Destroy the barrel once it passes below the defined Z-axis threshold
    if (transform.position.z < zAxisDestroy)
    {
      Destroy(gameObject);
    }
  }
}