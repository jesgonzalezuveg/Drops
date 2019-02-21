using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class testMaterias : MonoBehaviour {

    GameObject manager;
    public Image imagen;

    private void Awake() {
        manager = GameObject.Find("AppManager");
    }

    IEnumerator Start() {
        if (manager.GetComponent<appManager>().getImagen() != "") {
            WWW www = new WWW(manager.GetComponent<appManager>().getImagen());
            yield return www;
            imagen.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        }
    }

    public void clickCategoria(string categoria) {
        var idCategoria = webServiceCategoria.getIdCategoriaByNameSqLite(categoria);
        var materias = webServiceMateria.getIdMateriasByCategoriaSqLite(idCategoria);
        if (materias != "0") {
            string[] splitString = materias.Split(new string[] { "\r\n", "," }, StringSplitOptions.None);
            List<webServicePreguntas.preguntaData> preguntasList = new List<webServicePreguntas.preguntaData>();
            for (int i = 0; i < splitString.Length; i++) {
                preguntasList.Add(webServicePreguntas.getPreguntasByMateria(splitString[i]));
                Debug.Log(preguntasList[i].descripcion);
            }
        } else {
            Debug.Log("No hay materias registradas");
        }
    }

    public void salir() {

    }
}
