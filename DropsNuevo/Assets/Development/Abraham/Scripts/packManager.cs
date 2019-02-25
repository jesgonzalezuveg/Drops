using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class packManager : MonoBehaviour {

    public string paquete = "";
    private appManager manager;
    private bool bandera = true;

    public void Start() {
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
    }

    public void descargaPaquete() {
        StartCoroutine(webServicePreguntas.getPreguntasOfPack(paquete));
        StartCoroutine(webServiceRespuestas.getRespuestasByPack(paquete));
    }

}
