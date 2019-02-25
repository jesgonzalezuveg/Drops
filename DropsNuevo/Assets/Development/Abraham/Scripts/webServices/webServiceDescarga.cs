using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceDescarga : MonoBehaviour {

    /**
     * Estructura que almacena los datos de categoria
     */
    [Serializable]
    public class descargaData {
        public string id = "";
        public string equipo = "";
        public string fechaDescarga = "";
        public string idPaquete = "";
        public string idUsuario = "";
    }

    [Serializable]
    public class Data {
        public descargaData[] categoria;
    }

    public static descargaData getDescargaByPaquete(string idPaquete) {
        string query = "SELECT * FROM descarga WHERE idPaquete = '" + idPaquete + "';";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            descargaData categoria = JsonUtility.FromJson<descargaData>(result);
            return categoria;
        } else {
            return null;
        }
    }

    public static int insertarDescargaSqLite(string idPaquete, string idUsuario) {
        string query = "INSERT INTO descarga (equipo, fechaDescarga, idPaquete, idUsuario) VALUES ('" + SystemInfo.deviceUniqueIdentifier + "', GETDATE(), '" + idPaquete +"', '" + idUsuario +"');";
        var result = conexionDB.alterGeneral(query);
        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }
}
