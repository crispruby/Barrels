using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] powerups;
    private float zEnemySpawn = GameConfig.SpawnZ;
    private float xSpawnRange = GameConfig.SpawnXRange;
    private float ySpawn = GameConfig.SpawnEnemyY;
    private float yPowerup = GameConfig.SpawnPowerupY;

    private float powerupSpawnTime = GameConfig.SpawnPowerupTime;
    private float enemySpawnTime = GameConfig.SpawnEnemyTime;
    private bool shouldSpawn = false;

    public void SetSpawnTimes(float powerupTime, float enemyTime)
    {
        powerupSpawnTime = powerupTime;
        enemySpawnTime = enemyTime;
    }
    public void StartSpawning()
    {
        if (shouldSpawn)
        {
            return;
        }

        shouldSpawn = true;
        InvokeRepeating(nameof(SpawnEnemy), GameConfig.SpawnStartDelay, enemySpawnTime);
        InvokeRepeating(nameof(SpawnPowerup), GameConfig.SpawnStartDelay, powerupSpawnTime);
    }
    public void StopSpawning()
    {
        shouldSpawn = false;
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(SpawnPowerup));
    }
    void SpawnEnemy()
    {
        if (!shouldSpawn)
        {
            return;
        }

        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        int randomIndex = Random.Range(0, enemies.Length);
        Vector3 spawnPos = new Vector3(randomX, ySpawn, zEnemySpawn);
        Instantiate(enemies[randomIndex], spawnPos, enemies[randomIndex].gameObject.transform.rotation);
    }
    void SpawnPowerup()
    {
        if (!shouldSpawn)
        {
            return;
        }

        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        int randomIndex = Random.Range(0, powerups.Length);
        Vector3 spawnPos = new Vector3(randomX, yPowerup, zEnemySpawn);
        Instantiate(powerups[randomIndex], spawnPos, powerups[randomIndex].gameObject.transform.rotation);
    }
    public void AdjustEnemySpawnTime(float adjustment)
    {
        enemySpawnTime = Mathf.Max(GameConfig.MinEnemySpawnTime, enemySpawnTime + adjustment);

        if (shouldSpawn)
        {
            CancelInvoke(nameof(SpawnEnemy));
            InvokeRepeating(nameof(SpawnEnemy), GameConfig.SpawnStartDelay, enemySpawnTime);
        }
    }
}
