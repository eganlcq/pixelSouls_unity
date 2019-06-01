using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Filter : MonoBehaviour
{
	private Image starImage;
	private Color defaultStarColor;
	private Button button;
	private SelectOpponent select;

    // Start is called before the first frame update
    void Awake()
    {
        starImage = GetComponentsInChildren<Image>()[1];
		defaultStarColor = starImage.color;
		button = GetComponent<Button>();
    }

	private void Start() {

		select = (SelectOpponent)CharacterSelector.Instance;
	}

	public void FavUserFilter() {

		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => OriginalList());
		starImage.color = Color.yellow;
		select.FilterList();
	}

	public void OriginalList() {

		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => FavUserFilter());
		starImage.color = defaultStarColor;
		select.OriginalList();
	}
}
