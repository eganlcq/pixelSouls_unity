using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOpponent : CharacterSelector
{
	private List<Fighter> listFighter = new List<Fighter>();
	private List<Fighter> currentListFighter = new List<Fighter>();

	private List<Fighter> listFavFighter = new List<Fighter>();
	private List<GameObject> listFavFrame = new List<GameObject>();
	private List<GameObject> listfavLayout = new List<GameObject>();

	private int maxOpponentDisplayed = 10;

	[SerializeField]
	private OpponentData opponentData;

	protected override IEnumerator Display() {

		manager.FindAllOpponents();
		yield return new WaitWhile(() => !manager.requestDone);

		GetFighters();
		FillList(currentListFighter, listFrame, listLayouts);
		FillLayouts(listFrame, listLayouts);

		if(listFavFighter.Count > 0) {

			FillList(listFavFighter, listFavFrame, listfavLayout);
			FillLayouts(listFavFrame, listfavLayout);

			foreach (GameObject layout in listfavLayout) {

				layout.SetActive(false);
			}
		}

		FadeScreen.Instance.FadeOut(null);
	}

	protected override void GetFighters() {

		base.GetFighters();
		opponentData.fullList = fullListOfFighter;

		if(fullListOfFighter.Count < maxOpponentDisplayed) {

			maxOpponentDisplayed = fullListOfFighter.Count;
		}

		int playerCharacterLevel = int.Parse(data.characterChosen.level);

		listFighter = ChooseOpponents(0, playerCharacterLevel);
		opponentData.opponents = listFighter;
		
		for(int i = 0; i < maxOpponentDisplayed; i++) {

			currentListFighter.Add(listFighter[i]);
		}

		foreach (Fighter fighter in fullListOfFighter) {

			foreach (FavoriteUser user in data.characterChosen.owner.favoriteUsers) {

				if (user.id == fighter.owner.id) listFavFighter.Add(fighter);
			}
		}

		listFavFighter.Sort((p1, p2) => int.Parse(p1.level).CompareTo(int.Parse(p2.level)));
	}

	private List<Fighter> ChooseOpponents(int treshold, int level) {

		List<Fighter> fighters = new List<Fighter>();

		do {
			fighters = fullListOfFighter.FindAll((fighter) => int.Parse(fighter.level) >= (level - treshold) && int.Parse(fighter.level) <= (level + treshold));
			treshold++;
		} while(fighters.Count < maxOpponentDisplayed);

		ShuffleList.Shuffle(fighters);

		return fighters;
	}

	protected override void FillList(List<Fighter> fighters, List<GameObject> frames, List<GameObject> layouts) {
		
		foreach(Fighter fighter in fighters) {

			frames.Add(Instantiate(Resources.Load(FRAME_OPPONENT)) as GameObject);
			ModifyDatas(fighter, frames[frames.Count - 1]);
		}

		base.FillList(fighters.Count, layouts);
	}

	protected override void ModifyDatas(Fighter fighter, GameObject frame) {

		Text[] listText = frame.GetComponentsInChildren<Text>();
		Image avatar = frame.GetComponentsInChildren<Image>()[5];
		User owner = fighter.owner;
		Button choose = frame.GetComponentsInChildren<Button>()[0];

		DBManager.Instance.GetTexture("https://www.pixelsouls.be/img/avatars/" + owner.image, avatar);
        //DBManager.Instance.GetTexture("http://127.0.0.1:8000/img/avatars/" + owner.image, avatar);

		listText[0].text = fighter.name;
		listText[1].text = fighter.level;
		listText[2].text = fighter.strength;
		listText[3].text = fighter.dexterity;
		listText[4].text = fighter.vitality;

		choose.onClick.RemoveAllListeners();
        choose.onClick.AddListener(() => _guiSound.Play(_validateSound));
		choose.onClick.AddListener(() => OpponentSelected(fighter));
		choose.onClick.AddListener(() => GameManager.Instance.ChangeLevel("fight"));
	}

	public void FilterList() {

		foreach(GameObject layout in listLayouts) {

			layout.SetActive(false);
		}

		foreach(GameObject layout in listfavLayout) {

			layout.SetActive(true);
		}
	}

	public void OriginalList() {

		foreach (GameObject layout in listfavLayout) {

			layout.SetActive(false);
		}

		foreach (GameObject layout in listLayouts) {

			layout.SetActive(true);
		}
	}

	public void RefreshList() {

		currentListFighter.Clear();
		ShuffleList.Shuffle(listFighter);

		for(int i = 0; i < maxOpponentDisplayed; i++) {

			currentListFighter.Add(listFighter[i]);
			ModifyDatas(listFighter[i], listFrame[i]);
		}
	}
}
