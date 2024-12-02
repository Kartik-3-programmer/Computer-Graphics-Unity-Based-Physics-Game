using System.Diagnostics; // For Process and ProcessStartInfo
using System.IO; // For File operations
using UnityEngine; // For Unity logging

public class PythonRunner : MonoBehaviour
{
    [SerializeField] private string pythonFilePath = "/Users/pavan/Downloads/modeltraining.py"; // Path to the Python file
    [SerializeField] private string outputTextFilePath = "/Users/pavan/Downloads/accuracy.txt"; // Path to the output text file

    public void RunPythonAndSaveResult()
    {
        // Ensure the Python file path and output file path are set
        if (string.IsNullOrEmpty(pythonFilePath) || string.IsNullOrEmpty(outputTextFilePath))
        {
            UnityEngine.Debug.LogError("Python file path or output file path is not set.");
            return;
        }

        try
        {
            // Create a new process to run the Python script
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "python", // Use "python" command; ensure Python is in PATH
                Arguments = pythonFilePath, // Add the Python script path as an argument
                RedirectStandardOutput = true, // Capture the output of the script
                UseShellExecute = false, // Don't use the system shell
                CreateNoWindow = true // Run without a console window
            };

            using (Process process = Process.Start(processStartInfo))
            {
                if (process == null)
                {
                    UnityEngine.Debug.LogError("Failed to start the Python process.");
                    return;
                }

                // Read the output of the Python script
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Save the output to the specified text file
                File.WriteAllText(outputTextFilePath, output);

                UnityEngine.Debug.Log($"Python script executed successfully. Result saved to: {outputTextFilePath}");
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"An error occurred while running the Python script: {ex.Message}");
        }
    }
}
