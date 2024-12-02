using System.Collections;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public AudioClip endpointAudio; // Drag your audio clip here in the inspector
    private AudioSource audioSource;
    

    private void Start()
    {
        // Ensure there's an AudioSource component on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Optional: Configure the AudioSource
        audioSource.playOnAwake = false; // Don't play the audio on start
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Stop the timer, calculate the score, and freeze player controls
            GameManager.Instance.StopTimerAndCalculateScore();
              FindObjectOfType<GameManager>().EndGame();

            // Disable player controls by deactivating its movement script
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            // Play the audio clip
            if (endpointAudio != null)
            {
                audioSource.clip = endpointAudio;
                audioSource.Play();
            }

            // Print a confirmation message
            Debug.Log("Player reached the endpoint!");
        }
    }

}
