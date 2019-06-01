using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
	[Header("-- Image --")]
	[SerializeField]
	private Image experience;

	private float maxExperience;
	private readonly float timeToAdd = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        maxExperience = Fight.Instance.experienceNeeded;
		experience.fillAmount = Fight.Instance.experience / maxExperience;
    }

    public IEnumerator AddExperience(float experienceEarned) {

		yield return new WaitForSeconds(0.5f);

		float elapsedTime = 0;
		float currentAmount = experience.fillAmount;

		while (elapsedTime < timeToAdd) {

			experience.fillAmount = Mathf.Lerp(currentAmount, currentAmount + (experienceEarned / maxExperience), (elapsedTime / timeToAdd));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

        experience.fillAmount = currentAmount + (experienceEarned / maxExperience);

		if(Fight.Instance.levelUp) {

			Fight.Instance.canvasAnim.SetBool("levelUp", true);
		}
		else {

			Fight.Instance.canvasAnim.SetTrigger("notLevelUp");
		}
	}
}
