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
        if (Users.ContainsUser(usernameField.text))
        {
            Debug.Log("Found user");
            User attemptUser = Users.GetUser(usernameField.text);
            if (attemptUser.Password == passwordField.text)
            {
                Debug.Log("Valid login");
            }
        }
    }
}
