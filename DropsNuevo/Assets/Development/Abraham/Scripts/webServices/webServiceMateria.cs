using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServiceMateria : MonoBehaviour {

    public static string getMateriasByCategoria(int categoria) {
        string query = "SELECT id FROM catalogoMateria WHERE idCategoria = " + categoria + ";";
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
