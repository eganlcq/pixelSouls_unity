using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomString
{
    public static string Generate(int length = 20) {

		string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int charactersLength = characters.Length;
		string randomString = "";
		for(int i = 0; i < length; i++) {

			randomString += characters[Random.Range(0, charactersLength - 1)];
		}
		return randomString;
	}
}
