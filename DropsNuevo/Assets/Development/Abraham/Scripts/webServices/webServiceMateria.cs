using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceMateria : MonoBehaviour {


    /**
     * Estructura que almacena los datos de una materia
     */ 
    [Serializable]
    public class materiaData {
        public string id = "";
        public string claveMAteria = "";
        public string descripcion = "";
        public string status = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
        public string idCategoria = "";
    }

    /**
     * Funcion que regresa un string con los ids de las materias que pertenecen a determinada categoria
     * cada id de materia esta separado por una coma ejemplo 1,4,5,16
     * @param categoria, id de la categoria de la cual se desean consultar sus materias
     */
    public static string getIdMateriasByCategoria(string categoria) {
        string query = "SELECT id FROM catalogoMateria WHERE idCategoria = " + categoria + ";";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            Debug.Log(result);
            result = result.Replace("{\"id\": \"", "");
            result = result.Replace("\"}", "");
            Debug.Log(result);
            return result;
        } else {
            return "0";
        }
    }

}
