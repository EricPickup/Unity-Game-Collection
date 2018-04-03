using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour {

	public void PlayGame() {
		SceneManager.LoadScene("_Main_Menu_Scene");
	}

	public void QuitGame() {
		Application.Quit();
	}

}
