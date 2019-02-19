using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServicePaquetes : MonoBehaviour {

    /**
     * Estructura que almacena los datos de un paquete
     */
    [Serializable]
    public class paqueteData {
        public string id = "";
        public string descripcion = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
    }


    /**
     * Funcion que regresa los paquetes que existen en la base de datos local
     * 
     */
    public static paqueteData getPaquetes() {
        string query = "SELECT * FROM paquete;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            paqueteData paquete = JsonUtility.FromJson<paqueteData>(result);
            return paquete;
        } else {
            return null;
        }
    }

}
