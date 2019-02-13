using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System;
using System.Linq.Expressions;

public class WebServiceCodigo : MonoBehaviour {
    private const string URL = "http://sii.uveg.edu.mx/unity/dropsV2/controllers/webServiceController.php";
    private const string API_KEY = "AJFFF-ASFFF-GWEGG-WEGERG-ERGEG-EGERG-ERGEG";//KEY falsa, remplazar por autentica

    [Serializable]
    public class Data {
        public string id = "";
        public string descripcion = "";
        public string status = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
    }

    // Use this for initialization
    public static IEnumerator insertarCodigo(string codigo) {
        //Start the fading process
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;

        form.AddField("metodo", "insertarCodigo");
        form.AddField("codigo", codigo);
        //byte[] rawFormData = form.data;
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) {
            //www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string text;
                text = www.downloadHandler.text;
                Debug.Log("Form upload complete!");
                Debug.Log(text);
            }
        }
    }


    public static IEnumerator obtenerCodigo(string codigo, int status) {
        //Start the fading process
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;

        form.AddField("metodo", "obtenerCodigo");
        form.AddField("codigo", codigo);
        form.AddField("status", status);
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
                //Debug.Log(text);

                //Data2 jsonResponse = JsonUtility.FromJson<Data2>(text);
                if (text=="0") {
                    //Debug.Log("No se encontro el código");
                    pairingCode.status = text;
                    pairingCode.valCodigoSii = 0;
                } else {
                    //Debug.Log("Se encontro el código");
                    Data data = JsonUtility.FromJson<Data>(text);
                    pairingCode.status = data.status;
                    pairingCode.valCodigoSii = 1;
                }
            }
        }
    }
}
