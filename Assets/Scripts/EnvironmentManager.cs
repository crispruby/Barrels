using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
  public GameObject[] walls;
  public GameObject[] floorTiles;
  public GameObject[] objects; // Array for decorative objects
  public float moveSpeed = 1.5f;
  public float resetPositionZ = 15f; // Position to reset the tiles and objects
  public float thresholdZ = -9f; // Threshold position for resetting
  public float wallThresholdZ = -15f;
  void Update()
  {
    if (Input.GetKey(KeyCode.UpArrow))
    {
      MoveElements();
    }
  }
  void MoveElements()
  {
    MoveWalls(walls);
    MoveArray(floorTiles);
    MoveArray(objects); // Move decorative objects
  }

  void MoveArray(GameObject[] elements)
  {
    foreach (GameObject element in elements)
    {
      MoveElement(element);
    }
  }
  void MoveElement(GameObject element)
  {
    element.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

    // Check if the element has reached the threshold position and reset it
    if (element.transform.position.z <= thresholdZ)
    {
      element.transform.position = new Vector3(element.transform.position.x, element.transform.position.y, resetPositionZ);
    }
  }
  void MoveWalls(GameObject[] elements)
  {
    foreach (GameObject element in elements)
    {
      MoveWall(element);
    }
  }
  void MoveWall(GameObject element)
  {
    element.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

    // Check if the element has reached the threshold position and reset it
    if (element.transform.position.z <= wallThresholdZ)
    {
      element.transform.position = new Vector3(element.transform.position.x, element.transform.position.y, resetPositionZ);
    }
  }
}