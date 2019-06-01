using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private string nameNextLevel;

	private static GameManager _instance;
	public static GameManager Instance {

		get {

			if (_instance == null) {

				GameObject obj = GameObject.FindGameObjectWithTag("Manager");
				_instance = obj.GetComponent<GameManager>();
			}

			return _instance;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		Scene currentScene = SceneManager.GetActiveScene();
		if(currentScene.name == "menu") {

			FadeScreen.Instance.FadeOut(null);
		}
	}

	public void ChangeLevel(string nameLevel) {

		FadeScreen.Instance.FadeIn(CallBackFadeIn);
		nameNextLevel = nameLevel;
	}

	public void CallBackFadeIn(float direction) {

		SceneManager.LoadScene(nameNextLevel);

	}
}
