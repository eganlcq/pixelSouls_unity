using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacter : CharacterSelector {
	
	[Header("-- Deletion Frame --")]
	public GameObject frameObject;
	[Header("-- New character canvas --")]
	public GameObject obj;
	private Animator canvasAnim;
	private Canvas canvas;
	
	private readonly int maxFrame = 6;
	private Popup popup;
	private DeletionFrame deletionFrame;
	private List<Button> listButton = new List<Button>();

	private Coroutine checkCoroutine = null;
    TouchScreenKeyboard keyboard;

	protected override void Awake() {

		base.Awake();
		canvasAnim = obj.GetComponent<Animator>();
		canvas = obj.GetComponent<Canvas>();
	}

	// Start is called before the first frame update
	protected override void Start() {
		popup = frameObject.GetComponent<Popup>();
		deletionFrame = frameObject.GetComponent<DeletionFrame>();

		base.Start();
	}

    protected override IEnumerator Display() {

		manager.FindOneUserCharacters();
		yield return new WaitWhile(() => !manager.requestDone);

		GetFighters();
		FillList(fullListOfFighter, listFrame, listLayouts);
		FillLayouts(listFrame, listLayouts);
		FadeScreen.Instance.FadeOut(null);
	}

	protected override void FillList(List<Fighter> fighters, List<GameObject> frames, List<GameObject> layouts) {

		foreach (Fighter fighter in fighters) {

			frames.Add(Instantiate(Resources.Load(FRAME_CHARACTER)) as GameObject);
			ModifyDatas(fighter, frames[frames.Count - 1]);
			FillListButton(frames[frames.Count - 1]);
		}

		int remainingFrames = maxFrame - fullListOfFighter.Count;

		for (int i = 0; i < remainingFrames; i++) {

			frames.Add(Instantiate(Resources.Load(FRAME_EMPTY)) as GameObject);
			ModifyDatas(frames[frames.Count - 1]);
			FillListButton(frames[frames.Count - 1]);
		}

		base.FillList(maxFrame, layouts);
	}

	protected override void ModifyDatas(Fighter fighter, GameObject frame) {

		Text[] listText = frame.GetComponentsInChildren<Text>();
		Button choose = frame.GetComponentsInChildren<Button>()[0];
		Button delete = frame.GetComponentsInChildren<Button>()[1];

		listText[0].text = fighter.name;
		listText[1].text = fighter.level;
		listText[2].text = fighter.strength;
		listText[3].text = fighter.dexterity;
		listText[4].text = fighter.vitality;

		choose.onClick.AddListener(() => CharacterSelected(fighter));
		choose.onClick.AddListener(() => GameManager.Instance.ChangeLevel("selectOpponent"));
        choose.onClick.AddListener(() => _guiSound.Play(_validateSound));
		delete.onClick.AddListener(() => deletionFrame.EnableClick(fighter.id, listButton));
		delete.onClick.AddListener(popup.Pop);
        delete.onClick.AddListener(() => _guiSound.Play(_validateSound));
	}

	private void ModifyDatas(GameObject frame) {

		Button choose = frame.GetComponentInChildren<Button>();
		choose.onClick.AddListener(() => NewCharacter());
	}

	private void NewCharacter() {

		canvas.sortingOrder = 1;
		canvasAnim.enabled = true;
	}

	public void CreateCharacter(string text) {

		if(checkCoroutine == null)
			checkCoroutine = StartCoroutine(SendNewCharacterData(text));
	}

	private IEnumerator SendNewCharacterData(string text) {

		NewCharacter character = new NewCharacter();
		character.name = text;

		DBManager.Instance.NewCharacter(character);

		yield return new WaitUntil(() => DBManager.Instance.dataUpdated);

		GameManager.Instance.ChangeLevel("chooseCharacter");
		checkCoroutine = null;
	}

	private void FillListButton(GameObject frame) {

		Button[] buttons = frame.GetComponentsInChildren<Button>();

		foreach(Button button in buttons) {

			listButton.Add(button);
		}
	}
}
