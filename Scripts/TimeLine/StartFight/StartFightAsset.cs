using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class StartFightAsset : PlayableAsset, ITimelineClipAsset {

	//private StartFight template = new StartFight();

	public ClipCaps clipCaps {

		get {

			return ClipCaps.None;
		}
	}

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {

		return ScriptPlayable<StartFight>.Create(graph);
	}
}
