﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileMenuManager : MonoBehaviour {

    public Canvas FileCanvas;

    public Canvas PackageCanvas;

    public Canvas AccountCanvas;
    public Button CreateUserMenuButton;
    public Button DeleteUserMenuButton;
    public Button UnblockUserMenuButton;

    public Canvas PasswordCanvas;
    public Button ChangePasswordButton;
    public InputField NewPasswordField;
    public Text ChangePasswordStatus;

    public Canvas NewUserCanvas;
    public Button CreateUserButton;
    public InputField NewUserField;
    public Text NewUserStatus;

    public Canvas DeleteUserCanvas;
    public Button DeleteUserButton;
    public Dropdown UserList;
    public Text DeleteUserStatus;

    public Canvas UnblockUserCanvas;
    public Button UnblockUserButton;
    public Dropdown BlockedList;
    public Text UnblockUserStatus;

    public Canvas ConfigurationCanvas;
    public Sprite[] Sprites;
    public Image Background;
    public Dropdown BackgroundDropdown;
    public Dropdown MusicDropdown;
    public AudioClip[] BackgroundMusic;
    public AudioSource CurrentSong;
    public Image PreviewImage;
    public Button ChangeBackgroundButton;

    public AudioSource ButtonClickSound;

    public Canvas HistoryCanvas;
    public Dropdown HistoryDropdown;
    

    public static Stack<Canvas> canvasHistory = new Stack<Canvas>();  //Keeps track of canvas history so we don't need back button functions for each canvas

    // Use this for initialization
    void Start () {

        //HideAllCanvas();
        CurrentSong.Play();
        //Exiting a game will trigger Start() function again, skip login if this occurs
       

    }
    
    // Update is called once per frame
    void Update () {
        PreviewImage.sprite = Sprites[BackgroundDropdown.value];
        if (!(Sprites[BackgroundDropdown.value].Equals(PreviewImage.sprite)))
        {
            PreviewImage.sprite = Sprites[BackgroundDropdown.value];
        }
        if (!(BackgroundMusic[MusicDropdown.value].Equals(CurrentSong.clip)))
        {
            CurrentSong.clip = BackgroundMusic[MusicDropdown.value];
            CurrentSong.Play();
        }
    }

    public void AccountMenuClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        canvasHistory.Push(FileCanvas);
        HideAllCanvas();
        AccountCanvas.gameObject.SetActive(true);
        if (!(Users.CurrentUser == null) && !(Users.CurrentUser.username == "admin"))
        {
            CreateUserButton.gameObject.SetActive(false);
            DeleteUserMenuButton.gameObject.SetActive(false);
            UnblockUserMenuButton.gameObject.SetActive(false);
        }

    }

    public void ChangePasswordMenuClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        canvasHistory.Push(AccountCanvas);
        HideAllCanvas();
        PasswordCanvas.gameObject.SetActive(true);
    }

    public void CreateUserMenuClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        canvasHistory.Push(AccountCanvas);
        HideAllCanvas();
        NewUserCanvas.gameObject.SetActive(true);
    }

    public void DeleteUserMenuClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        canvasHistory.Push(AccountCanvas);
        HideAllCanvas();
        DeleteUserCanvas.gameObject.SetActive(true);
        UserList.options.Clear();
        foreach (User user in Users.GetUsers().Values) {
            UserList.options.Add(new Dropdown.OptionData() { text = user.username });
        }
        
    }

    public void UnblockUserMenuClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        canvasHistory.Push(AccountCanvas);
        HideAllCanvas();
        UnblockUserCanvas.gameObject.SetActive(true);
        BlockedList.options.Clear();
        foreach (User user in Users.GetUsers().Values)
        {
            if (user.status == "blocked")
            {
                Debug.Log("Added user to blocked list");
                BlockedList.options.Add(new Dropdown.OptionData() { text = user.username });
            }
        }
    }

    public void HistoryMenuClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        canvasHistory.Push(FileCanvas);
        HideAllCanvas();
        HistoryDropdown.ClearOptions();
        if (Users.CurrentUser == null || Users.CurrentUser.username == "admin")
        {
            LoadHistoryDropdownAdmin();
        } else
        {
            LoadHistoryDropdown();
        }
        HistoryCanvas.gameObject.SetActive(true);
        HistoryDropdown.Show();

    }

    public void LoadHistoryDropdown()
    {
        string header = string.Format("{0,-20}{1,-30}{2,-20}", "Username", "Date", "Length");
        HistoryDropdown.options.Add(new Dropdown.OptionData() { text = header });

        foreach (SessionLogObject session in Users.CurrentUser.Logins)
        {
            string currentLog = string.Format("{0,-20}{1,-30}{2,-20}", Users.CurrentUser.username, session.Time, session.Length);
            HistoryDropdown.options.Add(new Dropdown.OptionData() { text = currentLog });
        }

    }

    public void LoadHistoryDropdownAdmin()
    {
        string header = string.Format("{0,-20}{1,-30}{2,-20}{3,-20}", "Username", "Date", "Length", "Status");
        HistoryDropdown.options.Add(new Dropdown.OptionData() { text = header });
        foreach (User user in Users.GetUsers().Values)
        {
            foreach (SessionLogObject session in user.Logins)
            {
                string currentLog = string.Format("{0,-20}{1,-30}{2,-20}{3,-20}", user.username, session.Time, session.Length, user.status);
                HistoryDropdown.options.Add(new Dropdown.OptionData() { text = currentLog });
            }
        }
    }

    public void ChangePassword()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        if (NewPasswordField.text != "")
        {
            Users.ChangePassword(NewPasswordField.text);
            ChangePasswordStatus.color = Color.green;
            ChangePasswordStatus.text = "Successfully changed password";
        } else
        {
            ChangePasswordStatus.color = Color.red;
            ChangePasswordStatus.text = "Password must contain at least one character";
        }
    }

    public void CreateUser()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        if (Users.ContainsUser(NewUserField.text))
        {
            NewUserStatus.color = Color.red;
            NewUserStatus.text = "User already exists!";
        } else if (NewUserField.text == "")
        {
            NewUserStatus.color = Color.red;
            NewUserStatus.text = "Username must consist of at least one character!";
        } else
        {
            Users.AddUser(NewUserField.text);
            NewUserStatus.color = Color.green;
            NewUserStatus.text = "Created user with password \"password\".";
        }
    }

    public void DeleteUser()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        string selectedUser = UserList.options[UserList.value].text;
        if (selectedUser == "admin")
        {
            DeleteUserStatus.color = Color.red;
            DeleteUserStatus.text = "Cannot delete admin user";
        } else
        {
            Users.DeleteUser(selectedUser);
            DeleteUserStatus.color = Color.green;
            DeleteUserStatus.text = "Deleted user.";
            UserList.options.Remove(UserList.options[UserList.value]);
            UserList.value = 0;
        }
    }

    public void ChangeBackground()
    {
        Background.sprite = Sprites[BackgroundDropdown.value];
    }

    public void ConfigurationMenuClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        canvasHistory.Push(FileCanvas);
        HideAllCanvas();
        ConfigurationCanvas.gameObject.SetActive(true);
    }

    public void UnblockUser()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        Users.UnblockUser(BlockedList.options[BlockedList.value].text);
        UnblockUserStatus.color = Color.green;
        UnblockUserStatus.text = "Unblocked user.";
        BlockedList.options.Remove(BlockedList.options[BlockedList.value]);
        BlockedList.value = 0;
    }

    public void BackToFileButtonClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        HideAllCanvas();
        canvasHistory.Pop().gameObject.SetActive(true);
    }

    public void HideAllCanvas()
    {
        FileCanvas.gameObject.SetActive(false);
        AccountCanvas.gameObject.SetActive(false);
        PasswordCanvas.gameObject.SetActive(false);
        DeleteUserCanvas.gameObject.SetActive(false);
        NewUserCanvas.gameObject.SetActive(false);
        UnblockUserCanvas.gameObject.SetActive(false);
        PackageCanvas.gameObject.SetActive(false);
        ConfigurationCanvas.gameObject.SetActive(false);
        HistoryCanvas.gameObject.SetActive(false);
        
    }
}
