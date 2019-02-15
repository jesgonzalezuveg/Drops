using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServicePaquetes : MonoBehaviour {

    public static string getPaquetes() {
        string query = "SELECT * FROM paquete;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            return result;
        } else {
            return "0";
        }
    }

}
