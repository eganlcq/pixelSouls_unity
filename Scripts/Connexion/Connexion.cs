using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connexion : MonoBehaviour
{
    [SerializeField]
    private InputField _fieldMail;
    [SerializeField]
    private InputField _fieldPassword;
    [SerializeField]
    private Text _error;

    public void StartConnect() {

        DBManager.Instance.user = null;
        StartCoroutine(Connect());
    }

    private IEnumerator Connect() {

        UserConnexion user = new UserConnexion();
        user.mail = _fieldMail.text;
        user.password = _fieldPassword.text;

        DBManager.Instance.Connexion(user);
        yield return new WaitUntil(() => DBManager.Instance.requestDone);
        if(DBManager.Instance.user != null) {

            _error.enabled = false;
            GameManager.Instance.ChangeLevel("chooseCharacter");
        }
        else {

            _error.enabled = true;
        }
    }

    public void QuitApp() {

        Application.Quit();
    }

    public void createAccount() {

        Application.OpenURL("https://www.pixelsouls.be/register");
    }
}
