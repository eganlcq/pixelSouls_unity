using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitText : MonoBehaviour {

	private Text _currentText;
	private string _textToWrite;

	// Use this for initialization
	void Start () {
		
		_currentText = GetComponent<Text>();
		_textToWrite = _currentText.text;
		StartCoroutine(DisplayDashes(_textToWrite));
	}

	private IEnumerator DisplayDashes(string quote) {

		while(true) {

			_currentText.text = quote;
			yield return new WaitForSeconds(1f);
			_currentText.text = quote + ".";
			yield return new WaitForSeconds(1f);
			_currentText.text = quote + "..";
			yield return new WaitForSeconds(1f);
			_currentText.text = quote + "...";
			yield return new WaitForSeconds(1f);
		}
	}
}
