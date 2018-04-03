using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PackageMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpaceButtonClick()
    {
        SceneManager.LoadScene("_Main_Menu_Scene");
    }

    public void MemoryGameClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
