using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PackageMenu : MonoBehaviour {

    public Canvas FileCanvas;
    public Canvas PackageCanvas;

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

    public void ApplePickerClick()
    {
        SceneManager.LoadScene(6);
    }

    public void RPSClick()
    {
        SceneManager.LoadScene(7);
    }

    public void FileClick()
    {
        FileMenuManager.canvasHistory.Push(PackageCanvas);
        PackageCanvas.gameObject.SetActive(false);
        FileCanvas.gameObject.SetActive(true);
    }
}
