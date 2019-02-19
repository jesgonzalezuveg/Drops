using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class webServicePreguntas : MonoBehaviour {

    /**
     * Estructura que almacena los datos de una pregunta
     */
    [Serializable]
    public class preguntaData {
        public string id = "";
        public string descripcion = "";
        public string status = "";
        public string fechaRegistro = "";
        public string fechaModificacion = "";
        public string idTipoEjercicio = "";
        public string idMateria = "";
        public string idPaquete = "";
    }

    /**
     * Función que regresa la estructura preguntaData
     * la cual almacena los datos de las preguntas relacionadas a la materia
     * @param materia, id de la materia de la cual se requieren las preguntas
     */
    public static preguntaData getPreguntasByMateria(string materia) {
        string query = "SELECT id, descripcion, idTipoEjercicio FROM pregunta WHERE idMateria = " + materia + " AND status = 1;";
        var result = conexionDB.selectGeneral(query);
        if (result != "0") {
            preguntaData data = JsonUtility.FromJson<preguntaData>(result);
            return data;
        } else {
            return null;
        }
    }
}
