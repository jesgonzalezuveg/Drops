using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.Text;

public class webServiceUsuario : MonoBehaviour {

    private const string USUARIO_DATA = "http://siid.uveg.edu.mx/core/api/apiUsuarios.php";     ///< URL del API que se utilizará
    private const string API_KEY = "AJFFF-ASFFF-GWEGG-WEGERG-ERGEG-EGERG-ERGEG";                ///< API_KEY KEY que se necesitará para la conexión

    /** Estructura que almacena los datos del usuario desde SII
     */
    [Serializable]
    public class Data {
        public string Usuario = "";
        public string Nombre = "";
        public string PrimerApellido = "";
        public string SegundoApellido = "";
        public string Correo = "";
        public string Imagen = "";
        public string ProgramaAcademico = "";
        public string ProgramaEstudio = "";
    }

    /**
     * Estructura que almacena los datos del usuario desde SqLite
     */
    [Serializable]
    public class userDataSqLite {
        public string id = "";
        public string usuario = "";
        public string nombre = "";
        public string rol = "";
        public string gradoEstudios = "";
        public string programa = "";
        public string fechaRegistro = "";
        public string status = "";
        public string syncroStatus = "";
    }

    [Serializable]
    public class usersAllDataSqLite {
        public userDataSqLite[] usuarios;
    }

    /** Estructura que almacena los datos del usuario y en caso de ser necesario los datos de inicio de sesión 
     */
    [Serializable]
    public class JsonResponse {
        public Data data = new Data();
        public string mensaje = "";
        public string estatus = "";
        public string estatusCode = "";

    }

    /** Función que inseta los datos del usuario en la base de datos local
     * @param usuario matricula o correo del usuario
     * @param nombre nombre del usuario
     * @param rol tipo de usuario puede ser usuarioUveg, invitado o invitadoFacebook
     * @param gradoEstudios puede ser nulo, en caso de ser alumno uveg insertará el nivel de estudios que tiene
     * @param programa puede ser nulo, en caso de ser alumno uveg insertará el programa al cual esta inscrito
     */
    public static int insertarUsuarioSqLite(string usuario, string nombre, string rol, string gradoEstudios, string programa) {
        string query = "INSERT INTO usuario (usuario, nombre, rol, gradoEstudios, programa, fechaRegistro, status, SyncroStatus) VALUES ('" + usuario + "','" + nombre + "','" + rol + "','" + gradoEstudios + "','" + programa + "', datetime(), 1, 0);";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    /** Función que inserta los datos del usuario en la base de datos local
     * @param usuario matricula o correo del usuario
     * @param nombre nombre del usuario
     * @param rol tipo de usuario puede ser usuarioUveg, invitado o invitadoFacebook
     * @param gradoEstudios puede ser nulo, en caso de ser alumno uveg insertará el nivel de estudios que tiene
     * @param programa puede ser nulo, en caso de ser alumno uveg insertará el programa al cual esta inscrito
     */
    public static int insertarUsuarioSqLite(string usuario, string nombre, string rol, string gradoEstudios, string programa, string fechaRegistro, int status) {
        string query = "INSERT INTO usuario (usuario, nombre, rol, gradoEstudios, programa, fechaRegistro, status, syncroStatus) VALUES ('" + usuario + "', '" + nombre + "', '" + rol + "', '" + gradoEstudios + "', '" + programa + "',  dateTime(), " + status + ", 2)";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    /** Función que consulta si es que el usuario que esta ingresado ya esta dado de alta
     * @param usuario matricula o correo electronico del usuario
     */
    public static string consultarUsuarioSqLite(string usuario) {
        string query = "SELECT * FROM usuario WHERE usuario = '" + usuario + "';";
        var result = conexionDB.selectGeneral(query);
        return result;
    }

    public static userDataSqLite[] consultarUsuariosSqLite() {
        string query = "SELECT * FROM usuario;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            byte[] bytes = Encoding.Default.GetBytes(result);
            result = Encoding.UTF8.GetString(bytes);
            result = "{\"usuarios\":[" + result + "]}";
            usersAllDataSqLite data = JsonUtility.FromJson<usersAllDataSqLite>(result);
            return data.usuarios;
        } else {
            return null;
        }
    }

    /** Función que consulta el id del usuario
     * @param usuario matricula o correo electronico del usuario
     */
    public static string consultarIdUsuarioSqLite(string usuario) {
        string query = "SELECT id FROM usuario WHERE usuario = '" + usuario + "';";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            userDataSqLite data = JsonUtility.FromJson<userDataSqLite>(result);
            return data.id;
        } else {
            return "0";
        }
    }

