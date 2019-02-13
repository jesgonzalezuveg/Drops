using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuManager : MonoBehaviour {

    public void pairCode() {
        SceneManager.LoadScene(1);
    }

    public void facebookLogin() {
        SceneManager.LoadScene(2);
    }

    public void invitado() {
        SceneManager.LoadScene("");
    }

    public void login() {
        SceneManager.LoadScene(3);
    }
}
