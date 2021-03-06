﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class mainMenuManager : MonoBehaviour {

    public GameObject[] vistas;
    GameObject vistaActiva;

    public void Start() {
        vistaActiva = vistas[0];
    }

    /**
     * Función que se ejecuta al pulzar el boton Jugar
     * 
     */
    public void cambiarVista(int vista) {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        vistaActiva.SetActive(false);
        vistas[vista].SetActive(true);
        vistaActiva = vistas[vista];
        if (GameObject.FindObjectOfType<keyboardManager>()) {
            GameObject.FindObjectOfType<keyboardManager>().setUsuario("");
            GameObject.FindObjectOfType<keyboardManager>().setNombre("");
            GameObject.FindObjectOfType<keyboardManager>().setPassword("");
            GameObject.FindObjectOfType<keyboardManager>().setPassword2("");
            foreach (var input in GameObject.FindObjectOfType<keyboardManager>().inputs) {
                input.GetComponentInChildren<Text>().text = "";
            }
        }
    }

    public void pairingCode() {
        StartCoroutine(GameObject.Find("AppManager").GetComponent<appManager>().cambiarEscena("ParingCode", "mainMenu"));
    }

    /**
     * Coroutine que espera a que termine de reprocucir el audio de click de los botones
     * para poder ir a la nueva escena
     */
    IEnumerator loadScene(string scene) {
        yield return new WaitUntil(() => this.GetComponent<AudioSource>().isPlaying == false);
        StartCoroutine(GameObject.Find("AppManager").GetComponent<appManager>().cambiarEscena(scene,"mainMenu"));
    }
}
