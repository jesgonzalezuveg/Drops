using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class mainMenuManager : MonoBehaviour {

    public GameObject facebookButon;    ///< facebookButon boton de login con facebook


    /**
     * Funcion que se manda llamar al inicio de la escena (frame 1) 
     * Verifica si el dispositivo en el que se esta ejecuntando es un oculus.
     * en caso que si oculta el boton de login con facebook
     */
    private void Start() {
        var cadenas = SystemInfo.deviceModel.Split(' ');
        Debug.Log(cadenas[0]);
        if (cadenas[0] == "Oculus") {
            Debug.Log("Hiding Facebook login");
            facebookButon.SetActive(false);
        }
    }

    /**
     * Función que se ejecuta al pulzar el boton login con pair code
     * 
     */
    public void pairCode() {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(loadScene("sampleScene"));
    }

    /**
     * Función que se ejecuta al pulzar el boton login
     * 
     */
    public void login() {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(loadScene("login"));
    }

    /**
     * Función que se ejecuta al pulzar el boton login invitado
     * 
     */
    public void invitado() {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(loadScene("menuCategorias"));
    }

    /**
     * Coroutine que espera a que termine de reprocucir el audio de click de los botones
     * para poder ir a la nueva escena
     */
    IEnumerator loadScene(string scene) {
        yield return new WaitUntil(() => this.GetComponent<AudioSource>().isPlaying == false);
        SceneManager.LoadScene(scene);
    }
}
