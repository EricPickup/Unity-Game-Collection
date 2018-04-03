using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour {

    public Text usernameField;
    public Text passwordField;
    public Text warningText;

    Dictionary<string, int> loginViolations = new Dictionary<string, int>();

    // Use this for initialization
    void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ValidateLogin()
    {
        if (Users.ContainsUser(usernameField.text))
        {
            User attemptUser = Users.GetUser(usernameField.text);
            if (attemptUser.Status == "blocked")
            {
                warningText.color = Color.red;
                warningText.text = "User is blocked from signing in!";
            } else if (attemptUser.Password == passwordField.text)
            {
                warningText.color = Color.green;
                warningText.text = "Successfully logged in";
            } else
            {
                int remainingAttempts = addViolation(usernameField.text);
                if (remainingAttempts == 0)
                {
                    warningText.color = Color.red;
                    warningText.text = "Too many invalid attempts, account is now blocked!";
                    attemptUser.Status = "blocked";
                    
                } else
                {
                    warningText.color = Color.red;
                    warningText.text = "Invalid password: Account will be blocked after " + remainingAttempts + " more invalid attempts";
                }
            }
        } else
        {
            warningText.color = Color.red;
            warningText.text = "User does not exist";
        }
    }

    public int addViolation(string username)
    {
        if (loginViolations.ContainsKey(username))
        {
            loginViolations[username]--;
        } else
        {
            loginViolations[username] = 2;
        }
        return loginViolations[username];
    }
}
