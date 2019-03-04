using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class packManager : MonoBehaviour {

    public webServicePaquetes.paqueteData paquete = null;
    private appManager manager;
    public GameObject mensaje;

    public void Start() {
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
        if (mensaje) {
            mensaje.SetActive(false);
        }
    }

    public void descargaPaquete() {
        mensaje.SetActive(true);
        mensaje.GetComponentInChildren<Text>().text = "Descargando paquete";
        GetComponentInChildren<Button>().interactable = false;
        StartCoroutine(webServicePreguntas.getPreguntasOfPack(paquete.descripcion));
        StartCoroutine(webServiceRespuestas.getRespuestasByPack(paquete.descripcion));
        webServiceDescarga.insertarDescargaSqLite(paquete.id, webServiceUsuario.consultarIdUsuarioSqLite(manager.getUsuario()));
    }

    public void jugarPaquete() {
        Debug.Log(paquete.id);
        manager.preguntasCategoria = webServicePreguntas.getPreguntasByPackSqLite(paquete.id);
        SceneManager.LoadScene("salon");
    }

    public void actualizarPaquete() {
        Debug.Log("actualizar");
    }

}
