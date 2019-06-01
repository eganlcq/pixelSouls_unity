using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSelector : MonoBehaviour
{
	[Header("-- Where to put layouts --")]
	public GameObject contentContainer;

	[Header("-- Characters chosen --")]
	public FightData data;

    [Header("-- GUI SOUND --")]
    [SerializeField]
    protected ButtonSound _guiSound;
    [SerializeField]
    protected AudioClip _validateSound;
    [SerializeField]
    protected AudioClip _backSound;

	public static CharacterSelector Instance;

	protected DBManager manager;
	protected const string FRAME_CHARACTER = "Prefab/GUI/Character Selection/Frame Character";
	protected const string FRAME_OPPONENT = "Prefab/GUI/Character Selection/Frame Opponent";
	protected const string FRAME_EMPTY = "Prefab/GUI/Character Selection/Frame Empty";
	protected const string H_LAYOUT = "Prefab/GUI/Character Selection/Horizontal Layout";

	protected List<GameObject> listLayouts = new List<GameObject>();
	protected List<GameObject> listFrame = new List<GameObject>();
	protected List<Fighter> fullListOfFighter = new List<Fighter>();

	protected int nbColumn = 2;

	protected virtual void Awake() {
		
		Instance = this;
	}

	// Start is called before the first frame update
	protected virtual void Start()
    {
		manager = DBManager.Instance;
		StartCoroutine(Display());
	}

	protected abstract IEnumerator Display();

	protected virtual void GetFighters() {

		fullListOfFighter = new List<Fighter>(manager.fighters);
	}

	protected virtual void FillList(int nbFrame, List<GameObject> layouts) {

		float nbLayouts = Mathf.Ceil(nbFrame / 2f);

		for (int i = 0; i < nbLayouts; i++) {

			layouts.Add(Instantiate(Resources.Load(H_LAYOUT)) as GameObject);
		}
	}
	
	protected abstract void FillList(List<Fighter> fighters, List<GameObject> frames, List<GameObject> layouts);

	protected void FillLayouts(List<GameObject> frames, List<GameObject> layouts) {

		int index = 0;

		foreach (GameObject layout in layouts) {

			layout.transform.SetParent(contentContainer.transform, false);

			for (int i = 0; i < nbColumn && index < frames.Count; i++) {

				frames[index].transform.SetParent(layout.transform, false);
				index++;
			}
		}
	}

	protected abstract void ModifyDatas(Fighter fighter, GameObject frame);

	protected void CharacterSelected(Fighter fighter) {

		data.characterChosen = fighter;
	}

	protected void OpponentSelected(Fighter fighter) {

		data.opponentChosen = fighter;
	}
}
