using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene"); // Replace with your game scene name
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/saveData.json"))
        {
            SceneManager.LoadScene("SampleScene"); // Replace with your game scene name
        }
        else
        {
            Debug.Log("No save data found.");
        }
    }
}
