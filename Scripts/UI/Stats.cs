using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
	[Header("-- Text --")]
	[SerializeField]
	private Text strengthValue;
	[SerializeField]
	private Text dexterityValue;
	[SerializeField]
	private Text vitalityValue;
	[Header("-- Animator --")]
	[SerializeField]
	private Animator trigger;

	private int currentStrength;
	private int currentDexterity;
	private int currentVitality;
	private int strengthLevelUp;
	private int dexterityLevelUp;
	private int vitalityLevelUp;

	private int nbCoroutinePassed = 0;

	private void Start() {
		
		currentStrength = Fight.Instance.strength;
		currentDexterity = Fight.Instance.dexterity;
		currentVitality = Fight.Instance.vitality;

		strengthValue.text = currentStrength.ToString();
		dexterityValue.text = currentDexterity.ToString();
		vitalityValue.text = currentVitality.ToString();
	}

	public IEnumerator PrepareData() {

		strengthLevelUp = Fight.Instance.strength;
		dexterityLevelUp = Fight.Instance.dexterity;
		vitalityLevelUp = Fight.Instance.vitality;

		yield return new WaitForSeconds(1f);

		yield return IncrementValue(currentStrength, strengthLevelUp, strengthValue);
		yield return new WaitForSeconds(0.5f);
		yield return IncrementValue(currentDexterity, dexterityLevelUp, dexterityValue);
		yield return new WaitForSeconds(0.5f);
		yield return IncrementValue(currentVitality, vitalityLevelUp, vitalityValue);
	}

	private IEnumerator IncrementValue(int currentValue, int valueLevelUp, Text text) {

		currentValue++;
		text.text = currentValue.ToString();

		yield return new WaitForSeconds(0.05f);

		if(currentValue < valueLevelUp) {

			StartCoroutine(IncrementValue(currentValue, valueLevelUp, text));
		}
		else {
			nbCoroutinePassed++;
			if(nbCoroutinePassed == 3) {

				trigger.SetTrigger("statsDisplayed");
			}
		}
	}
}
