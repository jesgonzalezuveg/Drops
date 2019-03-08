using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    public GameObject pantallaCargando;
    public GameObject consola;
    bool isInMesagge = false;
    Quaternion rotationLock;

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

    public void setMensaje2(bool active, string mensaje) {
        consola.SetActive(active);
        consola.GetComponentInChildren<Text>().text = mensaje;
    }

    // Start is called before the first frame update
    void Start() {
        setMensaje(false, "");
        Time.timeScale = 1;
    }

    private void Update() {
        if (isInMesagge) {
            gameObject.GetComponentInChildren<Camera>().gameObject.transform.localRotation = rotationLock;
        }
    }

    public void regresar(string escenaAnterior) {
        SceneManager.LoadScene(escenaAnterior);
    }
}
