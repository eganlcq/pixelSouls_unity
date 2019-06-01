using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class StartFight : PlayableBehaviour
{
	public override void OnBehaviourPlay(Playable playable, FrameData info) {

		Fight.Instance.StartFight();
	}
}
