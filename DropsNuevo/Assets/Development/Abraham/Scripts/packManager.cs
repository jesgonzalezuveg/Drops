using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class packManager : MonoBehaviour {

    public string paquete = "";
    public string paqueteId = "";
    private appManager manager;

    public void Start() {
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
    }

    public void descargaPaquete() {
        StartCoroutine(webServicePreguntas.getPreguntasOfPack(paquete));
        StartCoroutine(webServiceRespuestas.getRespuestasByPack(paquete));
        webServiceDescarga.insertarDescargaSqLite(paqueteId,webServiceUsuario.consultarIdUsuarioSqLite(manager.getUsuario()));
    }

}
