using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplePickerMenu : MonoBehaviour {

    public Canvas MenuCanvas;

    public AudioSource ButtonClickSound;

    public Canvas HistoryMenuCanvas;
    public Dropdown HistoryDropdown;

    public static List<GameLog> logs = new List<GameLog>();


    // Use this for initialization
    void Start () {

        string path = Application.streamingAssetsPath + "/gameLogs.json";
        string jsonString = File.ReadAllText(path);
        gameData data = JsonUtility.FromJson<gameData>(jsonString);
        logs.Clear();
        if (!(data == null))
        {
            foreach (GameLog log in data.ApplePicker)
            {
                logs.Add(log);
            }
        }

    }

    public static List<GameLog> GetLogs()
    {
        return logs;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void StartButtonClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        SceneManager.LoadScene(6);
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
            SceneManager.LoadScene(8);
        } else
        {
            HideAllCanvas();
            FileMenuManager.canvasHistory.Pop().gameObject.SetActive(true);
        }
        
    }

    public void HideAllCanvas()
    {
        MenuCanvas.gameObject.SetActive(false);
        HistoryMenuCanvas.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class gameData
{
    public List<GameLog> ApplePicker = new List<GameLog>();

    public void Add(GameLog log)
    {
        ApplePicker.Add(log);
    }
}

[System.Serializable]
public class GameLog
{
    public string Username;
    public string Date;
    public int Score;
    public string HighestLevel; //Space-shooter only

    public GameLog(string u, string d, int s, string h)
    {
        this.Username = u;
        this.Date = d;
        this.Score = s;
        this.HighestLevel = h;
    }
}