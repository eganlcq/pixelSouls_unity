using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Fighter
{
	public string id;
    public string name;
	public string strength;
	public string dexterity;
	public string vitality;
	public string level;
	public string experience;
	public string defenseWon;
	public string defenseLost;
	public string attackWon;
	public string attackLost;
	public string experienceNeeded;
	public User owner;
	public Weapon[] weapons;
}
