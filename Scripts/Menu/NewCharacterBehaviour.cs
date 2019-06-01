using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacterBehaviour : MonoBehaviour
{
    [SerializeField]
    private ChooseCharacter _chooseCharacter;
    private TouchScreenKeyboard _keyboard;

    public void StartEnterName() {

        _keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, true);
        StartCoroutine(EnterNameRoutine());
    }

    private IEnumerator EnterNameRoutine() {

        yield return new WaitUntil(() => _keyboard.status == TouchScreenKeyboard.Status.Done && _keyboard.text != "");
        _chooseCharacter.CreateCharacter(_keyboard.text);
        _keyboard.active = false;
    }

    private void Update() {
        
        if(_keyboard != null) {

            if(_keyboard.status == TouchScreenKeyboard.Status.Done && _keyboard.text == "") {

                if (_keyboard.active == false) _keyboard.active = true;
            }
            else if(_keyboard.status == TouchScreenKeyboard.Status.LostFocus) _keyboard.active = true;
        }
    }

    public void OpenKeyboard() {

        _keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, true);
    }
}