    /** Función para actualizar los datos del usuario
     * @param usuario matricula o correo electronico del usuario
     * @param nombre nombre completo del usuario
     * @param rol rol del usuario
     * @param gradoEstudios el grado de estudios del usuario
     * @param programa la carrera del usuario
     * @param status estado del usuario
     */
    public static int updateUserSqlite(string usuario, string nombre, string rol, string gradoEstudios, string programa, int status) {
        string query = "UPDATE usuario SET usuario = '" + usuario + "', nombre = '" + nombre + "', rol = '" + rol + "', gradoEstudios = '" + gradoEstudios + "', programa = '" + programa + "', status = " + status + " WHERE usuario = '" + usuario + "'";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    public static int updateSyncroStatusSqlite(string id, int sincroStatus) {
        string query = "UPDATE usuario SET syncroStatus = '" + sincroStatus + "' WHERE id = '" + id + "'";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    /** Función que verifica di el usuario existe
     * @param usuario matricula o correo electronico del usuario
     */
    public static int existUserSqlite(string usuario) {
        string query = "SELECT * FROM usuario WHERE usuario = '" + usuario + "'";
        var result = conexionDB.selectGeneral(query);

        if (result != "0") {
            return 1;
        } else {
            return 0;
        }
    }


    /** Coroutine que consulta base de datos de SII para obtener los datos del usuario
     * @param usuario matricula, correo institucional o correo personal del alumno que ingresa
     * @param contraseña, contraseña del usuario del cual quieres consultar datos, sirve para verificar el login
     */
    public static IEnumerator getUserData(string usuario, string contraseña) {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;
        form.AddField("data", "{\"usuario\": \"" + usuario + "\", \"contrasena\": \"" + contraseña + "\"}");
        using (UnityWebRequest www = UnityWebRequest.Post(USUARIO_DATA, form)) {
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
                text = text.Replace("[", "");
                text = text.Replace("]", "");
                JsonResponse data = JsonUtility.FromJson<JsonResponse>(text);
                if (data.data.Nombre != "") {
                    if (data.estatusCode == "001") {
                        string nombreCompleto = data.data.Nombre + " " + data.data.PrimerApellido + " " + data.data.SegundoApellido;
                        appManager manager = GameObject.Find("AppManager").GetComponent<appManager>();
                        manager.setUsuario(data.data.Usuario);
                        manager.setNombre(nombreCompleto);
                        manager.setCorreo(data.data.Correo);
                        manager.setImagen(data.data.Imagen);
                        var idLocal = consultarIdUsuarioSqLite(data.data.Usuario);
                        if (idLocal == "0") {
                            insertarUsuarioSqLite(data.data.Usuario, nombreCompleto, "usuarioUveg", data.data.ProgramaAcademico, data.data.ProgramaEstudio);
                        }
                        webServiceLog.insertarLogSqLite(data.data.Usuario);
                        webServiceRegistro.validarAccionSqlite("Login teclado", data.data.Usuario, "Login");
                        //webServiceRegistro.insertarRegistroSqLite("Login teclado", data.data.Usuario, 1);
                        SceneManager.LoadScene("menuCategorias");
                    } else {
                        //Aqui va mensaje de contraseña incorrecta
                        GameObject.FindObjectOfType<keyboardManager>().mensaje.text = "Contraseña incorrecta";
                        GameObject.FindObjectOfType<PlayerManager>().setMensaje(false, "");
                        Debug.Log("Contraseña incorrecta");

                    }
                } else {
                    //Aqui va mensaje de usuario incorrecto
                    GameObject.FindObjectOfType<keyboardManager>().mensaje.text = "El usuario no existe";
                    GameObject.FindObjectOfType<PlayerManager>().setMensaje(false, "");
                    Debug.Log("El usuario no existe");
                }
            }
        }
    }

    public static IEnumerator getUserData(string usuario) {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;
        form.AddField("data", "{\"usuario\": \"" + usuario + "\", \"contrasena\": \"" + usuario + "\"}");
        using (UnityWebRequest www = UnityWebRequest.Post(USUARIO_DATA, form)) {
            AsyncOperation asyncLoad = www.SendWebRequest();
            // Wait until fully loads
            while (!asyncLoad.isDone) {
                yield return null;
            }
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string text;
                text = www.downloadHandler.text;
                text = text.Replace("[", "");
                text = text.Replace("]", "");
                JsonResponse data = JsonUtility.FromJson<JsonResponse>(text);
                if (data.data.Usuario != "") {
                    string nombreCompleto = data.data.Nombre + " " + data.data.PrimerApellido + " " + data.data.SegundoApellido;
                    appManager manager = GameObject.Find("AppManager").GetComponent<appManager>();
                    manager.setUsuario(data.data.Usuario);
                    manager.setNombre(nombreCompleto);
                    manager.setCorreo(data.data.Correo);
                    manager.setImagen(data.data.Imagen);
                    webServiceRegistro.validarAccionSqlite("Login Pairing Code", data.data.Usuario, "Login");
                    //webServiceRegistro.insertarRegistroSqLite("Login Pairing Code", data.data.Usuario, 1);
                } else {
                    //Aqui va mensaje de usuario incorrecto
                    Debug.Log("El usuario no existe");
                }
            }
        }
    }

