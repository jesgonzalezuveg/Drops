﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
        //StopAllCoroutines();
        if (mensaje) {
            mensaje.SetActive(true);
            mensaje.GetComponentInChildren<Text>().text = "Descargando paquete";
        }
        webServiceRegistro.validarAccionSqlite("Descarga: " + paquete.descripcion, manager.getUsuario(), "Descargar paquete");
        consultarDatos();
    }

    //Sustituir valor "10" con el numero de reactivos que seleccione el usuario
    public void jugarPaquete() {
        //StopAllCoroutines();
        manager.preguntasCategoria = webServicePreguntas.getPreguntasByPackSqLiteCurso(paquete.id, 5);
        webServiceRegistro.validarAccionSqlite("Partida: " + paquete.descripcion, manager.getUsuario(), "Comenzó ejercicio");
        SceneManager.LoadScene("salon");
    }

    public void actualizarPaquete() {
        //StopAllCoroutines();
        if (mensaje) {
            mensaje.SetActive(true);
            mensaje.GetComponentInChildren<Text>().text = "Actualizando paquete";
        }
        webServiceRegistro.validarAccionSqlite("Actualización : " + paquete.descripcion, manager.getUsuario(), "Actualizar paquete");
        consultarDatos();
    }

    void consultarDatos() {
        manager.setPreguntas(null);
        manager.setRespuestas(null);
        foreach (var k in GetComponentsInChildren<Button>()) {
            k.interactable = false;
        }
        StartCoroutine(webServicePreguntas.getPreguntasOfPack(paquete.descripcion));
        StartCoroutine(webServiceRespuestas.getRespuestasByPack(paquete.descripcion));
        webServiceDescarga.insertarDescargaSqLite(paquete.id, webServiceUsuario.consultarIdUsuarioSqLite(manager.getUsuario()));
    }

}
