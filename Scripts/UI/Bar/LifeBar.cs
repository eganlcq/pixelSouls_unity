using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
	[Header("-- Character --")]
	public FightersFight character;

	[Header("-- Image --")]
	public Image life;
	public Image lifeChunk;

	private float maxLife;
	private bool isChanging = false;
	private float timeToReduce = 1;

	// Start is called before the first frame update
	void Start()
    {
		maxLife = character.life;
    }

    // Update is called once per frame
    void Update()
    {
        life.fillAmount = character.life / maxLife;

		if(life.fillAmount != lifeChunk.fillAmount && !isChanging) {

			isChanging = true;
			StartCoroutine(ReduceLifeChunk());
		}
    }

	private IEnumerator ReduceLifeChunk() {

		yield return new WaitForSeconds(1f);

		float elapsedTime = 0;
		float currentAmount = life.fillAmount;

		while(elapsedTime < timeToReduce) {

			if(currentAmount != life.fillAmount) {
				
				StartCoroutine(ReduceLifeChunk());
				yield break;
			}
			
			lifeChunk.fillAmount = Mathf.Lerp(lifeChunk.fillAmount, life.fillAmount, (elapsedTime / timeToReduce));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		isChanging = false;
	}
}
