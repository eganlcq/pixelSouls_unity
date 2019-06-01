using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
	public FightData data;

	private void Awake() {
		
		Fighter character = data.characterChosen;
		Fighter opponent = data.opponentChosen;
		User characterOwner = character.owner;
		User opponentOwner = opponent.owner;

		character.id = "1";
		character.name = "Character";
		character.strength = "100";
		character.dexterity = "100";
		character.vitality = "100";
		character.level = "1";
		character.experience = "0";
		character.defenseWon = "0";
		character.defenseLost = "0";
		character.attackWon = "0";
		character.attackLost = "0";
		character.experienceNeeded = "100";

		opponent.id = "2";
		opponent.name = "Opponent";
		opponent.strength = "100";
		opponent.dexterity = "100";
		opponent.vitality = "100";
		opponent.level = "1";
		opponent.experience = "0";
		opponent.defenseWon = "0";
		opponent.defenseLost = "0";
		opponent.attackWon = "0";
		opponent.attackLost = "0";
		opponent.experienceNeeded = "100";

		characterOwner.id = "1";
		characterOwner.pseudo = "CharacterOwner";
		characterOwner.score = "0";
		characterOwner.image = "img";

		opponentOwner.id = "1";
		opponentOwner.pseudo = "CharacterOwner";
		opponentOwner.score = "0";
		opponentOwner.image = "img";
	}
}
