using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs;
    public GameObject enemyBossPrefab;
    private GameObject playerInstance;
    private PlayerController playerScript;

    public List<GameObject> enemiesAlive;
    public GameObject[] spawnPoints;
    public int waveNumber = 0;
    public int totalWaves = 0;
    public int numberOfEnemies = 4;
    public bool gameOver = true;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timeTakenText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestTimeText;

    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject levelSelectionMenu;
    public GameObject gameMenu;
    public GameObject gameOverMenu;

    public AudioSource audioSourceMain;
    public Camera cameraMain;

    private int timeTaken = 0;
    private int score = 0;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = true;
        waveNumber = 0;
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        OnZeroEnemies();
        GamePauseToggle();
    }

    public void StartGame(Color color)
    {
        cameraMain.backgroundColor = color;
        gameOver = false;
        UnPauseGame();
        ShowGameMenu();
        InstantiatePlayer();
        StartCoroutine(GameTimer());
        DisplayScore();
        ShowBestTime();
    }

    void InstantiatePlayer()
    {
        playerInstance = Instantiate(playerPrefab, new Vector2(0f, -3.8f), playerPrefab.transform.rotation);
        playerScript = playerInstance.GetComponent<PlayerController>();
        playerScript.SetGameManager(this);
    }

    void InstantiateEnemy()
    {
        int tempIndex = Random.Range(0, spawnPoints.Length);
        GameObject tempEnemy = Instantiate(enemyPrefabs[waveNumber], spawnPoints[tempIndex].transform.position, enemyPrefabs[waveNumber].transform.rotation);
        Enemy tempScript = tempEnemy.GetComponent<Enemy>();
        tempScript.SetPlayerScript(playerScript);
        tempScript.SetGameManager(this);
        enemiesAlive.Add(tempEnemy);
    }

    void InstantiateEnemyBoss()
    {
        int tempIndex = Random.Range(0, spawnPoints.Length);
        GameObject tempEnemy = Instantiate(enemyBossPrefab, spawnPoints[5].transform.position, enemyBossPrefab.transform.rotation);
        Enemy tempScript = tempEnemy.GetComponent<Enemy>();
        tempScript.SetPlayerScript(playerScript);
        tempScript.SetGameManager(this);
        enemiesAlive.Add(tempEnemy);
    }

    void SpawnEnemyWave()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            InstantiateEnemy();
        }
    }

    void OnZeroEnemies()
    {
        if (gameOver) return;
        if (waveNumber == enemyPrefabs.Length)
        {
            InstantiateEnemyBoss();
            waveNumber = 0;
        }
        if(enemiesAlive.Count < 1)
        {
            SpawnEnemyWave();
            numberOfEnemies += 2;
        }
    }

    public void GameOver()
    {
        gameOver = true;
        ShowGameOverMenu();
    }

    public void RemoveEnemyFromAliveList(GameObject enemy)
    {
        GameObject tempEnemy = null;
        foreach (GameObject member in enemiesAlive)
        {
            if(enemy == member)
            {
                tempEnemy = member;
            }
        }
        if(tempEnemy != null)
        {
            enemiesAlive.Remove(tempEnemy);
        }
        AddScore();
        if(enemiesAlive.Count < 1)
        {
            RecordBestTime();
            waveNumber++;
            totalWaves++;
            ShowBestTime();
            timeTaken = 0;
        }
    }

    public void DisplayPlayerHealth()
    {
        livesText.text = "Lives: " + playerScript.GetHealth().ToString();
    }

    void DisplayTimeTaken()
    {
        timeTakenText.text = "Time: " + timeTaken.ToString();
    }

    IEnumerator GameTimer()
    {
        while(!gameOver)
        {
            DisplayTimeTaken();
            yield return new WaitForSeconds(1);
            timeTaken++;
        }
    }

    void DisplayScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void AddScore()
    {
        score += 5;
        DisplayScore();
    }

    public void AddBonusScore()
    {
        score += 30;
        DisplayScore();
    }

    void GamePauseToggle()
    {
        if (gameOver) return;
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(!isPaused)
            {
                PauseGame();
            } else
            {
                UnPauseGame();
            }
            
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void ShowMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        levelSelectionMenu.gameObject.SetActive(false);
        gameMenu.gameObject.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
        levelSelectionMenu.gameObject.SetActive(false);
        gameMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
    }

    public void ShowLevelSelectionMenu()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        levelSelectionMenu.gameObject.SetActive(true);
        gameMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
    }

    public void ShowGameMenu()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        levelSelectionMenu.gameObject.SetActive(false);
        gameMenu.gameObject.SetActive(true);
        gameOverMenu.gameObject.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        levelSelectionMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MuteVolume()
    {
        audioSourceMain.volume = 0f;
    }

    public void UnMuteVolume()
    {
        audioSourceMain.volume = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void ShowBestTime()
    {
        bestTimeText.text = "Wave: " + totalWaves + " Best Time: " + PlayerPrefs.GetFloat(("BestTime" + totalWaves), 100).ToString();
    }

    void RecordBestTime()
    {
        if (timeTaken < PlayerPrefs.GetFloat(("BestTime" + totalWaves), 100))
        {
            PlayerPrefs.SetFloat(("BestTime" + totalWaves), timeTaken);
        }
    }
}
