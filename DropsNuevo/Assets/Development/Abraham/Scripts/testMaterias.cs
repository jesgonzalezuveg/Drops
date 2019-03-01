using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class testMaterias : MonoBehaviour {

    appManager manager;
    public Image imagen;
    public GameObject textoPaquetes;
    private bool bandera = true;

    private void Awake() {
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
    }

    private void Start() {
        StartCoroutine(webServicePaquetes.getPaquetes());
        StartCoroutine(webServiceCategoria.getCategorias());
        StartCoroutine(webServiceMateria.getMaterias());
        StartCoroutine(webServiceEjercicio.getEjercicios());
        
    }

    private void Update() {
        if (manager.getImagen() != "" && bandera) {
            StartCoroutine(getUserImg());
            bandera = false;
        }
    }


    IEnumerator getUserImg() {
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
            string[] splitString = materias.Split(',');
            bool banderaPreguntas = true;
            for (var i = 0; i < splitString.Length; i++ ) {
                var preguntas = webServicePreguntas.getPreguntasByMateria(splitString[i]);
                if (preguntas != null) {
                    foreach (var pregunta in preguntas) {
                        Debug.Log(pregunta.descripcion);
                        banderaPreguntas = false;
                    }
                    //MODIFICAR MANDAR DE TODAS LAS MATERIAS
                    manager.preguntasCategoria = preguntas;
                } else {
                    Debug.Log("No hay preguntas en esta categoria");
                }
            }
            if (!banderaPreguntas) {
                SceneManager.LoadScene("salon");
            }
        } else {
            Debug.Log("No hay materias registradas");
        }
    }

    public void salir() {
        Application.Quit();
    }
}
