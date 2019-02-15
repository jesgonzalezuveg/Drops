using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceLog : MonoBehaviour {

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
    public static int getLastLogSqLite(int idUsuario) {
        string query = "SELECT id FROM log WHERE idUsuario = " + idUsuario + " ORDER by fechaInicio DESC LIMIT 1;";
        var result = conexionDB.selectGeneral(query);
        result = result.Replace("{'id': '", "");
        result = result.Replace("'}", "");
        if (result != "0") {
            return Int32.Parse(result);
        } else {
            return 0;
        }
    }

    /** Función que inseta los datos del log
     * @param usuario matricula o correo del usuario
     */
    public static int cerrarLog(string usuario) {
        string id = webServiceUsuario.consultarIdUsuarioSqLite(usuario);
        var lastLog = getLastLogSqLite(Int32.Parse(id));
        string query = "UPDATE log SET fechaTermino = datetime() WHERE id = " + lastLog + ";";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

}
