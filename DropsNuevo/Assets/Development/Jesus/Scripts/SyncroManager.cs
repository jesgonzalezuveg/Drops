﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncroManager : MonoBehaviour
{
    appManager manager;
    public string jsonGeneral;
    public string jsonPerUser;
    webServiceUsuario.userDataSqLite dataUser = null;
    webServiceUsuario.userDataSqLite[] dataUsers = null;
    webServiceLog.logData[] logs = null;
    webServiceRegistro.registroData[] registros = null;
    webServiceIntento.intentoDataSqLite[] intentos = null;
    webServiceDetalleIntento.detalleIntentoDataSqLite[] detalleIntento = null;
    public static string respuestaWsSincro = "0";
    string usuarioActual;
    string idUsuarioActual;

    // Start is called before the first frame update
    void Awake()
    {
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
        if (manager.isFirstLogin == true && manager.isOnline == true) {
            string user = manager.getUsuario();
            if (getDataUser(user)) {
                manager.lastIdLog = webServiceLog.getLastLogSqLite(dataUser.id);
            }
            Debug.Log(manager.lastIdLog);
            Debug.Log("No sincronizar antes del primer login");
            sincronizacionUsuarios();
            validarJson(jsonGeneral);
            return;
        }
    }

    public void validarJson(string json) {
        if (json!=null && json != "{\"Usuarios\":[]}") {
            Debug.Log("Json generado " + json);
            StartCoroutine(webServiceSincronizacion.SincroData(json));
        } else {
            Debug.Log("El json no continen ningun dato");
        }
    }

    //public void sincronizacionUsuarioActual() {
    //    //Obtenemos los datos del usuario
    //    string user = manager.getUsuario();
    //    if (getDataUser(user)) {
    //        //Comenzamos a generar el json con los datos del usuario
    //        usuarioActual = dataUser.usuario;
    //        idUsuarioActual = dataUser.id;
    //        jsonGeneral += "{\"Usuarios\":[";
    //        jsonGeneral += "{\"id\": \"" + validateData(dataUser.id) + "\",";
    //        jsonGeneral += "\"usuario\": \"" + validateData(dataUser.usuario) + "\",";
    //        jsonGeneral += "\"nombre\": \"" + validateData(dataUser.nombre) + "\",";
    //        jsonGeneral += "\"rol\": \"" + validateData(dataUser.rol) + "\",";
    //        jsonGeneral += "\"gradoEstudios\": \"" + validateData(dataUser.gradoEstudios) + "\",";
    //        jsonGeneral += "\"programa\": \"" + validateData(dataUser.programa) + "\",";
    //        jsonGeneral += "\"fechaRegistro\": \"" + validateData(dataUser.fechaRegistro) + "\",";
    //        jsonGeneral += "\"status\": \"" + validateData(dataUser.status) + "\",";
    //        //Obtenemos los logs del usuario
    //        getLogsUser();
    //        jsonGeneral += jsonPerUser + "}]}";
    //        Debug.Log(jsonGeneral);
    //        //Comenzar sincronizacion con el SII
    //    }
    //}

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

    public void sincronizacionUsuarios() {
        //string user = manager.getUsuario();
        dataUsers = webServiceUsuario.consultarUsuariosSqLite();
        if (dataUsers != null) {
            //Continuamos generando el json agregando los logs del usuario
            jsonGeneral += "{\"Usuarios\":[";
            for (var i = 0; i < dataUsers.Length; i++) {
                usuarioActual = dataUsers[i].usuario;
                idUsuarioActual = dataUsers[i].id;
                jsonPerUser += "{\"id\": \"" + validateData(dataUsers[i].id) + "\",";
                jsonPerUser += "\"usuario\": \"" + validateData(dataUsers[i].usuario) + "\",";
                jsonPerUser += "\"nombre\": \"" + validateData(dataUsers[i].nombre) + "\",";
                jsonPerUser += "\"rol\": \"" + validateData(dataUsers[i].rol) + "\",";
                jsonPerUser += "\"gradoEstudios\": \"" + validateData(dataUsers[i].gradoEstudios) + "\",";
                jsonPerUser += "\"programa\": \"" + validateData(dataUsers[i].programa) + "\",";
                jsonPerUser += "\"fechaRegistro\": \"" + validateData(dataUsers[i].fechaRegistro) + "\",";
                jsonPerUser += "\"status\": \"" + validateData(dataUsers[i].status) + "\",";
                //Obtenemos los logs pertenecientes al usuario en turno
                getLogsUser();
                if (jsonPerUser != null) {
                    if ((dataUsers.Length - i) != 1) {
                        jsonPerUser += "},";
                    } else {
                        jsonPerUser += "}";
                    }
                }
            }

            jsonGeneral += jsonPerUser + "]}";
        } else {
            jsonGeneral = null;
            Debug.Log("No se encontraron usuarios");
        }
    }

    public void getLogsUser() {
        //Obtenemos logs del usuario
        logs = webServiceLog.getLogsByUser(idUsuarioActual, manager.lastIdLog);
        if (logs!=null){
            //Continuamos generando el json agregando los logs del usuario
            jsonPerUser += "\"logs\":[";
            for (var i = 0; i < logs.Length; i++) {
                jsonPerUser += "{\"id\": \""+ validateData(logs[i].id) + "\",";
                jsonPerUser += "\"fechaInicio\": \""+ validateData(logs[i].fechaInicio) + "\",";
                jsonPerUser += "\"fechaTermino\": \"" + validateData(logs[i].fechaTermino) + "\",";
                jsonPerUser += "\"dispositivo\": \"" + validateData(logs[i].dispositivo) + "\",";
                jsonPerUser += "\"idCodigo\": \"" + validateData(logs[i].idCodigo) + "\",";
                jsonPerUser += "\"idUsuario\": \"" + validateData(idUsuarioActual) + "\",";
                //Obtenemos los registros pertenecientes al log en turno
                getRegistrosUser(logs[i].id);
                //Obtenemos los intentos pertenecientes al log en turno
                getIntentosUser(logs[i].id);
                if ((logs.Length-i)!=1) {
                    jsonPerUser += "},";
                } else {
                    jsonPerUser += "}";
                }
            }
            jsonPerUser += "]";
        } else {
            jsonPerUser = null;
            Debug.Log("No se encontraron logs");
        }
    }

    public void getRegistrosUser(string idLog) {
        //Obtenemos los registros del log
        registros = webServiceRegistro.getRegistrossByLog(idLog);
        if (registros!=null) {
            //Continuamos generando el json agregando los registros del log
            jsonPerUser += "\"registros\":[";
            for (var i = 0; i < registros.Length; i++) {
                //Obtenemos la descripcion de la accion para ponerla en lugar del id
                string descripcionAccion = webServiceAcciones.consultarDescripcionAccionSqLite(registros[i].idAccion);
                jsonPerUser += "{\"id\": \"" + validateData(registros[i].id) + "\",";
                jsonPerUser += "\"detalle\": \"" + validateData(registros[i].detalle) + "\",";
                jsonPerUser += "\"fechaRegistro\": \"" + validateData(registros[i].fechaRegistro) + "\",";
                jsonPerUser += "\"idLog\": \"" + validateData(registros[i].idLog) + "\",";
                jsonPerUser += "\"idUsuario\": \"" + validateData(usuarioActual) + "\",";
                if (descripcionAccion != "0") {
                    jsonPerUser += "\"idAccion\": \"" + validateData(descripcionAccion) + "\"";
                } else {
                    jsonPerUser += "\"idAccion\": \"" + validateData(descripcionAccion) + "\"";
                    Debug.Log("No se encontro la accion con id: "+registros[i].idAccion);
                }
                if ((registros.Length - i) != 1) {
                    jsonPerUser += "},";
                } else {
                    jsonPerUser += "}";
                }
            }
            jsonPerUser += "],";
        } else {
            jsonPerUser += "\"registros\":[],";
            Debug.Log("No se encontraron registros");
        }
    }

    public void getIntentosUser(string idLog) {
        //Obtenemos los intentos del log
        intentos = webServiceIntento.getIntentosByLog(idLog);
        if (intentos != null) {
            //Continuamos generando el json agregando los intentos del log
            jsonPerUser += "\"intentos\":[";
            for (var i = 0; i < intentos.Length; i++) {
                jsonPerUser += "{\"id\": \"" + validateData(intentos[i].id) + "\",";
                jsonPerUser += "\"puntaje\": \"" + validateData(intentos[i].puntaje) + "\",";
                jsonPerUser += "\"fechaRegistro\": \"" + validateData(intentos[i].fechaRegistro) + "\",";
                jsonPerUser += "\"fechaModificacion\": \"" + validateData(intentos[i].fechaModificacion) + "\",";
                jsonPerUser += "\"idLog\": \"" + validateData(intentos[i].idLog) + "\",";
                //Obtenemos el detalle del intento
                getDetalleIntentoUser(intentos[i].id);
                if ((intentos.Length - i) != 1) {
                    jsonPerUser += "},";
                } else {
                    jsonPerUser += "}";
                }
            }
            jsonPerUser += "]";
        } else {
            jsonPerUser += "\"intentos\":[]";
            Debug.Log("No se encontraron intentos");
        }
    }

    public void getDetalleIntentoUser(string idIntento) {
        //Obtenemos el detalle del intento
        detalleIntento = webServiceDetalleIntento.getDetalleIntentosByIntento(idIntento);
        if (detalleIntento != null) {
            //Continuamos generando el json agregando el detalle del intento al intento correspondiente
            jsonPerUser += "\"detalleIntento\":[";
            for (var i = 0; i < detalleIntento.Length; i++) {
                jsonPerUser += "{\"id\": \"" + validateData(detalleIntento[i].id) + "\",";
                jsonPerUser += "\"correcto\": \"" + validateData(detalleIntento[i].correcto) + "\",";

                string idServerPregunta = webServicePreguntas.consultarIdServerPreguntaSqLiteById(detalleIntento[i].idPregunta);
                string idServerRespuesta = webServiceRespuestas.consultarIdServerRespuestaSqLiteById(detalleIntento[i].idRespuesta);
                if (idServerPregunta != "0") {
                    jsonPerUser += "\"idPregunta\": \"" + idServerPregunta + "\",";
                } else {
                    jsonPerUser += "\"idPregunta\": \""+ detalleIntento[i].idPregunta + "\",";
                    Debug.Log("No se encontro la pregunta con id: " + detalleIntento[i].idPregunta);
                }

                if (idServerRespuesta != "0") {
                    jsonPerUser += "\"idRespuesta\": \"" + idServerRespuesta + "\",";
                } else {
                    jsonPerUser += "\"idRespuesta\": \"" + detalleIntento[i].idRespuesta + "\",";
                    Debug.Log("No se encontro la respuesta con id: " + detalleIntento[i].idRespuesta);
                }

                jsonPerUser += "\"idIntento\": \"" + validateData(detalleIntento[i].idIntento) + "\"";
                if ((detalleIntento.Length - i) != 1) {
                    jsonPerUser += "},";
                } else {
                    jsonPerUser += "}";
                }
            }
            jsonPerUser += "]";
        } else {
            jsonPerUser += "\"detalleIntento\":[]";
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
