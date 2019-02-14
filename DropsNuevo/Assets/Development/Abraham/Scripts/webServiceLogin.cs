using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceLogin : MonoBehaviour {

    private const string USUARIO_DATA = "http://siid.uveg.edu.mx/core/api/apiUsuarios.php";     ///< URL del API que se utilizará
    private const string API_KEY = "AJFFF-ASFFF-GWEGG-WEGERG-ERGEG-EGERG-ERGEG";                ///< API KEY que se necesitará para la conexión

    /**
     * Estructura que almacena los datos del usuario
     */
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
        public string ProgramaAcademico = "";
        public string ProgramaEstudio = "";
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


    /**
     * Estructura que almacena los datos del usuario y en caso de ser necesario los datos de inicio de sesión 
     */
    [Serializable]
    public class JsonResponse {
        public Data data = new Data();
        public string mensaje = "";
        public string estatus = "";
        public string estatusCode = "";

    }

    /**
     * Coroutine que consulta base de datos de SII para obtener los datos del usuario
     * @param usuario matricula, correo institucional o correo personal del alumno que ingresa
     * @param contraseña, contraseña del usuario del cual quieres consultar datos, sirve para verificar el login
     */
    public static IEnumerator getUserData(string usuario, string contraseña) {
        print("usuario: " + usuario + "\nContraseña: " + contraseña);
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;
        form.AddField("data", "{\"usuario\": \"" + usuario + "\", \"contrasena\": \"" + contraseña + "\"}");
        using (UnityWebRequest www = UnityWebRequest.Post(USUARIO_DATA, form)) {
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
                if (data.data.Usuario != "") {
                    if (data.estatusCode == "001") {
                        string nombreCompleto = data.data.Nombre + " " + data.data.PrimerApellido + " " + data.data.SegundoApellido;
                        appManager manager = GameObject.Find("AppManager").GetComponent<appManager>();
                        manager.setNombre(nombreCompleto);
                        manager.setCorreo(data.data.Correo);
                        manager.setImagen(data.data.Imagen);
                        if (consultarUsuarioSqLite(data.data.Correo) == 1) {
                            SceneManager.LoadScene("template");
                        } else {
                            if (insertarUsuarioSqLite(data.data.Usuario, nombreCompleto, "usuarioUveg", data.data.ProgramaAcademico, data.data.ProgramaEstudio) == 1) {
                                SceneManager.LoadScene("template");
                            } else {
                                Debug.Log("Fallo el insert");
                            }
                        }
                    } else {
                        Debug.Log("Contraseña incorrecta");
                    }
                } else {
                    Debug.Log("El usuario no existe");
                }
            }
        }
    }

    /**
     * Coroutine que consulta base de datos de SII para obtener los datos del usuario
     * @param usuario matricula, correo institucional o correo personal del alumno que ingresa
     * @param name nombre del usuario de facebook
     * @facebook bool que detecta si se inicio sesión con facebook
     */
    public static IEnumerator getUserData(string usuario, string name, string imagen) {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;
        form.AddField("data", "{\"usuario\":\""+usuario+"\"}");
        using (UnityWebRequest www = UnityWebRequest.Post(USUARIO_DATA, form)) {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string text;
                text = www.downloadHandler.text;
                text = text.Replace("[", "");
                text = text.Replace("]", "");
                JsonResponse data = JsonUtility.FromJson<JsonResponse>(text);
                appManager manager = GameObject.Find("AppManager").GetComponent<appManager>();
                if (data.data.Usuario != "") {
                    string nombreCompleto = data.data.Nombre + " " + data.data.PrimerApellido + " " + data.data.SegundoApellido;
                    manager.setNombre(nombreCompleto);
                    manager.setCorreo(data.data.Correo);
                    manager.setImagen(data.data.Imagen);
                    if (consultarUsuarioSqLite(data.data.Correo) == 1) {
                        SceneManager.LoadScene("template");
                    } else {
                        if (insertarUsuarioSqLite(data.data.Usuario, nombreCompleto, "usuarioUveg", data.data.ProgramaAcademico, data.data.ProgramaEstudio) == 1) {
                            SceneManager.LoadScene("template");
                        } else {
                            Debug.Log("Fallo el insert");
                        }
                    }
                } else {
                    if (insertarUsuarioSqLite(usuario, name, "invitadoFacebook", "", "") == 1) {
                        manager.setNombre(name);
                        manager.setCorreo(usuario);
                        manager.setImagen(imagen);
                        SceneManager.LoadScene("template");
                    } else {
                        Debug.Log("Fallo el insert");
                    }
                }
            }
        }
    }

    /**
     * Función que inseta los datos del usuario en la base de datos local
     * @param usuario matricula o correo del usuario
     * @param nombre nombre del usuario
     * @param rol tipo de usuario puede ser usuarioUveg, invitado o invitadoFacebook
     * @param gradoEstudios puede ser nulo, en caso de ser alumno uveg insertará el nivel de estudios que tiene
     * @param programa puede ser nulo, en caso de ser alumno uveg insertará el programa al cual esta inscrito
     */
    public static int insertarUsuarioSqLite(string usuario, string nombre, string rol, string gradoEstudios, string programa) {
        string query = "INSERT INTO usuario (usuario, nombre, rol, gradoEstudios, programa, fechaRegistro, status, SyncroStatus) VALUES ('"+ usuario +"','"+ nombre +"','"+ rol +"','"+ gradoEstudios +"','"+ programa + "', datetime(), 1, 1);";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    /**
     * Función que consulta si es que el usuario que esta ingresado ya esta dado de alta
     * @param usuario matricula o correo electronico del usuario
     */ 
    public static int consultarUsuarioSqLite(string usuario) {
        string query = "SELECT * FROM usuario WHERE usuario = '" + usuario + "';";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }
}
