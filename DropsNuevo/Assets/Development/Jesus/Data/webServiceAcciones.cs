using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
public class webServiceAcciones : MonoBehaviour
{
    /** Estructura que almacena los datos de las acciones desde SII
     */
    [Serializable]
    public class Data {
        public string descripcion = "";
        public string status = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
    }

    /**
     * Estructura que almacena los datos de las acciones desde SqLite
     */
    [Serializable]
    public class accionDataSqLite {
        public string id = "";
        public string descripcion = "";
        public string status = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
    }

    /** Función que inseta los datos de la accion en la base de datos local
     * @param descripcion descripcion de la accion 
     * @param status estado de la accion (activa o inanctiva)
     */
    public static int insertarAccionSqLite(string descripcion, string status) {
        string query = "INSERT INTO catalogoAcciones (descripcion, status, fechaRegistro, fechaModificacion) VALUES ('" + descripcion + "'," + status + ", datetime(), datetime());";
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
    public static string consultarAccionSqLite(string accion) {
        string query = "SELECT * FROM catalogoAcciones WHERE descripcion = '" + accion + "';";
        var result = conexionDB.selectGeneral(query);
        return result;
    }

    /** Función que consulta el id de la accion
     * @param accion descripcion de la accion a consultar
     */
    public static string consultarIdAccionSqLite(string accion) {
        string query = "SELECT id FROM catalogoAcciones WHERE descripcion = '" + accion + "';";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            accionDataSqLite data = JsonUtility.FromJson<accionDataSqLite>(result);
            return data.id;
        } else {
            return "0";
        }
    }

    /** Función para actualizar los datos de una accion
     * @param accion descripcion de la accion 
     * @param status estado de la accion (activa o inanctiva)
     * @param idServer id de la accion en el servidor
     */
    public static int updateAccionSqlite(string accion, string status, string idServer) {
        string query = "UPDATE catalogoAcciones SET descripcion = '" + accion + "', status = " + status + ", fechaModificacion =  datetime() WHERE idServer = " + idServer + "";
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
    public static int existAccionSqlite(string accion) {
        string query = "SELECT * FROM catalogoAcciones WHERE descripcion = '" + accion + "'";
        var result = conexionDB.selectGeneral(query);

        if (result != "0") {
            return 1;
        } else {
            return 0;
        }
    }
}
