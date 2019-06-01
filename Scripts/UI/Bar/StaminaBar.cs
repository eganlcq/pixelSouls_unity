using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
	[Header("-- Character --")]
	public FightersFight character;

	[Header("-- Image --")]
	public Image stamina;

    // Update is called once per frame
    void Update()
    {
        stamina.fillAmount = character.stamina;
    }
}