    /** Coroutine que consulta base de datos de SII para obtener los datos del usuario
     * @param usuario matricula, correo institucional o correo personal del alumno que ingresa
     * @param name nombre del usuario de facebook
     * @facebook bool que detecta si se inicio sesión con facebook
     */
    public static IEnumerator getUserData(string usuario, string name, string imagen) {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers["Authorization"] = API_KEY;
        form.AddField("data", "{\"usuario\":\"" + usuario + "\"}");
        using (UnityWebRequest www = UnityWebRequest.Post(USUARIO_DATA, form)) {
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
                text = text.Replace("[", "");
                text = text.Replace("]", "");
                JsonResponse data = JsonUtility.FromJson<JsonResponse>(text);
                appManager manager = GameObject.Find("AppManager").GetComponent<appManager>();
                if (data.data.Usuario != "") {
                    string nombreCompleto = data.data.Nombre + " " + data.data.PrimerApellido + " " + data.data.SegundoApellido;
                    manager.setUsuario(data.data.Usuario);
                    manager.setNombre(nombreCompleto);
                    manager.setCorreo(data.data.Correo);
                    manager.setImagen(data.data.Imagen);
                    if (consultarUsuarioSqLite(data.data.Usuario) != "0") {
                        webServiceLog.insertarLogSqLite(data.data.Usuario);
                        webServiceRegistro.insertarRegistroSqLite("Login Facebook", data.data.Usuario, 1);
                        SceneManager.LoadScene("menuCategorias");

                    } else {
                        if (insertarUsuarioSqLite(data.data.Usuario, nombreCompleto, "usuarioUveg", data.data.ProgramaAcademico, data.data.ProgramaEstudio) == 1) {
                            webServiceLog.insertarLogSqLite(data.data.Usuario);
                            //webServiceRegistro.insertarRegistroSqLite("Login Facebook", data.data.Usuario, 2);
                            webServiceRegistro.validarAccionSqlite("Login Facebook", data.data.Usuario, "Login");
                            SceneManager.LoadScene("menuCategorias");
                        } else {
                            Debug.Log("Fallo el insert");
                        }
                    }
                } else {
                    manager.setUsuario(usuario);
                    manager.setNombre(name);
                    manager.setCorreo(usuario);
                    manager.setImagen(imagen);
                    if (consultarUsuarioSqLite(usuario) != "0") {
                        webServiceLog.insertarLogSqLite(usuario);
                        webServiceRegistro.insertarRegistroSqLite("Login Facebook", usuario, 2);
                        SceneManager.LoadScene("menuCategorias");
                    } else {
                        if (insertarUsuarioSqLite(usuario, name, "usuarioFacebook", "", "") == 1) {
                            webServiceLog.insertarLogSqLite(data.data.Usuario);
                            webServiceRegistro.insertarRegistroSqLite("Login Facebook", data.data.Usuario, 2);
                            SceneManager.LoadScene("menuCategorias");
                        } else {
                            Debug.Log("Fallo el insert");
                        }
                    }
                }
            }
        }
    }

}
