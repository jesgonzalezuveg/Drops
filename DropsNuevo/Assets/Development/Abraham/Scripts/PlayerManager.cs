using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    public GameObject pantallaCargando;
    public GameObject botonRegresar;

    public void setMensaje(bool active, string mensaje) {
        pantallaCargando.SetActive(active);
        pantallaCargando.GetComponentInChildren<Text>().text = mensaje;
    }

    // Start is called before the first frame update
    void Start() {
        setMensaje(false,"");
    }

    public void regresar(string escenaAnterior) {
        SceneManager.LoadScene(escenaAnterior);
    }
}
