using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public static bool FROZEN = false;  //Tells game when no cards should be flipped

    [SerializeField]
    private int _state;
    [SerializeField]
    private int _cardValue;
    [SerializeField]
    private bool _initialized = false;

    private Sprite _cardBack;
    private Sprite _cardFace;

    private GameObject _manager;

    public AudioClip sound;

    private AudioSource source
    {
        get
        {
            return GetComponent<AudioSource>();
        }
    }

    void Start()
    {
        _state = 1;
        _manager = GameObject.FindGameObjectWithTag("Manager");
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
    }

    public void setupGraphics()
    {
        _cardBack = _manager.GetComponent<GameManager>().getCardBack();
        _cardFace = _manager.GetComponent<GameManager>().getCardFace(_cardValue);
        flipCard();
    }

    public void flipCard()
    {
        source.PlayOneShot(sound);
        //Change to appropriate states
        if (_state == 0)
        {
            _state = 1;
        } else if (_state == 1)
        {
            _state = 0;
        }
        //check current state and make sure the card isn't frozen
        if (_state == 0 && !FROZEN)
        {
            GetComponent<Image>().sprite = _cardBack;
        } else if (_state == 1 && !FROZEN)
        {
            GetComponent<Image>().sprite = _cardFace;
        }
    }

    public int cardValue
    {
        get
        {
            return _cardValue;
        }
        set
        {
            _cardValue = value;
        }
    }

    public int state
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
        }
    }

    public bool initialized
    {
        get
        {
            return _initialized;
        }
        set
        {
            _initialized = value;
        }
    }

    public void falseCheck()
    {
        StartCoroutine(pause());
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(1);
        if (_state == 0)
        {
            GetComponent<Image>().sprite = _cardBack;
        } else if (_state == 1)
        {
            GetComponent<Image>().sprite = _cardFace;
        }
        FROZEN = false;
    }
    
}
