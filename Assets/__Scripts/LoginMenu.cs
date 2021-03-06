﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour {

    public Text usernameField;
    public Text passwordField;
    public Text warningText;

    public Canvas LoginCanvas;
    public Canvas PackageCanvas;
    public Canvas ChangePasswordCanvas;
    public Text ChangePasswordText;

    public AudioSource ButtonClickSound;

    Dictionary<string, int> loginViolations = new Dictionary<string, int>();

    // Use this for initialization
    void Start () {

        //LoginCanvas.gameObject.SetActive(true);
        if (Users.CurrentUser != null)
        {
            LoginCanvas.gameObject.SetActive(false);
            PackageCanvas.gameObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void ValidateLogin()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        if (Users.ContainsUser(usernameField.text))
        {
            User attemptUser = Users.GetUser(usernameField.text);
            if (attemptUser.status == "blocked")
            {
                warningText.text = "User is blocked from signing in!";
            } else if (attemptUser.password == passwordField.text)  //Success
            {
                Users.CurrentUser = Users.GetUser(usernameField.text);
                Users.currentUserStartTime = Time.time;
                LoginCanvas.gameObject.SetActive(false);
                if (Users.CurrentUser.status == "new")
                {
                    ChangePasswordCanvas.gameObject.SetActive(true);
                    ChangePasswordText.color = Color.cyan;
                    ChangePasswordText.text = "As a new user, you must update your password!";
                    Users.CurrentUser.status = "normal";
                    FileMenuManager.canvasHistory.Push(PackageCanvas);
                } else
                {
                    PackageCanvas.gameObject.SetActive(true);
                }

            } else
            {
                if (usernameField.text != "admin")  //Don't block admin
                {
                    int remainingAttempts = addViolation(usernameField.text);
                    if (remainingAttempts == 0)
                    {
                        warningText.text = "Too many invalid attempts, account is now blocked!";
                        Users.BlockUser(attemptUser.username);

                    }
                    else
                    {
                        warningText.text = "Invalid password: Account will be blocked after " + remainingAttempts + " more invalid attempts";
                    }
                } else
                {
                    warningText.text = "Invalid password";
                }
            }
        } else
        {
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
