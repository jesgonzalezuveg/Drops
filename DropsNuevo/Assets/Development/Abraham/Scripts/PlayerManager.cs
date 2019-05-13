using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    public GameObject pantallaCargando;     ///< pantallaCargando Referencia al canvas del mensaje 
    bool isInMesagge = false;               ///< isInMesagge bandera que valida si se encuentra activa o no la pantalla cargando
    Quaternion rotationLock;                ///< rotationLock quaternion que contiene la rotacion de la camara en el momento que la pantallaCarga se activa
    public GameObject appManager;

    /**
     * Funcion que activa o desactiva la pantallaCargando
     * @active true o false que se encarga de activar/desactivar la pantalla
     * @mensaje texto que mostrara la pantalla de carga
     */
    public void setMensaje(bool active, string mensaje) {
        if (active == true) {
            isInMesagge = true;
            rotationLock = gameObject.GetComponentInChildren<Camera>().gameObject.transform.localRotation;
        } else {
            isInMesagge = false;
        }
        pantallaCargando.SetActive(active);
        pantallaCargando.GetComponentInChildren<Text>().text = mensaje;
    }

    private void Awake() {
        if (GameObject.FindObjectOfType<appManager>() == null ) {
            appManager = Instantiate(appManager);
            appManager.name = "AppManager";
        }
    }

    /**
     * Funcion Start
     */
    void Start() {
    }


    /**
     * Funcion Update
     */
    private void Update() {

    }

    /**
     * Funcion que se manda llamar cada que se da click e el boton regresar
     * Regresa a la escena que se le indique
     * @escenaAnterior escena que se desea cargar
     */
    public void regresar(string escenaAnterior) {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        StartCoroutine(GameObject.Find("AppManager").GetComponent<appManager>().cambiarEscena(escenaAnterior,appManager.GetComponent<appManager>().actual));
    }

    public void cerrarApp() {
        Application.Quit();
    }
}
