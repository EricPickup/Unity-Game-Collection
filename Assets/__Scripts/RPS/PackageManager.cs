using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageManager : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		
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
            Application.Quit();
        }
  
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


}
