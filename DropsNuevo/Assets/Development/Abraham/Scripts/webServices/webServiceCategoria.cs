using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System;

public class webServiceCategoria : MonoBehaviour {

    [Serializable]
    public class Data {
        public string id = "";
        public string status = "";
    }


    public static string getIdCategoriaByName(string descripcion) {
        string query = "SELECT id FROM catalogoCatgoria WHERE descripcion = '" + descripcion + "' ;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            result = result.Replace("{'id': '", "");
            result = result.Replace("'}", "");
            return result;
        } else {
            return "0";
        }
    }

}
