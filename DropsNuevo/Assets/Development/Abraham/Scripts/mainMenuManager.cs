using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class mainMenuManager : MonoBehaviour {

    private void Start() {
        /*if (SystemInfo.deviceType == oculus) {
            Esconder boton de facebook
        }*/
    }

    /**
     * Función que se ejecuta al pulzar el boton login con pair code
     * 
     */
    public void pairCode() {
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(loadScene("sampleScene"));
    }

    /**
     * Función que se ejecuta al pulzar el boton login
     * 
     */
    public void login() {
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(loadScene("login"));
    }

    /**
     * Función que se ejecuta al pulzar el boton login invitado
     * 
     */
    public void invitado() {
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(loadScene("menuCategorias"));
    }

    IEnumerator loadScene(string scene) {
        yield return new WaitUntil(() => this.GetComponent<AudioSource>().isPlaying == false);
        SceneManager.LoadScene(scene);
    }
}
