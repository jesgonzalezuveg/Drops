using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenuManager : MonoBehaviour {


    /**
     * Funcion que se manda llamar al inicio de la escena (frame 1) 
     * Verifica si el dispositivo en el que se esta ejecuntando es un oculus.
     * en caso que si oculta el boton de login con facebook
     */
    private void Start() {
        GameObject.FindObjectOfType<appManager>().validarConexion();
        var cadenas = SystemInfo.deviceModel.Split(' ');
        Debug.Log(cadenas[0]);
        if (!GameObject.FindObjectOfType<appManager>().isOnline) {
            GameObject.Find("Facebook").GetComponent<Button>().interactable = false;
            GameObject.Find("PairCode").GetComponent<Button>().interactable = false;
            GameObject.Find("Logn").GetComponent<Button>().interactable = false;
        }
        if (cadenas[0] == "Oculus") {
            Debug.Log("Hiding Facebook login");
            GameObject.Find("Facebook").SetActive(false);
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
        StartCoroutine(loadScene("ParingCode"));
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
