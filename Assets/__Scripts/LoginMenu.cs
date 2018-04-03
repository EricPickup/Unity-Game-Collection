using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour {

    public Text usernameField;
    public Text passwordField;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ValidateLogin()
    {
        //validatelogin
        if (usernameField.text == "Hello")
        {
            Debug.Log("Valid");
        } else
        {
            Debug.Log("Invalid");
        }
    }
}

public class User
{
    string username;
    string password;
    string status;

    public User(string name)
    {
        
    }
}

public class GameLog
{
    DateTime date;
    int score;
    string sessionLength;
    int highestLevel;   //Spaceshooter game only
}
