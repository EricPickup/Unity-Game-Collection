using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PackageManager : MonoBehaviour {

    public Canvas MenuCanvas;
    public Canvas GameCanvas;

    public AudioSource ButtonClickSound;

    public Canvas HistoryMenuCanvas;
    public Dropdown HistoryDropdown;

    enum elements { Scissor = 1, Paper, Rock }

    private int playerChoice = -1;
    private int botChoice = -1;

    private bool playersTurn = true;

    public GameObject WinnerText;
    public GameObject FinalWinner;
    public GameObject PlayerScore;
    public GameObject BotScore;
    public GameObject TurnCount;
    public Button rockButton;
    public Button paperButton;
    public Button scissorButton;
    public Sprite paperImage, rockImage, scissorImage;
    public GameObject botChooseImage;

    private int numBotWins = 0;
    private int numPlayerWins = 0;
    private int numTurns = 0;

    public static List<GameLog> logs = new List<GameLog>();

    public Button MenuButton;

    // Use this for initialization
    void Start () {
        string path = Application.streamingAssetsPath + "/rpsLogs.json";
        string jsonString = File.ReadAllText(path);
        gameDataRPS data = JsonUtility.FromJson<gameDataRPS>(jsonString);
        logs.Clear();
        if (!(data == null))
        {
            foreach (GameLog log in data.RockPaperScissors)
            {
                logs.Add(log);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(playersTurn && playerChoice == -1)
        {
            return;
        } 
        BotChoose();
        TurnWinner();
        playerChoice = -1;
        playersTurn = true;
	}

    public void StartButtonClick()
    {
        FileMenuManager.canvasHistory.Push(MenuCanvas);
        MenuButton.gameObject.SetActive(false);
        MenuCanvas.gameObject.SetActive(false);
        GameCanvas.gameObject.SetActive(true);
        ResetGame();
        rockButton.gameObject.SetActive(true);
        paperButton.gameObject.SetActive(true);
        scissorButton.gameObject.SetActive(true);
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
            SceneManager.LoadScene(7);
        }
        else
        {
            HideAllCanvas();
            FileMenuManager.canvasHistory.Pop().gameObject.SetActive(true);
        }

    }

    public void HideAllCanvas()
    {
        MenuCanvas.gameObject.SetActive(false);
        HistoryMenuCanvas.gameObject.SetActive(false);
        GameCanvas.gameObject.SetActive(false);
    }

    //Determines the winner of the current term, sets the appropriate text/color
    void TurnWinner()
    {
        if (playerChoice == botChoice)
        {
            WinnerText.GetComponent<Text>().text = "DRAW";
            WinnerText.GetComponent<Text>().color = Color.black;
        }
        else if (playerChoice == (int)elements.Rock && botChoice == (int)elements.Paper)
        {
            numBotWins++;
            WinnerText.GetComponent<Text>().text = "Bot Wins!";
            WinnerText.GetComponent<Text>().color = Color.red;
        }
        else if (playerChoice == (int)elements.Rock && botChoice == (int)elements.Scissor)
        {
            numPlayerWins++;
            WinnerText.GetComponent<Text>().text = "Player Wins!";
            WinnerText.GetComponent<Text>().color = Color.green;
        }
        else if (playerChoice == (int)elements.Paper && botChoice == (int)elements.Rock)
        {
            numPlayerWins++;
            WinnerText.GetComponent<Text>().text = "Player Wins!";
            WinnerText.GetComponent<Text>().color = Color.green;
        }
        else if (playerChoice == (int)elements.Paper && botChoice == (int)elements.Scissor)
        {
            numBotWins++;
            WinnerText.GetComponent<Text>().text = "Bot Wins!";
            WinnerText.GetComponent<Text>().color = Color.red;
        }
        else if (playerChoice == (int)elements.Scissor && botChoice == (int)elements.Rock)
        {
            numBotWins++;
            WinnerText.GetComponent<Text>().text = "Bot Wins!";
            WinnerText.GetComponent<Text>().color = Color.red;
        }
        else if (playerChoice == (int)elements.Scissor && botChoice == (int)elements.Paper)
        {
            numPlayerWins++;
            WinnerText.GetComponent<Text>().text = "Player Wins!";
            WinnerText.GetComponent<Text>().color = Color.green;
        }
        numTurns++;
        UpdateScores();
        if (numTurns >= 10)
        {
            string winner;
            if (numBotWins > numPlayerWins)
            {
                winner = "Bot";
            }
            else if (numBotWins < numPlayerWins)
            {
                winner = "Player";
            }
            else
            {
                winner = "Tie";
            }
            FinalWinner.GetComponent<Text>().text = "FINAL WINNER: " + winner;
            if (Users.CurrentUser == null) {
                logs.Add(new GameLog("admin", System.DateTime.Now.ToString(), winner, "n/a"));
            } else
            {
                logs.Add(new GameLog(Users.CurrentUser.username, System.DateTime.Now.ToString(), winner, "n/a"));
            }
            rockButton.gameObject.SetActive(false);
            paperButton.gameObject.SetActive(false);
            scissorButton.gameObject.SetActive(false);
            SaveGameData();
            MenuButton.gameObject.SetActive(true);
        }
  
    }

    public void ResetGame()
    {
        botChooseImage.GetComponent<Image>().sprite = null;
        WinnerText.GetComponent<Text>().text = "";
        FinalWinner.GetComponent<Text>().text = "";
        numBotWins = 0;
        numPlayerWins = 0;
        numTurns = 0;
    }

    //Retrieves choice from player interacting with UI
    public void PlayerChoose(int choose)
    {
        playerChoice = choose;
        playersTurn = false;    //Now bots turn
    }

    //Bot chooses an integer between 1 and 3 inclusive, determining rock/paper/scissors and setting the appropriate image
    public void BotChoose()
    {
        botChoice = Random.Range(1, 4); 
        if (botChoice == 1) //paper
        {   
            botChooseImage.GetComponent<Image>().sprite = paperImage;
        } else if (botChoice == 2)  //rock
        {
            botChooseImage.GetComponent<Image>().sprite = rockImage;
        } else if (botChoice == 3)
        {
            botChooseImage.GetComponent<Image>().sprite = scissorImage;
        }
    }

    //Update the text on the UI containing the scores
    public void UpdateScores()
    {
        PlayerScore.GetComponent<Text>().text = "Player: " + numPlayerWins;
        BotScore.GetComponent<Text>().text = "Bot: " + numBotWins;
        TurnCount.GetComponent<Text>().text = "Turn: " + numTurns;
    }

    public void SaveGameData()
    {
        gameDataRPS newData = new gameDataRPS();
        string path = Application.streamingAssetsPath + "/rpsLogs.json";
        foreach (GameLog log in logs)
        {
            newData.Add(log);
        }
        Debug.Log(JsonUtility.ToJson(newData));
        File.WriteAllText(path, JsonUtility.ToJson(newData));
    }


}

[System.Serializable]
public class gameDataRPS
{
    public List<GameLog> RockPaperScissors = new List<GameLog>();

    public void Add(GameLog log)
    {
        RockPaperScissors.Add(log);
    }
}
