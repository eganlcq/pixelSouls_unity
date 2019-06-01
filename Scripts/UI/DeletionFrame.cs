using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeletionFrame : MonoBehaviour
{
	private Button delete;
	private Button revoke;

    private UnityAction _deleteChar;
    private UnityAction _buttonOff;
    private UnityAction _disableListener;

    // Start is called before the first frame update
    void Start()
    {
        delete = GetComponentsInChildren<Button>()[0];
		revoke = GetComponentsInChildren<Button>()[1];
    }

    public void EnableClick(string id, List<Button> buttons) {

        _deleteChar = () => DBManager.Instance.DeleteCharacter(id);
        _buttonOff = () => ButtonOnOff(buttons, true);
        _disableListener = () => DisableClick();

        delete.onClick.AddListener(_deleteChar);
		revoke.onClick.AddListener(_buttonOff);
        revoke.onClick.AddListener(_disableListener);
		ButtonOnOff(buttons, false);
	}

    public void DisableClick() {

        delete.onClick.RemoveListener(_deleteChar);
        revoke.onClick.RemoveListener(_buttonOff);
        revoke.onClick.RemoveListener(_disableListener);
    }

	private void ButtonOnOff(List<Button> buttons, bool enable) {

		foreach (Button button in buttons) {

			button.interactable = enable;
		}
	}
}
