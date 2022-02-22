using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Only Object Pooling and Vfx bonus are not done. Every other requirement and bonuses are done

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

    //This method starts the game
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

    //This method instantiates the player at the default position
    void InstantiatePlayer()
    {
        playerInstance = Instantiate(playerPrefab, new Vector2(0f, -3.8f), playerPrefab.transform.rotation);
        playerScript = playerInstance.GetComponent<PlayerController>();
        playerScript.SetGameManager(this);
    }

    // Requirement Complete: Each Wave will bring on New Enemies in a different pattern
    // 5 different enemies will come in each wave
    //This method instantiates enemy
    void InstantiateEnemy()
    {
        int tempIndex = Random.Range(0, spawnPoints.Length);
        GameObject tempEnemy = Instantiate(enemyPrefabs[waveNumber], spawnPoints[tempIndex].transform.position, enemyPrefabs[waveNumber].transform.rotation);
        Enemy tempScript = tempEnemy.GetComponent<Enemy>();
        tempScript.SetPlayerScript(playerScript);
        tempScript.SetGameManager(this);
        enemiesAlive.Add(tempEnemy);
    }

    // Bonus Complete: Hard Chickens(takes multiple hits before going down) / Boss Fight
    //This method instantiates enemy boss
    void InstantiateEnemyBoss()
    {
        int tempIndex = Random.Range(0, spawnPoints.Length);
        GameObject tempEnemy = Instantiate(enemyBossPrefab, spawnPoints[5].transform.position, enemyBossPrefab.transform.rotation);
        Enemy tempScript = tempEnemy.GetComponent<Enemy>();
        tempScript.SetPlayerScript(playerScript);
        tempScript.SetGameManager(this);
        enemiesAlive.Add(tempEnemy);
    }

    //This method spawns enemy waves
    void SpawnEnemyWave()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            InstantiateEnemy();
        }
    }

    // Requirement Complete: Next Wave will start exactly from where the previous wave ends.
    //This method checks if all enemies have died
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

    //This method sets game over
    public void GameOver()
    {
        gameOver = true;
        ShowGameOverMenu();
    }

    //This method removes an enemy from the alive list
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

    // Requirement Complete: Lives and Time taken should be Shown via UI.
    //This method displays player health
    public void DisplayPlayerHealth()
    {
        livesText.text = "Lives: " + playerScript.GetHealth().ToString();
    }

    // Requirement Complete: Lives and Time taken should be Shown via UI.
    //This method displays time taken on each wave
    void DisplayTimeTaken()
    {
        timeTakenText.text = "Time: " + timeTaken.ToString();
    }

    //This method shows the timer for each wave
    IEnumerator GameTimer()
    {
        while(!gameOver)
        {
            DisplayTimeTaken();
            yield return new WaitForSeconds(1);
            timeTaken++;
        }
    }

    //This method displays score
    void DisplayScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    //This method adds score
    void AddScore()
    {
        score += 5;
        DisplayScore();
    }

    // Bonus Complete: Collectibles and Obstacles falling down
    //This method adds score on collectable
    public void AddBonusScore()
    {
        score += 30;
        DisplayScore();
    }

    // Requirement Complete: Pause Menu
    //This method toggles pause game
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

    // Requirement Complete: Pause Menu
    //This method pauses the game
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Requirement Complete: Pause Menu
    //This method unpauses the game
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    // Requirement Complete: Main Menu
    //This method shows the main menu and disables everything else
    public void ShowMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        levelSelectionMenu.gameObject.SetActive(false);
        gameMenu.gameObject.SetActive(false);
    }

    // Requiement Complete: Settings(Sound on/off)
    //This method shows the settings menu and disables everything else
    public void ShowSettingsMenu()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
        levelSelectionMenu.gameObject.SetActive(false);
        gameMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
    }

    // Requirement Complete: Level Selection(Different Backgrounds)
    //This method shows the background selection menu and disables everything else
    public void ShowLevelSelectionMenu()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        levelSelectionMenu.gameObject.SetActive(true);
        gameMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
    }

    //This method shows the game menu and disables everything else
    public void ShowGameMenu()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        levelSelectionMenu.gameObject.SetActive(false);
        gameMenu.gameObject.SetActive(true);
        gameOverMenu.gameObject.SetActive(false);
    }

    //This method shows the game over menu and disables everything else
    public void ShowGameOverMenu()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        levelSelectionMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(true);
    }

    //This method reloads the scene
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Requiement Complete: Settings(Sound on/off)
    // Bonus Complete: SFX(Sounds)
    //This method mutes the volume by setting it to 0
    public void MuteVolume()
    {
        audioSourceMain.volume = 0f;
    }

    // Requiement Complete: Settings(Sound on/off)
    //This method unmutes the volume by setting it to 1
    public void UnMuteVolume()
    {
        audioSourceMain.volume = 1f;
    }

    //This method exits from the game
    public void ExitGame()
    {
        Application.Quit();
    }

    // Bonus Complete: Best Time for each Wave
    //This method shows the best time for the wave
    void ShowBestTime()
    {
        bestTimeText.text = "Wave: " + totalWaves + " Best Time: " + PlayerPrefs.GetFloat(("BestTime" + totalWaves), 100).ToString();
    }

    // Bonus Complete: Best Time for each Wave
    //This method records and saves the best time for the wave
    void RecordBestTime()
    {
        if (timeTaken < PlayerPrefs.GetFloat(("BestTime" + totalWaves), 100))
        {
            PlayerPrefs.SetFloat(("BestTime" + totalWaves), timeTaken);
        }
    }
}
