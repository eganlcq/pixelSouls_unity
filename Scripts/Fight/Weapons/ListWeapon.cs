using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListWeapon : MonoBehaviour
{
	public static ListWeapon Instance;
    [SerializeField]
	private WeaponData data;

	[SerializeField]
	private List<Sprite> sprites = new List<Sprite>();

	private void Awake() {
		Instance = this;
	}

	public Sprite GetSprite(WeaponId weapon) {

		return sprites[(int)weapon];
	}

	public float GetFloat(WeaponId weapon) {

		float delta = 1f / (Enum.GetValues(typeof(WeaponId)).Length - 1);
		float neededFloat = (delta * (int)weapon) + delta;
		return neededFloat;
	}

	public WeaponId GetWeapon(string name) {

		return data.IdWeaponFromString[name];
	}
}
