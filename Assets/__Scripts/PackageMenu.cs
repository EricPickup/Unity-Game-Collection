using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PackageMenu : MonoBehaviour {

    public Canvas FileCanvas;
    public Canvas PackageCanvas;
    public AudioSource ButtonClickSound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpaceButtonClick()
    {
        FileMenuManager.canvasHistory.Push(PackageCanvas);
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        SceneManager.LoadScene("_Main_Menu_Scene");
    }

    public void MemoryGameClick()
    {
        FileMenuManager.canvasHistory.Push(PackageCanvas);
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        SceneManager.LoadScene("Menu");
    }

    public void ApplePickerClick()
    {
        FileMenuManager.canvasHistory.Push(PackageCanvas);
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        SceneManager.LoadScene(9);
    }

    public void RPSClick()
    {
        FileMenuManager.canvasHistory.Push(PackageCanvas);
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        SceneManager.LoadScene(7);
    }

    public void FileClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        FileMenuManager.canvasHistory.Push(PackageCanvas);
        PackageCanvas.gameObject.SetActive(false);
        FileCanvas.gameObject.SetActive(true);
    }

    public void ExitClick()
    {
        Users.CurrentUser.Logins.Add(new SessionLogObject());
        Users.DumpUsers();
        Debug.Log("Added logout");
    }
}
