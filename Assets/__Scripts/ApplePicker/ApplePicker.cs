using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;

    // Use this for initialization
    void Start () {

        basketList = new List<GameObject>();
		for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void AppleDestroyed()
    {
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tGO in tAppleArray)
        {
            Destroy(tGO);
        }

        int basketIndex = basketList.Count - 1;
        GameObject tBasketGO = basketList[basketIndex];
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGO);
        if (basketList.Count == 0)  //Game over
        {
            if (Users.CurrentUser == null)
            {
                ApplePickerMenu.logs.Add(new GameLog("admin", System.DateTime.Now.ToString(), Basket.staticScore, "n/a"));
            } else {
                ApplePickerMenu.logs.Add(new GameLog(Users.CurrentUser.username, System.DateTime.Now.ToString(), Basket.staticScore, "n/a"));
            }
            SaveGameData();
            Basket.staticScore = 0;
            SceneManager.LoadScene(9);
        }
    }

    public void SaveGameData()
    {
        gameData newData = new gameData();
        string path = Application.streamingAssetsPath + "/gameLogs.json";
        foreach (GameLog log in ApplePickerMenu.logs)
        {
            newData.Add(log);
        }
        Debug.Log(JsonUtility.ToJson(newData));
        File.WriteAllText(path, JsonUtility.ToJson(newData));
    }

    
}