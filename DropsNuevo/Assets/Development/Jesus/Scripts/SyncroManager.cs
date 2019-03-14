using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncroManager : MonoBehaviour
{
    appManager manager;
    public string jsonGeneral;
    webServiceUsuario.userDataSqLite dataUser = null;
    webServiceLog.logData[] logs = null;
    webServiceRegistro.registroData[] registros = null;
    webServiceIntento.intentoDataSqLite[] intentos = null;
    webServiceDetalleIntento.detalleIntentoDataSqLite[] detalleIntento = null;
    public static string respuestaWsSincro = "0";

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log(GameObject.Find("AppManager").GetComponent<appManager>().isFirstLogin);
        if (GameObject.Find("AppManager").GetComponent<appManager>().isFirstLogin) {
            Debug.Log("No sincronizar antes del primer login");
            return;
        }
        jsonGeneral = "";
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
        Sincronizacion();
        StartCoroutine(webServiceSincronizacion.SincroData(jsonGeneral));
        //sincronizacionLocal();
    }

    public void sincronizacionLocal() {
        if (respuestaWsSincro == "1") {
            int resultado = webServiceSincronizacion.changeSyncroStatus(jsonGeneral);
            if (resultado == 1) {
                Debug.Log("respuesta local: termino sincronizacion");
            } else {
                Debug.Log("respuesta local: error al sincronizar");
            }
        } else {
            Debug.Log("respuesta =  0");
            //sincronizacionLocal();
        }
    }

    public void Sincronizacion() {
        //Obtenemos los datos del usuario
        string user = manager.getUsuario();
        if (getDataUser(user)) {
            //Comenzamos a generar el json con los datos del usuario
            jsonGeneral += "{\"Usuarios\":[";
            jsonGeneral += "{\"id\": \"" + validateData(dataUser.id) + "\",";
            jsonGeneral += "\"usuario\": \"" + validateData(dataUser.usuario) + "\",";
            jsonGeneral += "\"nombre\": \"" + validateData(dataUser.nombre) + "\",";
            jsonGeneral += "\"rol\": \"" + validateData(dataUser.rol) + "\",";
            jsonGeneral += "\"gradoEstudios\": \"" + validateData(dataUser.gradoEstudios) + "\",";
            jsonGeneral += "\"programa\": \"" + validateData(dataUser.programa) + "\",";
            jsonGeneral += "\"fechaRegistro\": \"" + validateData(dataUser.fechaRegistro) + "\",";
            jsonGeneral += "\"status\": \"" + validateData(dataUser.status) + "\",";
            //Obtenemos los logs del usuario
            getLogsUser();
            jsonGeneral += "}]}";
            Debug.Log(jsonGeneral);
            //Comenzar sincronizacion con el SII
        }
    }

    public bool getDataUser(string user) {
        string data = webServiceUsuario.consultarUsuarioSqLite(user);
        if (data != "0") {
            dataUser = JsonUtility.FromJson<webServiceUsuario.userDataSqLite>(data);
            return true;
        } else {
            Debug.Log("Error de sincronización al momento de buscar los datos del usuario");
            return false;
        }
    }

    public void getLogsUser() {
        //Obtenemos logs del usuario
        logs = webServiceLog.getLogsByUser(dataUser.id);
        if (logs!=null){
            //Continuamos generando el json agregando los logs del usuario
            jsonGeneral += "\"logs\":[";
            for (var i = 0; i < logs.Length; i++) {
                jsonGeneral += "{\"id\": \""+ validateData(logs[i].id) + "\",";
                jsonGeneral += "\"fechaInicio\": \""+ validateData(logs[i].fechaInicio) + "\",";
                jsonGeneral += "\"fechaTermino\": \"" + validateData(logs[i].fechaTermino) + "\",";
                jsonGeneral += "\"dispositivo\": \"" + validateData(logs[i].dispositivo) + "\",";
                jsonGeneral += "\"idCodigo\": \"" + validateData(logs[i].idCodigo) + "\",";
                jsonGeneral += "\"idUsuario\": \"" + validateData(dataUser.usuario) + "\",";
                //Obtenemos los registros pertenecientes al log en turno
                getRegistrosUser(logs[i].id);
                //Obtenemos los intentos pertenecientes al log en turno
                getIntentosUser(logs[i].id);
                if ((logs.Length-i)!=1) {
                    jsonGeneral += "},";
                } else {
                    jsonGeneral += "}";
                }
            }
            jsonGeneral += "]";
        } else {
            //jsonGeneral = "";
            jsonGeneral += "\"logs\":[]";
            Debug.Log("No se encontraron logs");
        }
    }

    public void getRegistrosUser(string idLog) {
        //Obtenemos los registros del log
        registros = webServiceRegistro.getRegistrossByLog(idLog);
        if (registros!=null) {
            //Continuamos generando el json agregando los registros del log
            jsonGeneral += "\"registros\":[";
            for (var i = 0; i < registros.Length; i++) {
                //Obtenemos la descripcion de la accion para ponerla en lugar del id
                string descripcionAccion = webServiceAcciones.consultarDescripcionAccionSqLite(registros[i].idAccion);
                jsonGeneral += "{\"id\": \"" + validateData(registros[i].id) + "\",";
                jsonGeneral += "\"detalle\": \"" + validateData(registros[i].detalle) + "\",";
                jsonGeneral += "\"fechaRegistro\": \"" + validateData(registros[i].fechaRegistro) + "\",";
                jsonGeneral += "\"idLog\": \"" + validateData(registros[i].idLog) + "\",";
                jsonGeneral += "\"idUsuario\": \"" + validateData(dataUser.usuario) + "\",";
                if (descripcionAccion != "0") {
                    jsonGeneral += "\"idAccion\": \"" + validateData(descripcionAccion) + "\"";
                } else {
                    jsonGeneral += "\"idAccion\": \"" + validateData(descripcionAccion) + "\"";
                    Debug.Log("No se encontro la accion con id: "+registros[i].idAccion);
                }
                if ((registros.Length - i) != 1) {
                    jsonGeneral += "},";
                } else {
                    jsonGeneral += "}";
                }
            }
            jsonGeneral += "],";
        } else {
            jsonGeneral += "\"registros\":[],";
            Debug.Log("No se encontraron registros");
        }
    }

    public void getIntentosUser(string idLog) {
        //Obtenemos los intentos del log
        intentos = webServiceIntento.getIntentosByLog(idLog);
        if (intentos != null) {
            //Continuamos generando el json agregando los intentos del log
            jsonGeneral += "\"intentos\":[";
            for (var i = 0; i < intentos.Length; i++) {
                jsonGeneral += "{\"id\": \"" + validateData(intentos[i].id) + "\",";
                jsonGeneral += "\"puntaje\": \"" + validateData(intentos[i].puntaje) + "\",";
                jsonGeneral += "\"fechaRegistro\": \"" + validateData(intentos[i].fechaRegistro) + "\",";
                jsonGeneral += "\"fechaModificacion\": \"" + validateData(intentos[i].fechaModificacion) + "\",";
                jsonGeneral += "\"idLog\": \"" + validateData(intentos[i].idLog) + "\",";
                //Obtenemos el detalle del intento
                getDetalleIntentoUser(intentos[i].id);
                if ((intentos.Length - i) != 1) {
                    jsonGeneral += "},";
                } else {
                    jsonGeneral += "}";
                }
            }
            jsonGeneral += "]";
        } else {
            jsonGeneral += "\"intentos\":[]";
            Debug.Log("No se encontraron intentos");
        }
    }

    public void getDetalleIntentoUser(string idIntento) {
        //Obtenemos el detalle del intento
        detalleIntento = webServiceDetalleIntento.getDetalleIntentosByIntento(idIntento);
        if (detalleIntento != null) { 
            //Continuamos generando el json agregando el detalle del intento al intento correspondiente
            jsonGeneral += "\"detalleIntento\":[";
            for (var i = 0; i < detalleIntento.Length; i++) {
                jsonGeneral += "{\"id\": \"" + validateData(detalleIntento[i].id) + "\",";
                jsonGeneral += "\"correcto\": \"" + validateData(detalleIntento[i].correcto) + "\",";

                string idServerPregunta = webServicePreguntas.consultarIdServerPreguntaSqLiteById(detalleIntento[i].idPregunta);
                string idServerRespuesta = webServiceRespuestas.consultarIdServerRespuestaSqLiteById(detalleIntento[i].idRespuesta);
                if (idServerPregunta != "0") {
                    jsonGeneral += "\"idPregunta\": \"" + idServerPregunta + "\",";
                } else {
                    jsonGeneral += "\"idPregunta\": \""+ detalleIntento[i].idPregunta + "\",";
                    Debug.Log("No se encontro la pregunta con id: " + detalleIntento[i].idPregunta);
                }

                if (idServerRespuesta != "0") {
                    jsonGeneral += "\"idRespuesta\": \"" + idServerRespuesta + "\",";
                } else {
                    jsonGeneral += "\"idRespuesta\": \"" + detalleIntento[i].idRespuesta + "\",";
                    Debug.Log("No se encontro la respuesta con id: " + detalleIntento[i].idRespuesta);
                }

                jsonGeneral += "\"idIntento\": \"" + validateData(detalleIntento[i].idIntento) + "\"";
                if ((detalleIntento.Length - i) != 1) {
                    jsonGeneral += "},";
                } else {
                    jsonGeneral += "}";
                }
            }
            jsonGeneral += "]";
        } else {
            jsonGeneral += "\"detalleIntento\":[]";
            Debug.Log("No se encontro el detalle del intento");
        }
    }

    public string validateData(string data) {
        if (data!=null && data !="") {
            return data;
        } else {
            return "";
        }
    }
}
