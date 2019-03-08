using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceDetalleIntento : MonoBehaviour
{
    /** Estructura que almacena los datos de las acciones desde SII
     */
    [Serializable]
    public class Data {
        public string id = "";
        public string correcto = "";
        public string idPregunta = "";
        public string idRespuesta = "";
        public string idIntento = "";
    }

    /**
     * Estructura que almacena los datos de las acciones desde SqLite
     */
    [Serializable]
    public class detalleIntentoDataSqLite {
        public string id = "";
        public string correcto = "";
        public string syncroStatus = "";
        public string idPregunta = "";
        public string idRespuesta = "";
        public string idIntento = "";
    }

    /** Función que inseta los datos de la accion en la base de datos local
     * @param descripcion descripcion de la accion 
     * @param status estado de la accion (activa o inanctiva)
     */
    public static int insertarDetalleIntentoSqLite(string correcto, string idPregunta, string idRespuesta, string idIntento) {
        string query = "INSERT INTO detalleIntento (correcto, syncroStatus, idPregunta, idRespuesta, idIntento) VALUES (" + correcto + ", 0, " + idPregunta + ", " + idRespuesta + ", " + idIntento + ");";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    /** Función que consulta y trae los datos de la accion solicitada
     * @param accion descripcion de la accion a consultar
     */
    public static string consultarDetalleIntentoByIdSqLite(string id) {
        string query = "SELECT * FROM detalleIntento WHERE id = " + id + ";";
        var result = conexionDB.selectGeneral(query);
        return result;
    }

    /** Función que consulta y trae los datos de la accion solicitada
     * @param accion descripcion de la accion a consultar
     */
    public static string consultarDetalleIntentoByIdIntentoSqLite(string id) {
        string query = "SELECT * FROM detalleIntento WHERE idIntento = " + id + ";";
        var result = conexionDB.selectGeneral(query);
        return result;
    }

    /** Función para actualizar los datos de una accion
     * @param accion descripcion de la accion 
     * @param status estado de la accion (activa o inanctiva)
     * @param idServer id de la accion en el servidor
     */
    public static int updateDetalleIntentoSqlite(string id, string correcto, string idPregunta, string idRespuesta) {
        string query = "UPDATE detalleIntento SET correcto = " + correcto + ", idPregunta =  "+ idPregunta + ", idRespuesta =  " + idRespuesta + " WHERE id = " + id + "";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    /** Función para actualizar los datos de una accion
     * @param accion descripcion de la accion 
     * @param status estado de la accion (activa o inanctiva)
     * @param idServer id de la accion en el servidor
     */
    public static int updateSyncroStarusIntentoSqlite(string id, int status) {
        string query = "UPDATE detalleIntento SET syncroStatus = " + status + " WHERE id = " + id + "";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    /** Función que verifica si la accion existe
     * @param usuario matricula o correo electronico del usuario
     */
    public static int existDetalleIntentoSqlite(string id) {
        string query = "SELECT * FROM detalleIntento WHERE id = " + id + "";
        var result = conexionDB.selectGeneral(query);

        if (result != "0") {
            return 1;
        } else {
            return 0;
        }
    }
}
