using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServicePreguntas : MonoBehaviour {

    private const string URL = "http://sii.uveg.edu.mx/unity/dropsV2/controllers/webServicePreguntas.php";
    private const string API_KEY = "AJFFF-ASFFF-GWEGG-WEGERG-ERGEG-EGERG-ERGEG";//KEY falsa, remplazar por autentica

    /**
     * Estructura que almacena los datos de una pregunta
     */
    [Serializable]
    public class preguntaData {
        public string id = "";
        public string descripcion = "";
        public string status = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
        public string idTipoEjercicio = "";
        public string idMateria = "";
        public string idPaquete = "";
        public string claveMateria = "";
        public string descripcionEjercicio = "";
        public string descripcionPaquete = "";
    }

    [Serializable]
    public class Data {
        public preguntaData[] preguntas;
    }

    /**
     * Función que regresa la estructura preguntaData
     * la cual almacena los datos de las preguntas relacionadas a la materia
     * @param materia, id de la materia de la cual se requieren las preguntas
     */
    public static preguntaData[] getPreguntasByMateria(string materia) {
        string query = "SELECT * FROM pregunta WHERE idMateria = " + materia + " AND status = 1;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            result = "{\"preguntas\":" + "[" + result + "]}";
            Debug.Log(result);
            Data data = JsonUtility.FromJson<Data>(result);
            return data.preguntas;
        } else {
            return null;
        }
    }

    public static int insertarPreguntaSqLite(string descripcion, string status, string fechaRegistro, string fechaModificacion, string idTipoEjercicio, string idMateria, string idPaquete) {
        string query = "INSERT INTO pregunta (descripcion, status, fechaRegistro, fechaModificacion, idTipoEjercicio, idMateria, idPaquete) VALUES ('" + descripcion + "', '" + status + "', '" + fechaRegistro + "','" + fechaModificacion + "', '" + idTipoEjercicio + "', '" + idMateria + "', '" + idPaquete +"');";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    public static preguntaData getPreguntaByDescripcionSqLite(string descripcion) {
        string query = "SELECT * FROM pregunta WHERE descripcion = '" + descripcion + "';";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            preguntaData data = JsonUtility.FromJson<preguntaData>(result);
            return data;
        } else {
            return null;
        }
    }


    public static IEnumerator getPreguntas() {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;

        form.AddField("metodo", "consultarPreguntas");
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
                    Debug.Log("No se encontraron preguntas");
                } else {
                    text = "{\"preguntas\":" + text + "}";
                    Data myObject = JsonUtility.FromJson<Data>(text);
                    GameObject.Find("AppManager").GetComponent<appManager>().setPreguntas(myObject.preguntas);
                }
            }
        }
    }


    public static IEnumerator getPreguntasOfPack(string paquete) {
        Debug.Log(paquete);
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;

        form.AddField("metodo", "consultarPreguntasOfPack");
        form.AddField("descripcion", paquete);
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
                    Debug.Log("No se encontraron preguntas");
                } else {
                    text = "{\"preguntas\":" + text + "}";
                    Data myObject = JsonUtility.FromJson<Data>(text);
                    GameObject.Find("AppManager").GetComponent<appManager>().setPreguntas(myObject.preguntas);
                }
            }
        }
    }

}
