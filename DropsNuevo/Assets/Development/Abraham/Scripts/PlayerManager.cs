using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    public GameObject pantallaCargando;     ///< pantallaCargando Referencia al canvas del mensaje cargando
    //public GameObject consola;              ///< consola Refenrencia al canvas que muetra la consola inGame
    bool isInMesagge = false;               ///< isInMesagge bandera que valida si se encuentra activa o no la pantalla cargando
    Quaternion rotationLock;                ///< rotationLock quaternion que contiene la rotacion de la camara en el momento que la pantallaCarga se activa

    /**
     * Funcion que activa o desactiva la pantallaCargando
     * @active true o false que se encarga de activar/desactivar la pantalla
     * @mensaje texto que mostrara la pantalla de carga
     */
    public void setMensaje(bool active, string mensaje) {
        if (active == true) {
            Time.timeScale = 0;
            isInMesagge = true;
            rotationLock = gameObject.GetComponentInChildren<Camera>().gameObject.transform.localRotation;
        } else {
            Time.timeScale = 1;
            isInMesagge = false;
        }
        pantallaCargando.SetActive(active);
        pantallaCargando.GetComponentInChildren<Text>().text = mensaje;
    }

    /**
     * Funcion que llena la consola
     */
    public void setMensaje2(bool active, string mensaje) {
        //consola.SetActive(active);
        //consola.GetComponentInChildren<Text>().text = mensaje;
    }

    /**
     * Funcion que se manda llamar al inicio de la escena (frame 1)
     * oculta el mensaje y establece Time.timeScale a 1
     */
    void Start() {
        setMensaje(false, "");
        Time.timeScale = 1;
    }


    /**
     * Funcion que se manda llamar cada frame
     * En caso de que se encuentre activa la pantella de carga bloquea la posicion de la camara
     */
    private void Update() {
        if (isInMesagge) {
            GameObject.Find("RightEyeAnchor").GetComponent<Camera>().gameObject.transform.localRotation = rotationLock;
            GameObject.Find("LeftEyeAnchor").GetComponent<Camera>().gameObject.transform.localRotation = rotationLock;
        }
    }

    /**
     * Funcion que se manda llamar cada que se da click e el boton regresar
     * Regresa a la escena que se le indique
     * @escenaAnterior escena que se desea cargar
     */
    public void regresar(string escenaAnterior) {
        SceneManager.LoadScene(escenaAnterior);
    }
}
