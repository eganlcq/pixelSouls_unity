using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentFight : FightersFight {
	
	protected override void Awake() {

		self = data.opponentChosen;
		base.Awake();
	}
}
