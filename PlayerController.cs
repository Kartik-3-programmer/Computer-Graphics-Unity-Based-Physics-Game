using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For UI pop-up
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 movement;

    public GameObject endGamePopup; // Reference to end game UI element
    public TextMeshProUGUI timerText; // Reference to TextMeshProUGUI for timer
    public TextMeshProUGUI scoreText; // Reference to UI text for score

    public AudioSource wallHitAudioSource; // Reference to the AudioSource for wall collision
    public AudioSource gravityChangeAudioSource; // Reference to the AudioSource for gravity change

    private float timer = 0f; // Timer to track time
    private bool isGameActive = true; // Track if the game is ongoing
    private int score = 0;

    public string difficulty = "Easy"; // Add difficulty setting

    private Vector3 currentGravity; // Store current gravity direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Set up UI elements
        endGamePopup.SetActive(false);
        if (timerText != null)
            timerText.text = "Time: 0.00s";
        if (scoreText != null)
            scoreText.text = "Score: 0";

        // Initialize current gravity
        currentGravity = Physics.gravity;
    }

    void Update()
    {
        if (isGameActive)
        {
            timer += Time.deltaTime;

            if (timerText != null)
                timerText.text = "Time: " + timer.ToString("F2") + "s";

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            movement = new Vector3(-moveX, 0f, -moveZ).normalized;

            // Detect gravity change with key press
            if (Input.GetKeyDown(KeyCode.G))
            {
                Physics.gravity = new Vector3(0, Physics.gravity.y * -1, 0); // Reverse gravity
                currentGravity = Physics.gravity;
                PlayGravityChangeAudio();
                ShowPopupMessage("Gravity changed!");
            }
        }
    }

    void FixedUpdate()
    {
        if (isGameActive)
        {
            Vector3 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit a Wall! Movement stopped.");
            movement = Vector3.zero;

            // Play the wall hit audio
            if (wallHitAudioSource != null)
            {
                wallHitAudioSource.Play();
                ShowPopupMessage("You hit a wall!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPoint"))
        {
            GameManager.Instance.StopTimerAndCalculateScore();
        }
    }

    void CalculateScore()
    {
        if (difficulty == "Easy")
        {
            score = Mathf.Max(10, 100 - (int)((timer - 15f) / 2f));
        }
        else if (difficulty == "Medium")
        {
            score = Mathf.Max(50, 200 - (int)((timer - 30f) / 2f) * 2);
        }
        else if (difficulty == "Hard")
        {
            score = Mathf.Max(100, 500 - (int)((timer - 75f) / 3f));
        }

        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void PlayGravityChangeAudio()
    {
        if (gravityChangeAudioSource != null)
        {
            gravityChangeAudioSource.Play();
            Debug.Log("Gravity change audio played!");
        }
        else
        {
            Debug.LogError("Gravity change audio source is not set!");
        }
    }

    void ShowPopupMessage(string message)
    {
        if (endGamePopup != null)
        {
            TextMeshProUGUI popupText = endGamePopup.GetComponentInChildren<TextMeshProUGUI>();
            if (popupText != null)
            {
                popupText.text = message;
            }

            StartCoroutine(DisplayPopup());
        }
    }

    IEnumerator DisplayPopup()
    {
        endGamePopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        endGamePopup.SetActive(false);
    }
}
