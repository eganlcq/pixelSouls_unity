using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFightTrigger : MonoBehaviour
{
	[SerializeField]
	private ExperienceBar expBar;
	[SerializeField]
	private Stats stats;

    public void TriggerExpBar() {

		StartCoroutine(expBar.AddExperience(Fight.Instance.nbExpEarned));
	}

	public void TriggerStatValues() {

		StartCoroutine(stats.PrepareData());
	}
}
