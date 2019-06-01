using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DBManager : MonoBehaviour
{
	private const string PATH = "Prefab/DB Management/DBManager";
	public delegate void CallBackJson(string json);
	public delegate Sprite CallBackSprite(Texture2D texture);

	public bool requestDone = false;
	public bool dataUpdated = false;
	public List<Fighter> fighters;
	public List<Weapon> weapons;
	public User user;

	private static DBManager _instance;
	public static DBManager Instance {

		get {

			if (_instance == null) {

				GameObject obj = Instantiate(Resources.Load(PATH)) as GameObject;
				_instance = obj.GetComponent<DBManager>();
			}

			return _instance;
		}
	}

	void Awake() {
		
		DontDestroyOnLoad(gameObject);
	}

    IEnumerator SendData(string uri, string json, string token, CallBackJson callBack = null) {

		WWWForm form = new WWWForm();
		form.AddField("json", json);
		form.AddField("token", token);
		using(UnityWebRequest www = UnityWebRequest.Post(uri, form)) {

            www.SetRequestHeader("Access-Control-Allow-Origin", "*");
			yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            }
            else {
                string jsonAnswer = www.downloadHandler.text;
                dataUpdated = true;
                if(callBack != null) callBack(jsonAnswer);
            }

		}
	}

	IEnumerator GetRequest(string uri, CallBackJson callBack, string token) {
		WWWForm form = new WWWForm();
		form.AddField("token", token);
		using (UnityWebRequest www = UnityWebRequest.Post(uri, form)) {

            www.SetRequestHeader("Access-Control-Allow-Origin", "*");
            yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log(www.error);
			}
			else {
				string json = www.downloadHandler.text;
				callBack(json);
			}
		}
	}

	IEnumerator GetTexture(string uri, Image img, CallBackSprite callBack) {

		using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri)) {

            www.SetRequestHeader("Access-Control-Allow-Origin", "*");
            yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log(www.error);
			}
			else {
				Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				img.sprite = callBack(texture);
			}
		}
	}

    public void Connexion(UserConnexion user) {

        requestDone = false;
        string token = RandomString.Generate();
        string json = JsonUtility.ToJson(user);
        StartCoroutine(SendData("https://www.pixelsouls.be/jsonLogin", json, token, ConnectUser));
    }

	public void GetTexture(string url, Image img) {
		
		StartCoroutine(GetTexture(url, img, SendSprite));
	}

	private Sprite SendSprite(Texture2D texture) {

		return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
	}

	public void FindOneUserCharacters() {

		requestDone = false;
		string token = RandomString.Generate();
		StartCoroutine(GetRequest("https://www.pixelsouls.be/listCharacters/" + token + "/" + user.tempToken, ListCharacters, token));
        //StartCoroutine(GetRequest("http://127.0.0.1:8000/listCharacters/" + token, ListCharacters, token));
    }

	public void DeleteCharacter(string id) {

		requestDone = false;
		string token = RandomString.Generate();
		StartCoroutine(GetRequest("https://www.pixelsouls.be/removeCharacter/" + id + "/" + token + "/" + user.tempToken, ListCharacters, token));
        //StartCoroutine(GetRequest("http://127.0.0.1:8000/removeCharacter/" + id + "/" + token, ListCharacters, token));
    }

	public void FindAllOpponents() {

		requestDone = false;
		string token = RandomString.Generate();
		StartCoroutine(GetRequest("https://www.pixelsouls.be/listOpponents/" + token + "/" + user.tempToken, ListCharacters, token));
        //StartCoroutine(GetRequest("http://127.0.0.1:8000/listOpponents/" + token, ListCharacters, token));
    }

	public void FindRemainingWeapons(string id) {

		requestDone = false;
		string token = RandomString.Generate();
		StartCoroutine(GetRequest("https://www.pixelsouls.be/listWeapons/" + id + "/" + token, ListWeapons, token));
        //StartCoroutine(GetRequest("http://127.0.0.1:8000/listWeapons/" + id + "/" + token, ListWeapons, token));
    }

	public void SendFighterData(FightData data) {

		string json = JsonUtility.ToJson(data);
		string token = RandomString.Generate();
		StartCoroutine(SendData("https://www.pixelsouls.be/updateFighter/" + token + "/" + user.tempToken, json, token));
        //StartCoroutine(SendData("http://127.0.0.1:8000/updateFighter/" + token, json, token));
    }

	public void SendWeaponData(Fighter fighter, Weapon weapon) {

		string json = JsonUtility.ToJson(weapon);
		string token = RandomString.Generate();
		StartCoroutine(SendData("https://www.pixelsouls.be/addWeapon/" + fighter.id + "/" + token, json, token));
        //StartCoroutine(SendData("http://127.0.0.1:8000/addWeapon/" + fighter.id + "/" + token, json, token));
    }

	public void NewCharacter(NewCharacter character) {

		dataUpdated = false;
		string json = JsonUtility.ToJson(character);
		string token = RandomString.Generate();
		StartCoroutine(SendData("https://www.pixelsouls.be/newCharacter/" + token + "/" + user.tempToken, json, token));
        //StartCoroutine(SendData("http://127.0.0.1:8000/newCharacter/" + token, json, token));
    }

	private void ListCharacters(string json) {
		
		fighters = new List<Fighter>(JsonHelper.FromJson<Fighter>(json));
		requestDone = true;
	}

	private void ConnectUser(string json) {

        try {
            user = JsonHelper.FromJsonUnique<User>(json);
        }
        catch { user = null; }
		requestDone = true;
	}

	private void ListWeapons(string json) {

		weapons = new List<Weapon>(JsonHelper.FromJson<Weapon>(json));
		requestDone = true;
	}
}
