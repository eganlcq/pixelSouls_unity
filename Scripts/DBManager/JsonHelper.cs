using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper {
	public static T[] FromJson<T>(string json) {

		string newJson = "{\"Items\":" + json + "}";
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
		return wrapper.Items;
	}

	public static T FromJsonUnique<T>(string json) {

		return JsonUtility.FromJson<T>(json);
	}

	public static string ToJson<T>(T[] array) {
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.Items = array;
		return JsonUtility.ToJson(wrapper);
	}

	[Serializable]
	private class Wrapper<T> {
		public T[] Items;
	}
}
