using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceLog : MonoBehaviour {


    /** 
     * Estructura que almacena los datos de un log
     */
    [Serializable]
    public class logData {
        public string id = "";
        public string fechaInicio = "";
        public string fechaTermino = "";
        public string dispositivo = "";
        public string idCodigo = "";
        public string idUsuario = "";
    }

    /** Función que inseta los datos del log
     * @param usuario matricula o correo del usuario
     */
    public static int insertarLogSqLite(string usuario) {
        string id = webServiceUsuario.consultarIdUsuarioSqLite(usuario);
        string query = "INSERT INTO log (fechaInicio, fechaTermino, dispositivo, syncroStatus, idCodigo, idUsuario) VALUES (dateTime(), '', '" + SystemInfo.deviceModel + "',0,0,'" + id + "');";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    /** Función que consulta el ultimo log de un usuario
     * @param usuario matricula o correo del usuario
     */
    public static string getLastLogSqLite(string idUsuario) {
        string query = "SELECT id FROM log WHERE idUsuario = " + idUsuario + " ORDER by fechaInicio DESC LIMIT 1;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            logData data = JsonUtility.FromJson<logData>(result);
            return data.id;
        } else {
            return "0";
        }
    }

    /** Función que inseta los datos del log
     * @param usuario matricula o correo del usuario
     */
    public static int cerrarLog(string usuario) {
        string id = webServiceUsuario.consultarIdUsuarioSqLite(usuario);
        var lastLog = getLastLogSqLite(id);
        string query = "UPDATE log SET fechaTermino = datetime() WHERE id = " + lastLog + ";";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

}
