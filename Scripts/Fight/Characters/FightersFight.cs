using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FightersFight : MonoBehaviour
{
	public static FightersFight Instance;
	private static bool attackingState = false;
	public static bool endFightState = false;

	[Header("-- Data --")]
	public FightData data;

	[Header("-- Animator --")]
	[SerializeField]
	protected Animator charAnim;
	
	[Header("-- Adversary --")]
	[SerializeField]
	protected FightersFight adversary;

	[Header("-- Weapon --")]
	[SerializeField]
	protected SpriteRenderer weaponSprite;

    [Header("-- SOUND --")]
    [SerializeField]
    private AudioClip _hitSound;
    [SerializeField]
    private AudioClip _weaponSound;
    private AudioSource _source;
    public AudioSource Source { get { return _source; } }

    [HideInInspector]
	public float life;
	[HideInInspector]
	public float stamina;
	protected float strength;
	protected float baseStrength;
	protected float power;
	protected float baseDexterity;
	protected float dexterity;
	protected float timeToWait;
	protected int timeNotChangingWeapon;
	protected readonly int minTimeBeforeChange = 0;
	protected readonly int maxTimeBeforeChange = 3;
	protected List<WeaponId> weapons = new List<WeaponId>();
	protected Dictionary<WeaponId, float> weaponStrength = new Dictionary<WeaponId, float>();
	protected Dictionary<WeaponId, float> weaponSpeed = new Dictionary<WeaponId, float>();
	protected WeaponId currentWeapon = WeaponId.None;
	
	protected Fighter self;
	protected Animator anim;
	protected SpriteRenderer render;

	protected virtual void Awake() {
		
		Instance = this;
		life = float.Parse(self.vitality);
		strength = float.Parse(self.strength);
		baseStrength = strength;
		power = strength / 10f;
		dexterity = float.Parse(self.dexterity);
		baseDexterity = dexterity;
		timeToWait = (100f / dexterity) * 3f;
		timeNotChangingWeapon = Random.Range(minTimeBeforeChange, maxTimeBeforeChange);
		anim = GetComponent<Animator>();
		render = GetComponentInChildren<SpriteRenderer>();
        _source = GetComponent<AudioSource>();
		enabled = false;
	}

	protected virtual void Start() {

		foreach (Weapon weaponData in self.weapons) {

			WeaponId weapon = ListWeapon.Instance.GetWeapon(weaponData.name);

			weapons.Add(weapon);
			weaponStrength.Add(weapon, float.Parse(weaponData.power) / 10f);
			weaponSpeed.Add(weapon, float.Parse(weaponData.speed) / 10f);
		}

		StartCoroutine(FillStamina());
	}

	protected IEnumerator FillStamina() {

		float elapsedTime = 0;
		stamina = 0;

		while (elapsedTime < timeToWait) {
			
			if(attackingState) yield return new WaitUntil(() => !attackingState);
			if (endFightState) yield break;
			stamina = Mathf.Clamp01(elapsedTime / timeToWait);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

        stamina = 1;

		if (attackingState) yield return new WaitUntil(() => !attackingState);
		if (endFightState) yield break;

		attackingState = true;
		yield return Attack();
		attackingState = false;
		
		StartCoroutine(FillStamina());
	}

	public IEnumerator Attack() {

		render.sortingOrder = 1;
		adversary.render.sortingOrder = 0;

		if(weapons.Count > 0 && timeNotChangingWeapon == 0) {

			yield return ChangeWeapon();
		}

		if(weapons.Count > 0) {

			timeNotChangingWeapon--;
		}

		anim.SetTrigger("attack");

		yield return new WaitForSeconds(0.2f);

		charAnim.SetTrigger("attack");

		yield return new WaitForSeconds(0.1f);

		Hit(adversary);

		yield return new WaitForSeconds(0.8f);

		if(endFightState) {

			charAnim.SetTrigger("reset");
			adversary.charAnim.SetTrigger("dead");
            _source.volume = 0.2f;
			yield break;
		}

		ResetAnim();
		adversary.ResetAnim();

		yield return new WaitForSeconds(0.5f);
	}

	protected void Hit(FightersFight fighter) {

		fighter.Damage();
		fighter.life -= power;
		if (fighter.life <= 0) {

			fighter.life = 0;
			endFightState = true;
            MusicManager.Instance.StopMusic();
            _source.volume = 0.8f;
		}
        _source.PlayOneShot(_hitSound);
    }

	protected void Damage() {

		anim.SetTrigger("hit");
		charAnim.SetTrigger("hit");
	}

	public void ResetAnim() {
		
		anim.SetTrigger("reset");
		charAnim.SetTrigger("reset");
	}

	protected IEnumerator ChangeWeapon() {

		timeNotChangingWeapon = Random.Range(minTimeBeforeChange + 1, maxTimeBeforeChange + 1);
		int rand = Random.Range(0, weapons.Count);

		if (currentWeapon != WeaponId.None) {

			weapons.Add(currentWeapon);
		}

		currentWeapon = weapons[rand];
		weapons.Remove(currentWeapon);

		strength = baseStrength;
		dexterity = baseDexterity;

		strength *= weaponStrength[currentWeapon];
		dexterity *= weaponSpeed[currentWeapon];
		power = strength / 10f;
		timeToWait = (100f / dexterity) * 3f;

		charAnim.SetTrigger("changeWeapon");
		weaponSprite.sprite = ListWeapon.Instance.GetSprite(currentWeapon);
		charAnim.SetFloat("weapon", ListWeapon.Instance.GetFloat(currentWeapon));
        _source.PlayOneShot(_weaponSound);

		yield return new WaitForSeconds(1f);

		weaponSprite.sprite = null;
		charAnim.SetTrigger("reset");
	}
}
