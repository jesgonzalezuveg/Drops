using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceRegistro : MonoBehaviour {

    /** Función que inseta los datos del registro
     * @param detalle string con el detalle del registro
     * @param usuario matricula o correo del usuario
     */
    public static int insertarRegistroSqLite(string detalle, string usuario, int idAccion) {
        string id = webServiceUsuario.consultarIdUsuarioSqLite(usuario);
        int idLog = webServiceLog.getLastLogSqLite(Int32.Parse(id));
        string query = "INSERT INTO registros (detalle, fechaRegistro, syncroStatus, idAccion, idLog, idUsuario) VALUES ('" + detalle + "',dateTime(), '0', " + idAccion + ", " + idLog + ", " + id + " );";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

}
