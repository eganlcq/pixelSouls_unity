using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
	public string id;
	public string pseudo;
	public string score;
	public string image;
    public string tempToken;
	public FavoriteUser[] favoriteUsers;
	public Fan[] fans;
}

[Serializable]
public class FavoriteUser {

	public string id;
}

[Serializable]
public class Fan {

	public string id;
}
