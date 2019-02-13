using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class webServiceLogin : MonoBehaviour {
    //////////////
    private const string USUARIO_DATA = "http://siid.uveg.edu.mx/core/api/apiUsuarios.php";
    private const string API_KEY = "AJFFF-ASFFF-GWEGG-WEGERG-ERGEG-EGERG-ERGEG";//KEY falsa, remplazar por autentica
    public Text responseText;
    public GameObject manager;

    [Serializable]
    public class Data {
        public string Usuario = "";
        public string Nombre = "";
        public string PrimerApellido = "";
        public string SegundoApellido = "";
        public string Correo = "";
        public string CorreoPersonal = "";
        public string Rama = "";
        public string Genero = "";
        public string Domicilio = "";
        public string Colonia = "";
        public string TelefonoParticular = "";
        public string TelefonoCelular = "";
        public string EstadoCivil = "";
        public string NombrePerfil = "";
        public string NombrePerfilCompleto = "";
        public string CodigoPostal = "";
        public string CURP = "";
        public string DependientesEconomicos = "";
        public string PaisID = "";
        public string PaisClave = "";
        public string EstadoID = "";
        public string MunicipioID = "";
        public string Municipio = "";
        public string OcupacionID = "";
        public string Trabaja = "";
        public string LugarTrabajo = "";
        public string TelefonoTrabajo = "";
        public string CiudadTrabajo = "";
        public string Otro = "";
        public string GrupoEtnico = "";
        public string FechaNacimiento = "";
        public string FechaUltimaActualizacion = "";
        public string MesesUltimaActualizacion = "";
        public string Imagen = "";
        public string existeImagen = "";
        public string AvisoAppMovil = "";
        public string UrlAppMovil = "";
        public string Subsistema = "";
        public string TipoAcceso = "";
        public string Ava = "";
        public string Y = "";
        public string numeroIncidencias = "";
        public string usuarioCE = "";
        public string passCE = "";
        public string urlInscripcion = "";
        public string urlEgreso = "";
        public string urlEnrolamientoPreguntas = "";
        public string urlEnrolamientoFoto = "";
        public string urlEgreSegDoc = "";
        public string urlReqPasosGen = "";
        public string urlRecuperacionMaterias = "";
        public string opcB = "";
        public string opcI = "";
        public string usuarioBI = "";
        public string urlTBCV3Responsable = "";
        public string urlTBCV3Docente = "";
        public string NumeroMaterias = "";
    }

    [Serializable]
    public class JsonResponse {
        public Data data = new Data();
        public string mensaje = "";
        public string estatus = "";
        public string estatusCode = "";

    }

    public static IEnumerator getUserData(string usuario, string contraseña) {
        //Start the fading process
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;
        form.AddField("data", "{\"usuario\": " + usuario + ", \"contrasena\": " + contraseña + "}");
        //byte[] rawFormData = form.data;
        using (UnityWebRequest www = UnityWebRequest.Post(USUARIO_DATA, form)) {
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
                JsonResponse data = JsonUtility.FromJson<JsonResponse>(text);
                keyboardManager.sesion = data.estatusCode;
                appManager manager = GameObject.Find("AppManager").GetComponent<appManager>();
                manager.setNombre(data.data.Nombre);
                manager.setPApellido(data.data.PrimerApellido);
                manager.setSApellido(data.data.SegundoApellido);
                manager.setCorreo(data.data.Correo);
                manager.setImagen(data.data.Imagen);
                if (data.estatusCode == "001") {
                    SceneManager.LoadScene("template");
                } else {

                }
            }
        }
    }

    public static IEnumerator getUserData(string usuario) {
        //Start the fading process
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;
        form.AddField("data", "{\"usuario\": \"10002080\", \"contrasena\": \"12345\"}");
        //byte[] rawFormData = form.data;
        using (UnityWebRequest www = UnityWebRequest.Post(USUARIO_DATA, form)) {
            //www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string text;
                text = www.downloadHandler.text;
                text = text.Replace("[", "");
                text = text.Replace("]", "");
                JsonResponse data = JsonUtility.FromJson<JsonResponse>(text);
                keyboardManager.sesion = data.estatusCode;
                appManager manager = GameObject.Find("AppManager").GetComponent<appManager>();
                manager.setNombre(data.data.Nombre);
                manager.setPApellido(data.data.PrimerApellido);
                manager.setSApellido(data.data.SegundoApellido);
                manager.setCorreo(data.data.Correo);
                manager.setImagen(data.data.Imagen);
                SceneManager.LoadScene("template");
            }
        }
    }
    //////////////
}
