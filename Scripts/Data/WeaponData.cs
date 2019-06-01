using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
	private Dictionary<string, WeaponId> idWeaponFromString = new Dictionary<string, WeaponId>() {

		{ "Dagger", WeaponId.Dagger },
		{ "Sword", WeaponId.Sword },
		{ "Axe", WeaponId.Axe },
		{ "Hammer", WeaponId.Hammer },
		{ "Greatsword", WeaponId.TwoHandedSword },
		{ "Greataxe", WeaponId.TwoHandedAxe },
		{ "Greathammer", WeaponId.TwoHandedHammer },
		{ "Curved sword", WeaponId.CurvedSword },
		{ "Estoc", WeaponId.Estoc },
		{ "Halberd", WeaponId.Halberd },
		{ "Katana", WeaponId.Katana },
		{ "Scythe", WeaponId.Scythe },
		{ "Mace", WeaponId.Cudgel },
		{ "Spear", WeaponId.Spear },
		{ "Bo", WeaponId.Bo },
		{ "Twin blades", WeaponId.Guandao },
		{ "Sai", WeaponId.Sai },
		{ "Tonfa", WeaponId.Tonfa },
		{ "Fork", WeaponId.Fork },
		{ "Frying pan", WeaponId.Pan },
	};
	public Dictionary<string, WeaponId> IdWeaponFromString {

		get { return idWeaponFromString; }
	}
}
