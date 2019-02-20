using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServicePaquetes : MonoBehaviour{

    private const string URL = "http://sii.uveg.edu.mx/unity/dropsV2/controllers/webServicePaquetes.php";
    private const string API_KEY = "AJFFF-ASFFF-GWEGG-WEGERG-ERGEG-EGERG-ERGEG";//KEY falsa, remplazar por autentica

    /**
     * Estructura que almacena los datos de un paquete
     */
    [Serializable]
    public class paqueteData {
        public string id = "";
        public string descripcion = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
    }

    [Serializable]
    public class Data {
        public paqueteData[] paquete;
    }


    /**
     * Funcion que regresa los paquetes que existen en la base de datos local
     * 
     */
    public static paqueteData getPaquetesSqLite() {
        string query = "SELECT * FROM paquete;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            paqueteData paquete = JsonUtility.FromJson<paqueteData>(result);
            return paquete;
        } else {
            return null;
        }
    }

    public static paqueteData getPaquetesByDescripcionSqLite(string descripcion) {
        string query = "SELECT * FROM paquete WHERE descripcion = '" + descripcion + "';";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            paqueteData paquete = JsonUtility.FromJson<paqueteData>(result);
            return paquete;
        } else {
            return null;
        }
    }

    public static int insertarPaqueteSqLite(string descripcion, string fechaCreacion, string fechaModificacion) {
        string query = "INSERT INTO paquete (descripcion, fechaRegistro, fechaModificacion) VALUES ('" + descripcion + "','" + fechaCreacion + "','" + fechaModificacion + "');";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }


    public static IEnumerator getPaquetes() {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;

        form.AddField("metodo", "consultarPaquetes");
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string text;
                text = www.downloadHandler.text;
                if (text == "") {
                    Debug.Log("No se encontraron paquetes");
                } else {
                    text = "{\"paquete\":" + text + "}";
                    Data myObject = JsonUtility.FromJson<Data>(text);
                    GameObject.Find("AppManager").GetComponent<appManager>().setPaquetes(myObject.paquete);
                }
            }
        }
    }

}
