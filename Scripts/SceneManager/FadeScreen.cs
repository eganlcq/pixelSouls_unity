using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
	private const string PATH_PREFAB = "Prefab/Scene Management/ScreenFade";
	private static FadeScreen _instance;
	public static FadeScreen Instance {

		get {

			if(_instance == null) {

				GameObject obj = Instantiate(Resources.Load(PATH_PREFAB)) as GameObject;
				_instance = obj.GetComponent<FadeScreen>();
			}

			return _instance;
		}
	}

	private float timeToFade = 0.3f;
	public delegate void CallBackMethod(float direction);
	private CanvasGroup _canvasGroup;

    // Start is called before the first frame update
    void Awake() {
        
		_canvasGroup = GetComponent<CanvasGroup>();
		DontDestroyOnLoad(gameObject);
    }

    public void FadeIn(CallBackMethod callBack) {

		StartCoroutine(CoroutineFade(timeToFade, 1, callBack));
	}

	public void FadeOut(CallBackMethod callBack) {

		StartCoroutine(CoroutineFade(timeToFade, -1, callBack));
	}

	IEnumerator CoroutineFade(float time, float directionFade, CallBackMethod callBack) {

		float timeEnd = Time.time + time;
		while (Time.time < timeEnd) {
			_canvasGroup.alpha += (Time.deltaTime / time) * directionFade;
			yield return null;
		}
		if (directionFade == -1) _canvasGroup.alpha = 0;
		else _canvasGroup.alpha = 1;

		if (callBack != null) callBack(directionFade);
	}
}
