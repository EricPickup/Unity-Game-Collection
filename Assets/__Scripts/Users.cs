using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Users : MonoBehaviour {

    static Hashtable users = new Hashtable();    //Key: Name (String), Value: User Object

	// Use this for initialization
	void Start () {
        string path = Application.streamingAssetsPath + "/userData.json";
        string jsonString = File.ReadAllText(path);
        userData data = JsonUtility.FromJson<userData>(jsonString);
        foreach (UserObject user in data.Users)
        {
            users.Add(user.Username, new User(user.Username, user.Password, user.Score, user.Status));
        }    
    }

    public static Boolean ContainsUser(string name)
    {
        return users.ContainsKey(name);
    }

    public static User GetUser(string name)
    {
        return (User)users[name];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadUsers()
    {

    }
    
    void Awake()
    {

    }
}

[System.Serializable]
public struct UserObject
{
    public string Username;
    public string Password;
    public int Score;
    public string Status;
}

[System.Serializable]
public class userData
{
    public List<UserObject> Users;
}

public class User
{
    string username;
    string password;
    int score;
    string status;
    public User(string name)
    {
        this.username = name;
        this.password = "password";
        this.score = 0;
        this.status = "new";
    }

    public User(string name, string password, int score, string status)
    {
        this.username = name;
        this.password = password;
        this.score = score;
        this.status = status;
    }

    public string Username
    {
        get { return this.username; }
    }

    public string Password
    {
        get { return this.password; }
    }

    public string Status
    {
        get { return this.status; }
        set { this.status = value; }
    }
}

public class GameLog
{
    DateTime date;
    int score;
    string sessionLength;
    int highestLevel;   //Spaceshooter game only

}