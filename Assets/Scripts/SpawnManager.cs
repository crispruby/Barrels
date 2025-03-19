using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
  public GameObject[] enemies;
  public GameObject[] powerups;
  private float zEnemySpawn = 14.0f;
  private float xSpawnRange = 5.0f;
  private float ySpawn = 0.65f;
  private float yPowerup = 1;

  private float powerupSpawnTime = 10.0f;
  private float enemySpawnTime = 0.75f;
  private float startDelay = 1.0f;
  private bool shouldSpawn = false;
  // Start is called before the first frame update
  void Start()
  {

  }
  // Update is called once per frame
  void Update()
  {

  }
  public void SetSpawnTimes(float powerupTime, float enemyTime)
  {
    powerupSpawnTime = powerupTime;
    enemySpawnTime = enemyTime;
  }
  public void StartSpawning()
  {
    shouldSpawn = true;
    InvokeRepeating("SpawnEnemy", startDelay, enemySpawnTime);
    InvokeRepeating("SpawnPowerup", startDelay, powerupSpawnTime);
  }
  public void StopSpawning()
  {
    shouldSpawn = false;
    CancelInvoke("SpawnEnemy");
    CancelInvoke("SpawnPowerup");
  }
  void SpawnEnemy()
  {
    if (shouldSpawn)
    {
      float randomX = Random.Range(-xSpawnRange, xSpawnRange);
      int randomIndex = Random.Range(0, enemies.Length);
      Vector3 spawnPos = new Vector3(randomX, ySpawn, zEnemySpawn);
      Instantiate(enemies[randomIndex], spawnPos, enemies[randomIndex].gameObject.transform.rotation);
    }
  }
  void SpawnPowerup()
  {
    if (shouldSpawn)
    {
      float randomX = Random.Range(-xSpawnRange, xSpawnRange);
      int randomIndex = Random.Range(0, powerups.Length);
      Vector3 spawnPos = new Vector3(randomX, yPowerup, zEnemySpawn);
      Instantiate(powerups[randomIndex], spawnPos, powerups[randomIndex].gameObject.transform.rotation);
    }
  }
  public void AdjustEnemySpawnTime(float adjustment)
  {
    enemySpawnTime = Mathf.Max(0.2f, enemySpawnTime + adjustment); // Ensure spawn time doesn't go below 0.1
    CancelInvoke("SpawnEnemy"); // Cancel the current spawn
    InvokeRepeating("SpawnEnemy", startDelay, enemySpawnTime); // Reinvoke with the new time
  }
}
