using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServicePreguntas : MonoBehaviour {

    public static string getPreguntasByMateria(string materia) {
        string query = "SELECT id, descripcion, idTipoEjercicio FROM pregunta WHERE idMateria = " + materia + " AND status = 1;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            return result;
        } else {
            return "0";
        }
    }
}
