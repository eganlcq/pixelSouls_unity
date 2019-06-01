using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Fight", order = 1)]
public class FightData : ScriptableObject
{
    public Fighter characterChosen;
	public Fighter opponentChosen;
    public bool isWon;
}
