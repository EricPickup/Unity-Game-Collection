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
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        SceneManager.LoadScene("_Main_Menu_Scene");
    }

    public void MemoryGameClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        SceneManager.LoadScene("Menu");
    }

    public void ApplePickerClick()
    {
        ButtonClickSound.PlayOneShot(ButtonClickSound.clip, 1.0f);
        SceneManager.LoadScene(6);
    }

    public void RPSClick()
    {
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
}
