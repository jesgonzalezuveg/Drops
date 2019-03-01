using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class packManager : MonoBehaviour {

    public string paquete = "";
    public string paqueteId = "";
    public bool existe = false;
    private appManager manager;

    public void Start() {
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
    }

    public void descargaPaquete() {
        if (existe) {
            //Update tables
            GetComponentInChildren<Button>().interactable = false;
            StartCoroutine(webServicePreguntas.getPreguntasOfPack(paquete));
            StartCoroutine(webServiceRespuestas.getRespuestasByPack(paquete));
            webServiceDescarga.insertarDescargaSqLite(paqueteId, webServiceUsuario.consultarIdUsuarioSqLite(manager.getUsuario()));
        } else {
            GetComponentInChildren<Button>().interactable = false;
            StartCoroutine(webServicePreguntas.getPreguntasOfPack(paquete));
            StartCoroutine(webServiceRespuestas.getRespuestasByPack(paquete));
            webServiceDescarga.insertarDescargaSqLite(paqueteId, webServiceUsuario.consultarIdUsuarioSqLite(manager.getUsuario()));
        }
    }

}
