using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            StartCoroutine(webServicePreguntas.getPreguntasOfPack(paquete));
            StartCoroutine(webServiceRespuestas.getRespuestasByPack(paquete));
            webServiceDescarga.insertarDescargaSqLite(paqueteId, webServiceUsuario.consultarIdUsuarioSqLite(manager.getUsuario()));
        } else {
            StartCoroutine(webServicePreguntas.getPreguntasOfPack(paquete));
            StartCoroutine(webServiceRespuestas.getRespuestasByPack(paquete));
            webServiceDescarga.insertarDescargaSqLite(paqueteId, webServiceUsuario.consultarIdUsuarioSqLite(manager.getUsuario()));
        }
    }

}
