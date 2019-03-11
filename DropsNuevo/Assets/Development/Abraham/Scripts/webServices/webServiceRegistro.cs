using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceRegistro : MonoBehaviour {

    /**
     * Estructura que almacena los datos de un registro
     */
    [Serializable]
    public class registroData {
        public string id = "";
        public string detalle = "";
        public string fechaRegistro = "";
        public string idAccion = "";
        public string idLog = "";
        public string idUsuario = "";
    }

    /** Función que inseta los datos del registro
     * @param detalle string con el detalle del registro
     * @param usuario matricula o correo del usuario
     */
    public static int insertarRegistroSqLite(string detalle, string usuario, int idAccion) {
        try {
            string id = webServiceUsuario.consultarIdUsuarioSqLite(usuario);
            string idLog = webServiceLog.getLastLogSqLite(id);
            string query = "INSERT INTO registros (detalle, fechaRegistro, syncroStatus, idAccion, idLog, idUsuario) VALUES ('" + detalle + "',dateTime(), '0', " + idAccion + ", " + idLog + ", " + id + " );";
            var result = conexionDB.alterGeneral(query);
            if (result == 1) {
                return 1;
            } else {
                return 0;
            }
        }
        catch (Exception ex) {
            Debug.LogError("Error en insertarRegistro: " + ex);
            return 0;
        }
    }

    public static void validarAccionSqlite(string descripcion, string usuario, string accion) {
        string idAccion = webServiceAcciones.consultarIdAccionSqLite(accion);
        if (idAccion != "0") {
            insertarRegistroSqLite(descripcion, usuario, Convert.ToInt32(idAccion));
        } else {
            insertarRegistroSqLite(descripcion + " *No había accion registrada", usuario, 0);
        }
    }

}
