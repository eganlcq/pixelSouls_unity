using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fight : MonoBehaviour
{
	public static Fight Instance;

	[Header("-- Fighters --")]
	[SerializeField]
	private FightersFight character;
	[SerializeField]
	private FightersFight opponent;

	[Header("-- EndFight --")]
	public Animator canvasAnim;
	[SerializeField]
	private Text endText;
	[SerializeField]
	private ExperienceBar experienceBar;
	[SerializeField]
	private Text levelText;
	[SerializeField]
	private Text strengthValue;
	[SerializeField]
	private Text dexterityValue;
	[SerializeField]
	private Text vitalityValue;
	[SerializeField]
	private Image weaponImg;

	[Header("-- Data --")]
	[SerializeField]
	private FightData data;
	[SerializeField]
	private OpponentData opponentData;
	private Fighter fighter;
	private Fighter otherFighter;
	[HideInInspector]
	public float experience;
	[HideInInspector]
	public float experienceNeeded;
	private float level;
	[HideInInspector]
	public int strength;
	[HideInInspector]
	public int dexterity;
	[HideInInspector]
	public int vitality;
	[HideInInspector]
	public bool levelUp = false;
	private List<Weapon> weapons = new List<Weapon>();
	private Weapon weaponToAdd;

    [Header("-- SOUND --")]
    [SerializeField]
    private AudioSource _source;
    [SerializeField]
    private AudioClip _winSound;
    [SerializeField]
    private AudioClip _looseSound;

    [Header("-- GUI --")]
    [SerializeField]
    private Image _characterImg;
    [SerializeField]
    private Image _opponentImg;
    [SerializeField]
    private Text _characterName;
    [SerializeField]
    private Text _opponentName;

	private int f_attackWon;
	private int f_attackLost;
	private int o_defenseWon;
	private int o_defenseLost;

    private int score;
    private int opponentLevel;

	[HideInInspector]
	public float nbExpEarned = 0;

	private void Awake() {
		
		Instance = this;
		fighter = data.characterChosen;
		otherFighter = data.opponentChosen;
		experience = float.Parse(fighter.experience);
		experienceNeeded = float.Parse(fighter.experienceNeeded);
		level = float.Parse(fighter.level);
		strength = int.Parse(fighter.strength);
		dexterity = int.Parse(fighter.dexterity);
		vitality = int.Parse(fighter.vitality);
		f_attackWon = int.Parse(fighter.attackWon);
		f_attackLost = int.Parse(fighter.attackLost);
		o_defenseWon = int.Parse(otherFighter.defenseWon);
		o_defenseLost = int.Parse(otherFighter.defenseLost);
        score = int.Parse(fighter.owner.score);
        opponentLevel = int.Parse(otherFighter.level);

        _characterName.text = data.characterChosen.name;
        _opponentName.text = data.opponentChosen.name;
        DBManager.Instance.GetTexture("https://www.pixelsouls.be/img/avatars/" + data.characterChosen.owner.image, _characterImg);
        DBManager.Instance.GetTexture("https://www.pixelsouls.be/img/avatars/" + data.opponentChosen.owner.image, _opponentImg);

        //DBManager.Instance.GetTexture("http://127.0.0.1:8000/img/avatars/" + data.characterChosen.owner.image, _characterImg);
        //DBManager.Instance.GetTexture("http://127.0.0.1:8000/img/avatars/" + data.opponentChosen.owner.image, _opponentImg);

        foreach (Weapon weapon in fighter.weapons) {

			weapons.Add(weapon);
		}
		FightersFight.endFightState = false;

		FadeScreen.Instance.FadeOut(null);
		enabled = false;
	}

	private IEnumerator Start() {

		yield return new WaitUntil(() => FightersFight.endFightState);
        StartCoroutine(PlaySound());

		if (character.life == 0) {

			canvasAnim.SetBool("win", false);
			endText.text = "LOOSER";
			nbExpEarned = 0;
			f_attackLost++;
			o_defenseWon++;
            float f = level >= opponentLevel ? (level - opponentLevel + 1) * 100 : 100 - ((-level + opponentLevel) * 10);
            score -= (int)f;
            if (score < 0) score = 0;
            data.isWon = false;
		}
		else {

			canvasAnim.SetBool("win", true);
			endText.text = "WINNER";
			nbExpEarned = level <= opponentLevel ? (-level + opponentLevel + 1) * 10 : 10 - (level - opponentLevel);
			f_attackWon++;
			o_defenseLost++;
            float f = level <= opponentLevel ? (-level + opponentLevel + 1) * 100 : 100 - ((level - opponentLevel) * 10);
            score += (int)f;
            data.isWon = true;
		}

        if(level < 20) {

            experience += nbExpEarned;
            experience = Mathf.Clamp(experience, 0, experienceNeeded);
        }
        else {

            experience = experienceNeeded;
        }

		if(experience == experienceNeeded && level < 20) {

			level++;
			experienceNeeded = level * 10;
            if (level < 20) experience = 0;
            else experience = experienceNeeded;
			strength += Random.Range(50, 100);
			dexterity += Random.Range(50, 100);
			vitality += Random.Range(50, 100);
			
			levelText.text = level.ToString();

			yield return AddWeapon();

			levelUp = true;
		}

		UpdateData();

		yield return new WaitForSeconds(2f);

		canvasAnim.enabled = true;
	}

    private IEnumerator PlaySound() {

        yield return new WaitForSeconds(0.8f);
        _source.PlayOneShot(character.life == 0 ? _looseSound : _winSound);
        yield return new WaitWhile(() => _source.isPlaying);
        MusicManager.Instance.PlayNoFade(MusicManager.Music.EndFight);
    }

	private void UpdateData() {

		data.characterChosen.experience = experience.ToString();
        data.characterChosen.experienceNeeded = experienceNeeded.ToString();
        data.characterChosen.level = level.ToString();
        data.characterChosen.strength = strength.ToString();
        data.characterChosen.dexterity = dexterity.ToString();
        data.characterChosen.vitality = vitality.ToString();
        data.characterChosen.attackWon = f_attackWon.ToString();
        data.characterChosen.attackLost = f_attackLost.ToString();
        data.characterChosen.owner.score = score.ToString();
        data.opponentChosen.defenseWon = o_defenseWon.ToString();
        data.opponentChosen.defenseLost = o_defenseLost.ToString();
		DBManager.Instance.SendFighterData(data);

		if(levelUp) {

			DBManager.Instance.SendWeaponData(fighter, weaponToAdd);
		}
	}

	private IEnumerator AddWeapon() {

		DBManager.Instance.FindRemainingWeapons(fighter.id);
		yield return new WaitUntil(() => DBManager.Instance.requestDone);

		List<Weapon> remainingWeapons = new List<Weapon>(DBManager.Instance.weapons);
		int rand = Random.Range(0, remainingWeapons.Count);
		weaponToAdd = remainingWeapons[rand];
		weaponImg.sprite = ListWeapon.Instance.GetSprite(ListWeapon.Instance.GetWeapon(weaponToAdd.name));

		weapons.Add(weaponToAdd);
		fighter.weapons = weapons.ToArray();
	}

	public void StartFight() {

		enabled = true;
		character.enabled = true;
		opponent.enabled = true;
	}

	public void StartOver() {

		GameManager.Instance.ChangeLevel(SceneManager.GetActiveScene().name);
	}

	public void QuitFight() {

		GameManager.Instance.ChangeLevel("selectOpponent");
	}
}
