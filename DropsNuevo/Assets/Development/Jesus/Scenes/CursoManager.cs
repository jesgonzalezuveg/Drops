using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class CursoManager : MonoBehaviour {

    public Text textoPuntaje;
    public GameObject correctoimg;
    public GameObject incorrectoimg;

    webServicePreguntas.preguntaData[] preguntas = null;
    List<webServiceRespuestas.Data> respuestasTodas = new List<webServiceRespuestas.Data>();
    appManager manager;
    int countPreguntas=0;
    int score;

    // Start is called before the first frame update
    void Start() {
        score = 0;
        textoPuntaje.text = score + "";
        correctoimg.SetActive(false);
        incorrectoimg.SetActive(false);
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
        preguntas = manager.preguntasCategoria;
        for (var i = 0; i < preguntas.Length; i++) {
            //Debug.Log(manager.preguntasCategoria[i].descripcion);
            Debug.Log(manager.preguntasCategoria[i].id);
            var respuestas = webServiceRespuestas.getRespuestasByPreguntaSqLite(manager.preguntasCategoria[i].id);
            respuestasTodas.Add(respuestas);
        }

        llamarPreguntas(countPreguntas);
        
    }

    public void llamarPreguntas(int position) {
        GameObject.Find("txtQuestion").GetComponent<Text>().text = manager.preguntasCategoria[position].descripcion;
        int count = 2;
        for (int i = 0; i < respuestasTodas[position].respuestas.Length; i++) {
            string url = respuestasTodas[position].respuestas[i].urlImagen;
            if (respuestasTodas[position].respuestas[i].correcto == "True") {
                var splitUrk = url.Split('/');
                var urlReal = splitUrk[splitUrk.Length - 1];
                ponerImagen(1, urlReal);
            } else {
               
                var splitUrk = url.Split('/');
                var urlReal = splitUrk[splitUrk.Length - 1];
                ponerImagen(count, urlReal);
                count++;
            }
        }
        countPreguntas++;
    }

    public void ponerImagen(int i, string path) {
        if (File.Exists(Application.persistentDataPath + path)) {
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
            Texture2D texture = new Texture2D(8,8);
            texture.LoadImage(byteArray);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            var objeto = "objRespuesta" + i;
            var imagen = GameObject.Find(objeto);
            imagen.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    public void correcto() {
        Debug.Log("correcto  --- " + preguntas.Length + "CountPreguntas: " + countPreguntas);
        StartCoroutine(esperaSegundos(2, correctoimg));
        score++;
        textoPuntaje.text = score + "";
        if (preguntas.Length > countPreguntas) {
            llamarPreguntas(countPreguntas);
        } else {
            SceneManager.LoadScene("menuCategorias");
        }
    }

    public void incorrecto() {
        Debug.Log("incorrecto  --- " + preguntas.Length + "CountPreguntas: " + countPreguntas);
        StartCoroutine(esperaSegundos(2, incorrectoimg));
        if (preguntas.Length > countPreguntas) {
            llamarPreguntas(countPreguntas);
        } else {
            SceneManager.LoadScene("menuCategorias");
        }
    }

    IEnumerator esperaSegundos(int segundos, GameObject objeto) {
        objeto.SetActive(true);
        yield return new WaitForSeconds(segundos);
        objeto.SetActive(false);
    }

}
