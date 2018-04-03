using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Sprite[] cardFace;
    public Sprite cardBack;
    public GameObject[] cards;
    public Text matchText;
    public Text scoreText;
    public Text gameOverText;
    public Text timeText;

    private bool _init = false;
    private int _pairs = 10;
    private int _score = 1000;
    private float fire_start_time;

    public AudioClip matchSound;
    public AudioClip winSound;
    public AudioClip mismatchSound;
    public AudioClip loseSound;

    private AudioSource matchSource { get { return GetComponent<AudioSource>(); } }
    private AudioSource winSource { get { return GetComponent<AudioSource>(); } }
    private AudioSource mismatchSource { get { return GetComponent<AudioSource>(); } }
    private AudioSource loseSource { get { return GetComponent<AudioSource>(); } }



    // Use this for initialization
    void Start () {
        gameObject.AddComponent<AudioSource>();
        matchSource.clip = matchSound;
        matchSource.playOnAwake = false;
        winSource.clip = winSound;
        winSource.playOnAwake = false;
        mismatchSource.clip = mismatchSound;
        mismatchSource.playOnAwake = false;
        loseSource.clip = loseSound;
        loseSource.playOnAwake = false;
        fire_start_time = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
		if (!_init)
        {
            InitializeCards();
        }
        if (Input.GetMouseButtonUp(0)) //When mouse is left-clicked
            CheckCards();
    }

    void InitializeCards()  //Shuffling/initializing card values
    {
        for (int id = 0; id < 2; id++)
        {
            for (int i = 1; i < 11; i++)
            {
                bool takenCheck = false;
                int choice = 0;
                while (!takenCheck)
                {
                    choice = Random.Range(0, cards.Length);
                    takenCheck = !(cards[choice].GetComponent<Card>().initialized);
                }
                cards[choice].GetComponent<Card>().cardValue = i;
                cards[choice].GetComponent<Card>().initialized = true;
            }
        }

        foreach(GameObject cardsToCheck in cards)
        {
            cardsToCheck.GetComponent<Card>().setupGraphics();
        }

        if (!_init)
        {
            _init = true;
        }
    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        return cardFace[i - 1];
    }

    void CheckCards()
    {
        List<int> cardsToCheck = new List<int>();
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null && cards[i].GetComponent<Card>().state == 1)   //If the card exists and is flipped:
            {
                cardsToCheck.Add(i);    //Add the card to the list of cards to be checked for matches
            }
        }
        if (cardsToCheck.Count == 2)    //If two cards are flipped, compare them
        {
            CardComparison(cardsToCheck);
        }
    }

    void CardComparison(List<int> cardsToCheck)
    {
        Card.FROZEN = true; //Tell cards to stop responding

        int state = 0;

        if (cards[cardsToCheck[0]].GetComponent<Card>().cardValue == cards[cardsToCheck[1]].GetComponent<Card>().cardValue) { //If the cards are a match

            state = 2;
            _pairs--;   //Subtract the number of pairs left
            matchText.text = "Number of Matches: " + _pairs;

            if (_pairs == 0)  //Win (no pairs left)
            {
                winSource.PlayOneShot(winSound);
                StartCoroutine(GameOverPause(4));
            } else //If not a win
            {
                matchSource.PlayOneShot(matchSound);
            }
            StartCoroutine(MatchPause(2, cards[cardsToCheck[0]], cards[cardsToCheck[1]]));  //Delay for 2 seconds, then delete the matched cards

        } else
        {
            //Mismatch
            _score -= 40;
            scoreText.text = "Score: " + _score;
            if (_score <= 0)    //LOSE
            {
                timeText.text = "Ellapsed Time: " + (Time.time - fire_start_time);
                FreezeAllCards();
                gameOverText.text = "GAME OVER";
                loseSource.PlayOneShot(loseSound);
                StartCoroutine(GameOverPause(4));
           
            } else //Mismatch but not lose
            {
                mismatchSource.PlayOneShot(mismatchSound);
            }
        }
        for (int i = 0; i < cardsToCheck.Count; i++)    //Update the states of the cards (if they're being checked or not)
        {
            cards[cardsToCheck[i]].GetComponent<Card>().state = state;  
            cards[cardsToCheck[i]].GetComponent<Card>().falseCheck();
        }

    }

    IEnumerator GameOverPause(int time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Menu");
    }

    IEnumerator MatchPause(int time, GameObject card1, GameObject card2)
    {
        yield return new WaitForSeconds(time);
        Destroy(card1);
        Destroy(card2);
    }

    void FreezeAllCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)   //If the card exists and is flipped:
            {
                Card.FROZEN = true;
            }
        }
    }
}
