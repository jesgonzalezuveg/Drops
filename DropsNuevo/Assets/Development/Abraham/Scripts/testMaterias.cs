using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class testMaterias : MonoBehaviour {

    [Serializable]
    public class Data {
        public string id = "";
        public string descripcion = "";
    }

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
        var idCategoria = webServiceCategoria.getIdCategoriaByName(categoria);
        var materias = webServiceMateria.getMateriasByCategoria(Int32.Parse(idCategoria));
        string[] splitString = materias.Split(new string[] { "\r\n", "," }, StringSplitOptions.None);

        List<string> preguntas = new List<string>();
        List<Data> preguntasList = new List<Data>();

        for (int i = 0; i < splitString.Length; i++) {
            preguntas.Add(webServicePreguntas.getPreguntasByMateria(splitString[i]));
            string stringjsonData = "{\"id\": \"1\", \"descripcion\": \"hola\"}";
            Debug.Log(preguntas[i]);
            Debug.Log(stringjsonData);
            Data json = JsonUtility.FromJson<Data>(stringjsonData);
            print(json.descripcion);
        }
    }

    public void salir() {

    }
}
