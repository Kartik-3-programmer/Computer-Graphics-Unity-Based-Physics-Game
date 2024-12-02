using System.IO;
using UnityEngine;

public class GameDataRecorder : MonoBehaviour
{
    private string filePath = "C:/GameData.csv"; // Change this to your desired path

    void Start()
    {
        // Ensure the file exists and has a header
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0) // File exists but is empty
            {
                AddHeader();
            }
        }
        else
        {
            Debug.LogError($"File not found! Please create GameData.csv at: {filePath}");
        }
    }

    private void AddHeader()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Score,Difficulty,Time"); // Add header
        }
        Debug.Log("Header added to existing file: " + filePath);
    }

    // Method to record game data (call this after each game session)
    public void RecordGameData(int score, string difficulty, float time)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found! Please ensure GameData.csv exists at: " + filePath);
            return;
        }

        using (StreamWriter writer = new StreamWriter(filePath, true)) // Append mode
        {
            string dataLine = $"{score},{difficulty},{time}";
            writer.WriteLine(dataLine); // Write data
        }
        Debug.Log($"Game data recorded: Score={score}, Difficulty={difficulty}, Time={time}");
    }
}
