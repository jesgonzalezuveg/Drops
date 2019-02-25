﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceRespuestas : MonoBehaviour {

    private const string URL = "http://sii.uveg.edu.mx/unity/dropsV2/controllers/webServiceRespuestas.php";
    private const string API_KEY = "AJFFF-ASFFF-GWEGG-WEGERG-ERGEG-EGERG-ERGEG";//KEY falsa, remplazar por autentica

    [Serializable]
    public class respuestaData{
        public string id = "";
        public string descripcion = "";
        public string urlImagen = "";
        public string correcto = "";
        public string relacion = "";
        public string status = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
        public string idPregunta = "";
        public string descripcionPregunta = "";
    }

    [Serializable]
    public class Data {
        public respuestaData[] respuestas;
    }

    public static Data getRespuestasByPreguntaSqLite(string idPregunta) {
        string query = "SELECT * FROM respuesta WHERE idPregunta = '" + idPregunta + "';";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            Data respuesta = JsonUtility.FromJson<Data>(result);
            return respuesta;
        } else {
            return null;
        }
    }

    public static respuestaData getRespuestaByDescripcionSqLite(string descripcion) {
        string query = "SELECT * FROM respuesta WHERE descripcion = '" + descripcion + "';";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            respuestaData respuesta = JsonUtility.FromJson<respuestaData>(result);
            return respuesta;
        } else {
            return null;
        }
    }

    public static respuestaData getRespuestaByDescripcionAndPreguntaSquLite(string descripcion, string idPregunta) {
        string query = "SELECT * FROM respuesta WHERE descripcion = '" + descripcion + "' AND idPregunta = '" + idPregunta + "';";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            respuestaData respuesta = JsonUtility.FromJson<respuestaData>(result);
            return respuesta;
        } else {
            return null;
        }
    }

    public static int insertarRespuestaSqLite(string descripcion, string urlImagen, string correcto, string relacion, string status, string fechaRegistro, string fechaModificacion, string idPregunta) {
        string query = "INSERT INTO respuesta (descripcion, urlImagen, correcto, relacion, status, fechaRegistro, fechaModificacion, idPregunta) VALUES ('" + descripcion + "', '" + urlImagen + "', '" + correcto + "', '" + relacion + "', '" + status + "', '" + fechaRegistro + "', '" + fechaModificacion + "', '" + idPregunta + "'); ";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }


    public static IEnumerator getRespuestas() {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;

        form.AddField("metodo", "consultarRespuestas");
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) {
            AsyncOperation asyncLoad = www.SendWebRequest();
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone) {
                yield return null;
            }

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string text;
                text = www.downloadHandler.text;
                if (text == "") {
                    Debug.Log("No se encontraron respuestas");
                } else {
                    text = "{\"respuestas\":" + text + "}";
                    Data myObject = JsonUtility.FromJson<Data>(text);
                    GameObject.Find("AppManager").GetComponent<appManager>().setRespuestas(myObject.respuestas);
                }
            }
        }
    }

    public static IEnumerator getRespuestasByPack(string descripcionPack) {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;

        form.AddField("metodo", "consultarRespuestasByPack");
        form.AddField("paquete", descripcionPack);
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) {
            AsyncOperation asyncLoad = www.SendWebRequest();
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone) {
                yield return null;
            }
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string text;
                text = www.downloadHandler.text;
                if (text == "") {
                    Debug.Log("No se encontraron respuestas");
                } else {
                    text = "{\"respuestas\":" + text + "}";
                    Data myObject = JsonUtility.FromJson<Data>(text);
                    GameObject.Find("AppManager").GetComponent<appManager>().setRespuestas(myObject.respuestas);
                }
            }
        }
    }

}
