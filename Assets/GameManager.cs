using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
  public TextMeshProUGUI titleText;
  public TextMeshProUGUI healthText;
  public TextMeshProUGUI pointsText;
  public TextMeshProUGUI livesText;
  public TextMeshProUGUI gameOverText;
  public Button restartButton;
  public Button easyButton;
  public Button mediumButton;
  public Button hardButton;
  public bool isGameActive;
  public SpawnManager spawnManager;
  // Start is called before the first frame update
  void Start()
  {
    isGameActive = false;
    gameOverText.gameObject.SetActive(false);
    restartButton.gameObject.SetActive(false);
    healthText.gameObject.SetActive(false);
    livesText.gameObject.SetActive(false);
    pointsText.gameObject.SetActive(false);
    titleText.gameObject.SetActive(true);
    easyButton.gameObject.SetActive(true);
    mediumButton.gameObject.SetActive(true);
    hardButton.gameObject.SetActive(true);
    spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    easyButton.onClick.AddListener(EasyGame);
    mediumButton.onClick.AddListener(MediumGame);
    hardButton.onClick.AddListener(HardGame);
    restartButton.onClick.AddListener(RestartGame);
  }
  void EasyGame()
  {
    StartGame(10.0f, 0.8f); // Longer powerup spawn, slower enemy spawn
  }

  void MediumGame()
  {
    StartGame(15.0f, 0.7f); // Moderate powerup and enemy spawn times
  }

  void HardGame()
  {
    StartGame(20.0f, 0.6f); // Shorter powerup spawn, faster enemy spawn
  }

  // Update is called once per frame
  void Update()
  {
        
  }
  void StartGame(float powerupSpawnTime, float enemySpawnTime)
  {
    isGameActive = true;
    titleText.gameObject.SetActive(false);
    easyButton.gameObject.SetActive(false);
    mediumButton.gameObject.SetActive(false);
    hardButton.gameObject.SetActive(false);
    healthText.gameObject.SetActive(true);
    livesText.gameObject.SetActive(true);
    pointsText.gameObject.SetActive(true);
    spawnManager.SetSpawnTimes(powerupSpawnTime, enemySpawnTime);
    spawnManager.StartSpawning();
  }
  public void GameOver()
  {
    isGameActive =false;
    spawnManager.StopSpawning();
    ClearObjects();
    gameOverText.gameObject.SetActive(true);
    restartButton.gameObject.SetActive(true);
  }
  void RestartGame()
  {
    // Reload the scene or reset variables
    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
  }
  public void ClearObjects()
  {
    GameObject[] toxicbarrels = GameObject.FindGameObjectsWithTag("ToxicBarrel");
    foreach (GameObject toxicbarrel in toxicbarrels)
    {
      Destroy(toxicbarrel);
    }
    GameObject[] gasbarrels = GameObject.FindGameObjectsWithTag("GasBarrel");
    foreach (GameObject gasbarrel in gasbarrels)
    {
      Destroy(gasbarrel);
    }
    GameObject[] stripedbarrels = GameObject.FindGameObjectsWithTag("StripedBarrel");
    foreach (GameObject stripedbarrel in stripedbarrels)
    {
      Destroy(stripedbarrel);
    }
    GameObject[] rustybarrels = GameObject.FindGameObjectsWithTag("RustyBarrel");
    foreach (GameObject rustybarrel in rustybarrels)
    {
      Destroy(rustybarrel);
    }
    GameObject[] redPowers = GameObject.FindGameObjectsWithTag("Powerup");
    foreach (GameObject redPower in redPowers)
    {
      Destroy(redPower);
    }
    GameObject[] greenPowers = GameObject.FindGameObjectsWithTag("Power1");
    foreach (GameObject greenPower in greenPowers)
    {
      Destroy(greenPower);
    }
  }
}