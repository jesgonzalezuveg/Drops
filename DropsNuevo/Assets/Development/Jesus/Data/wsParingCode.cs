using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using System;

public class wsParingCode : MonoBehaviour
{
    private const string URL = "http://sii.uveg.edu.mx/unity/dropsV2/controllers/wsPairingCodeController.php";
    private const string API_KEY = "AJFFF-ASFFF-GWEGG-WEGERG-ERGEG-EGERG-ERGEG";//KEY falsa, remplazar por autentica

    [Serializable]
    public class Data {
        public string fechaInicio = "";
        public string fechaTermino = "";
        public string dispositivo = "";
        public string idCodigo = "";
        public string idUsuario = "";
        public string usuario = "";
        public string nombre = "";
        public string rol = "";
        public string gradoEstudios = "";
        public string programa = "";
        public string fechaRegistro = "";
        public string status = "";
        public string idC = "";
        public string codigo = "";
    }

    public static IEnumerator getDataSesionByCode(string idCodigo) {
        //Start the fading process
        WWWForm form = new WWWForm();
        //Debug.Log(idCodigo);
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;

        form.AddField("metodo", "getSesionByCode");
        form.AddField("idCodigo", idCodigo);
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
                if (text == "0") {
                    //Debug.Log("No se encontro el código");
                    pairingCode.status = text;
                    pairingCode.valCodigoSii = 0;
                } else {
                    //Debug.Log("Se encontro el código");
                    Data data = JsonUtility.FromJson<Data>(text);
                    pairingCode.fechaInicio = data.fechaInicio;
                    pairingCode.fechaTermino = data.fechaTermino;
                    pairingCode.dispositivo = data.dispositivo;
                    pairingCode.idCodigo = data.idCodigo;
                    pairingCode.idUsuario = data.idUsuario;
                    pairingCode.usuario = data.usuario;
                    pairingCode.nombre = data.nombre;
                    pairingCode.rol = data.rol;
                    pairingCode.gradoEstudios = data.gradoEstudios;
                    pairingCode.programa = data.programa;
                    pairingCode.fechaRegistro = data.fechaRegistro;
                    pairingCode.statusUsuario = data.status;
                    pairingCode.idC = data.idC;
                    pairingCode.codigo = data.codigo;
                    pairingCode.valCodigoSii = 1;

                    /*Debug.Log(data.fechaInicio);
                    Debug.Log(data.fechaTermino);
                    Debug.Log(data.dispositivo);
                    Debug.Log(data.idCodigo);
                    Debug.Log(data.idUsuario);
                    Debug.Log(data.usuario);
                    Debug.Log(data.nombre);
                    Debug.Log(data.rol);
                    Debug.Log(data.gradoEstudios);
                    Debug.Log(data.programa);
                    Debug.Log(data.fechaRegistro);
                    Debug.Log(data.idC);
                    Debug.Log(data.codigo);*/
                }
            }
        }
    }
}
