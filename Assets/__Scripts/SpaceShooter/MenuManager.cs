using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static List<GameLog> logs = new List<GameLog>();
    public AudioSource ButtonClickSound;
    public Canvas MenuCanvas;
    public Canvas HistoryMenuCanvas;
    public Dropdown HistoryDropdown;

    void Start()
    {
        string path = Application.streamingAssetsPath + "/shooterLogs.json";
        string jsonString = File.ReadAllText(path);
        gameDataShooter data = JsonUtility.FromJson<gameDataShooter>(jsonString);
        logs.Clear();
        if (!(data == null))
        {
            foreach (GameLog log in data.ShooterGame)
            {
                logs.Add(log);
            }
        }
    }


    public void HistoryMenuButtonClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        FileMenuManager.canvasHistory.Push(MenuCanvas);
        HideAllCanvas();

        HistoryDropdown.ClearOptions();
        LoadHistoryDropdown();

        HistoryMenuCanvas.gameObject.SetActive(true);
        HistoryDropdown.Show();
    }

    public void HideAllCanvas()
    {
        MenuCanvas.gameObject.SetActive(false);
        HistoryMenuCanvas.gameObject.SetActive(false);
    }

    public void LoadHistoryDropdown()
    {
        HistoryDropdown.ClearOptions();
        string header = string.Format("{0,-20}{1,-30}{2,-20}", "Username", "Date", "Score");
        HistoryDropdown.options.Add(new Dropdown.OptionData() { text = header });

        foreach (GameLog log in logs)
        {
            string currentLog = string.Format("{0,-20}{1,-30}{2,-20}", log.Username, log.Date, log.Score);
            HistoryDropdown.options.Add(new Dropdown.OptionData() { text = currentLog });
        }
    }

    public void BackToFileButtonClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        if (MenuCanvas.gameObject.activeInHierarchy)
        {
            HideAllCanvas();
            SceneManager.LoadScene(7);
        }
        else
        {
            HideAllCanvas();
            FileMenuManager.canvasHistory.Pop().gameObject.SetActive(true);
        }

    }

    public static void SaveGameData()
    {
        gameDataShooter newData = new gameDataShooter();
        string path = Application.streamingAssetsPath + "/shooterLogs.json";
        foreach (GameLog log in logs)
        {
            newData.Add(log);
        }
        Debug.Log(JsonUtility.ToJson(newData));
        File.WriteAllText(path, JsonUtility.ToJson(newData));
    }

}

[System.Serializable]
public class gameDataShooter
{
    public List<GameLog> ShooterGame = new List<GameLog>();

    public void Add(GameLog log)
    {
        ShooterGame.Add(log);
    }
}
