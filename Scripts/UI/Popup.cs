using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
	private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

	public void Pop() {

		anim.SetTrigger("pop");
	}

	public void DePop() {

		anim.SetTrigger("depop");
	}
}
