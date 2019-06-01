using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFight : FightersFight
{
	protected override void Awake()
    {
        self = data.characterChosen;
		base.Awake();
	}
}
