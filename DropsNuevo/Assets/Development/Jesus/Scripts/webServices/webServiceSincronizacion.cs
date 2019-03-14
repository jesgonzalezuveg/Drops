﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class webServiceSincronizacion : MonoBehaviour
{
    //URL de webservice del SII para los procesos del pairing code
    private const string URL = "http://sii.uveg.edu.mx/unity/dropsV2/controllers/webServiceSincronizacion.php";
    private const string API_KEY = "AJFFF-ASFFF-GWEGG-WEGERG-ERGEG-EGERG-ERGEG";//KEY falsa, remplazar por autentica

    [Serializable]
    public class Registro {
        public string id = "";
        public string detalle = "";
        public string fechaRegistro = "";
        public string idLog = "";
        public string idUsuario = "";
        public string idAccion = "";
    }
    

    [Serializable]
    public class DetalleIntento {
        public string id = "";
        public string correcto = "";
        public string idPregunta = "";
        public string idRespuesta = "";
        public string idIntento = "";
    }

    [Serializable]
    public class Intento {
        public string id = "";
        public string puntaje = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
        public string idLog = "";
        public DetalleIntento[] detalleIntento;
    }

    [Serializable]
    public class Log {
        public string id = "";
        public string fechaInicio = "";
        public string fechaTermino = "";
        public string dispositivo = "";
        public string idCodigo = "";
        public string idUsuario = "";
        public Registro[] registros;
        public Intento[] intentos;
    }

    [Serializable]
    public class Usuario {
        public string id = "";
        public string usuario = "";
        public string nombre = "";
        public string rol = "";
        public string gradoEstudios = "";
        public string programa = "";
        public string fechaRegistro = "";
        public string status = "";
        public Log[] logs;
    }

    [Serializable]
    public class RootObject {
        public Usuario[] Usuarios; 
    }

    public static int changeSyncroStatus(string json) {
        RootObject myObject = JsonUtility.FromJson<RootObject>(json);
        for (int i =0; i < myObject.Usuarios.Length; i++) {
            //myObject.Usuarios[i]
        }
        //myObject.Usuarios
        return 1;
    }

    public static IEnumerator SincroData(string data) {
        //Start the fading process
        WWWForm form = new WWWForm();
        //Debug.Log(idCodigo);
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;

        form.AddField("metodo", "sincronizar");
        form.AddField("dataSincronizacion", data);
        //byte[] rawFormData = form.data;
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) {
            //www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string text;
                text = www.downloadHandler.text;
                text = text.Replace("[", "");
                text = text.Replace("]", "");
                Debug.Log(text);
                Debug.Log("Respuesta json");
                if (text == "1") {
                    SyncroManager.respuestaWsSincro = "1";
                    Debug.Log("Sincronizacion realizada");
                } else {
                    SyncroManager.respuestaWsSincro = "0";
                    Debug.Log("Fallo la sincronizacion");
                }
            }
        }
    }
}