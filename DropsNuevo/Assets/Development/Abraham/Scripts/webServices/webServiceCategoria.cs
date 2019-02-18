using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System;

public class webServiceCategoria : MonoBehaviour {


    /**
     * Estructura que almacena los datos de categoria
     */ 
    [Serializable]
    public class categoriaData {
        public string id = "";
        public string descripcion = "";
        public string status = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
    }

    /**
     * Función que regresa el id de la categoria que se pide
     * @param descripcion nombre de la categoria que se esta solicitando el ID
     */ 
    public static string getIdCategoriaByName(string descripcion) {
        string query = "SELECT id FROM catalogoCatgoria WHERE descripcion = '" + descripcion + "' ;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            categoriaData data = JsonUtility.FromJson<categoriaData>(result);
            return data.id;
        } else {
            return "0";
        }
    }

}
