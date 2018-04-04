using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Users : MonoBehaviour {

    static Dictionary<string, User> users = new Dictionary<string, User>();    //Key: Name (String), Value: User Object
    static User currentUser = null;

	// Use this for initialization
	void Start () {
        string path = Application.streamingAssetsPath + "/userData.json";
        string jsonString = File.ReadAllText(path);
        userData data = JsonUtility.FromJson<userData>(jsonString);
        foreach (UserObject user in data.Users)
        {
            users.Add(user.Username, new User(user.Username, user.Password, user.Score, user.Status));
            
        }
        DumpUsers();
    }

    public static Boolean ContainsUser(string name)
    {
        return users.ContainsKey(name);
    }

    public static User GetUser(string name)
    {
        return users[name];
        
    }

    

    public static void AddUser(string name)
    {
        users.Add(name, new User(name));
        DumpUsers();
    }

    public static void DeleteUser(string name)
    {
        users.Remove(name);
        DumpUsers();
    }

    public static void UnblockUser(string name)
    {
        users[name].status = "normal";
        DumpUsers();
    }

    public static void BlockUser(string name)
    {
        users[name].status = "blocked";
        DumpUsers();
    }


    public static void ChangePassword(string newPassword)
    {
        if (CurrentUser != null)
        {
            users[CurrentUser.username].password = newPassword;
        }
        DumpUsers();
    }

    public static void DumpUsers()
    {
        userData newUsers = new userData();
        string path = Application.streamingAssetsPath + "/userData.json";
        foreach (User user in users.Values)
        {
            UserObject currentUser;
            currentUser.Username = user.username;
            Debug.Log("Dumping user: " + user.username);
            currentUser.Password = user.password;
            currentUser.Score = user.score;
            currentUser.Status = user.status;
            newUsers.Add(currentUser);
        }
        Debug.Log(JsonUtility.ToJson(newUsers));
        File.WriteAllText(path, JsonUtility.ToJson(newUsers));

    }

    public static Dictionary<string,User> GetUsers()
    {
        return users;
    }

    


    public static User CurrentUser
    {
        get { return currentUser; }
        set { currentUser = value; }
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
    public List<UserObject> Users = new List<UserObject>();

    public void Add(UserObject user)
    {
        Users.Add(user);
    }
}

[System.Serializable]
public class User
{
    string Username;
    string Password;
    int Score;
    string Status;

    public User(string name)
    {
        this.Username = name;
        this.Password = "password";
        this.Score = 0;
        this.Status = "new";
    }

    public User(string name, string password, int score, string status)
    {
        this.Username = name;
        this.Password = password;
        this.Score = score;
        this.Status = status;
    }

    public string username
    {
        get { return this.Username; }
        set { this.Username = value; }
    }

    public string password
    {
        get { return this.Password; }
        set { this.Password = value; }
    }

    public string status
    {
        get { return this.Status; }
        set { this.Status = value; }
    }

    public int score
    {
        get { return this.Score; }
        set { this.Score = value; }
    }
}

public class GameLog
{
    DateTime date;
    int score;
    string sessionLength;
    int highestLevel;   //Spaceshooter game only

}