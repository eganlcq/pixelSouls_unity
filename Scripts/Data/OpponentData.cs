using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Opponent", order = 3)]
public class OpponentData : ScriptableObject
{
	public List<Fighter> fullList;
	public List<Fighter> opponents;
}
