using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include this for UI text manipulation

public class GameManager : MonoBehaviour
{
    // Public variables assigned in the Inspector
    public GameObject pauseMenuCanvas;
    public GameObject player;
    public GameObject startPoint; // Set the start point transform in the Inspector
    public GameObject endPoint; // Assign the endpoint object in the inspector
    public TextMeshProUGUI scoreText; // Reference to the UI Text component for displaying the score

    public static int difficultyLevel = 1; // 1: Easy, 2: Medium, 3: Hard
    public static GameManager Instance { get; private set; }

    private float timer;
    private bool timerRunning;
    public int score { get; private set; }

    private bool isPaused = false;

    // Variables to store maze dimensions
    private int _mazeWidth;
    private int _mazeLength;
    private int playerScore = 100; // Example score
    private string difficulty = "easy"; // Example difficulty
    private float timeTaken = 11; // Timer to track time

    private bool isGameRunning = true; 
    public void EndGame()
    {
        isGameRunning = false; // Stop the timer

        // Calculate the final score (replace with your scoring logic)
        playerScore = 100; 

        // Save game data
        FindObjectOfType<GameDataRecorder>().RecordGameData(playerScore, difficulty, timeTaken);

        Debug.Log("Game Over! Data recorded.");
    }

    private void Awake()
    {
        // Singleton pattern for GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetMazeDimensions(); // Set maze dimensions based on difficulty level
        PositionEndPoint(); // Position the endpoint based on the maze size
        StartTimer();
    }

    private void Update()
    {
        // Update timer if it's running
        if (timerRunning)
        {
            timer += Time.deltaTime;
        }

        // Check for pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void SetMazeDimensions()
    {
        switch (difficultyLevel)
        {
            case 1: // Easy
                _mazeWidth = 5;
                _mazeLength = 5;
                break;
            case 2: // Medium
                _mazeWidth = 10;
                _mazeLength = 10;
                break;
            case 3: // Hard
                _mazeWidth = 15;
                _mazeLength = 15;
                break;
            default:
                _mazeWidth = 5;
                _mazeLength = 5;
                break;
        }
    }

    private void PositionEndPoint()
    {
        // Calculate the position for the endpoint based on the start point and maze dimensions
        Vector3 startPosition = startPoint.transform.position; // Use the GameObject's position
        Vector3 endPointPosition = new Vector3(startPosition.x + _mazeWidth - 1, startPosition.y, startPosition.z + _mazeLength - 1);
        endPoint.transform.position = endPointPosition; // Set the endpoint's position
    }

    public void StartTimer()
    {
        timer = 0f;
        timerRunning = true;
    }

    public void StopTimerAndCalculateScore()
    {
        timerRunning = false;
        CalculateScore(); // Call to calculate the final score
        Debug.Log("Game Over! Final Score: " + score);
    }

    private void CalculateScore()
    {
        // Calculate the score based on the current difficulty level
        if (difficultyLevel == 1) // Easy
        {
            score = CalculateScoreBasedOnTime(100, 15f, 2, 2f, 10);
        }
        else if (difficultyLevel == 2) // Medium
        {
            score = CalculateScoreBasedOnTime(200, 30f, 4, 2f, 50);
        }
        else if (difficultyLevel == 3) // Hard
        {
            score = CalculateScoreBasedOnTime(500, 75f, 3, 3f, 100);
        }

        // Update the UI Text component to reflect the score
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private int CalculateScoreBasedOnTime(int startingScore, float thresholdTime, int decrement, float decrementInterval, int minScore)
    {
        if (timer <= thresholdTime)
        {
            return startingScore; // Max score if completed within threshold time
        }

        // Calculate the time elapsed beyond the threshold
        float extraTime = timer - thresholdTime;

        // Calculate the score reduction based on the extra time and decrement rate
        int scoreReduction = (int)(extraTime / decrementInterval) * decrement;

        // Final score should not be less than the minimum score
        return Mathf.Max(minScore, startingScore - scoreReduction);
    }

    public void SetDifficulty(int level)
    {
        difficultyLevel = level; // Store selected difficulty
        SceneManager.LoadScene("SampleScene"); // Load the maze scene
        StartCoroutine(AdjustCameraAfterLoad());
    }

    private IEnumerator AdjustCameraAfterLoad()
    {
        // Wait until the scene has loaded to adjust the camera
        yield return new WaitForEndOfFrame();
        Camera.main.GetComponent<CameraController>()?.AdjustCamera(); // Adjust camera if the component exists
    }

    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true); // Show pause menu
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false); // Hide pause menu
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time scale is normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
        ResetPlayerPosition(); // Reset player position
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = startPoint.transform.position; // Reset to start point
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        playerRigidbody.velocity = Vector3.zero; // Clear velocity
        playerRigidbody.angularVelocity = Vector3.zero; // Stop rotation
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exited Game");
    }
}

// Serializable class for saving player data
[System.Serializable]
public class PlayerData
{
    public Vector3 playerPosition;

    public PlayerData(Vector3 position)
    {
        playerPosition = position;
    }
}
