using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.EventSystems;

public class CursoManager : MonoBehaviour {

    public Text textoPuntaje;
    public Text textoPuntajeMarcador;
    public GameObject correctoimg;
    public GameObject incorrectoimg;
    public GameObject scoreFinal;
    public Text preguntaText;

    public GameObject butonToInstantiate;
    public GameObject canvasParentOfAnswers;

    //Variables que almacenan la preguntas y sus respuestas
    webServicePreguntas.preguntaData[] preguntas = null;
    appManager manager;

    //Tipo de pregunta
    string descripcionTipoEjercicio;

    //Variables que almacena el numero de preguntas descargadas y el puntaje actual
    int countPreguntas = 0;
    int score;

    string idIntento;


    void Start() {
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
        preguntas = manager.preguntasCategoria;
        loadSalonCategoria();
        var idUsuario = webServiceUsuario.consultarIdUsuarioSqLite(manager.getUsuario());
        var idLog = webServiceLog.getLastLogSqLite(idUsuario);
        webServiceIntento.insertarIntentoSqLite("0", manager.getUsuario());
        idIntento = webServiceIntento.consultarUltimoIdIntentoByIdLogSqLite(idLog);
        llamarPreguntas();

    }

    public void llamarPreguntas() {
        Debug.Log(countPreguntas);
        if (countPreguntas < preguntas.Length) {
            webServiceRegistro.validarAccionSqlite("Pregunta: " + preguntas[countPreguntas].descripcion, manager.getUsuario(), "Entró a pregunta");
            Debug.Log(preguntas[countPreguntas].descripcionEjercicio);
            descripcionTipoEjercicio = preguntas[countPreguntas].descripcionEjercicio;
            switch (descripcionTipoEjercicio) {
                case "Seleccion simple":
                    imprimePregunta();
                    break;
                case "Completar palabra":
                    imprimePregunta();
                    break;
                case "Seleccion Multiple":
                    imprimePregunta();
                    break;
                case "Relacionar":
                    imprimePregunta();
                    break;
                case "Seleccion simple texto":
                    imprimePregunta();
                    break;
                default:
                    Debug.Log("No existe el tipo de ejercicio");
                    break;
            }
        }
    }

    public void imprimePregunta() {
        preguntaText.text = preguntas[countPreguntas].descripcion;
        if (descripcionTipoEjercicio == "Completar palabra") {
            var respuestasOfQuestion = webServiceRespuestas.getRespuestasByPreguntaSqLite(preguntas[countPreguntas].id);
            var palabra = respuestasOfQuestion.respuestas[0].descripcion;
            llenarLetras(palabra);
        } else {
            var respuestasOfQuestion = webServiceRespuestas.getRespuestasByPreguntaSqLite(preguntas[countPreguntas].id);
            llenarRespuestas(respuestasOfQuestion.respuestas);
        }
    }

    public void llenarRespuestas(webServiceRespuestas.respuestaData[] respuestas) {
        Array.Sort(respuestas);
        var numberOfObjects = respuestas.Length;
        if (numberOfObjects == 6) {
            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0,30,0));
        }
        var radius = 4f;
        int p = 1;
        int i = 0;
        foreach (var respuesta in respuestas) {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            if (p == 1) {
                p = 2;
                crearBoton(respuesta, angle, radius);
            } else {
                p = 1;
                crearBoton(respuesta, angle, radius);
            }
            i++;
        }
    }

    public void llenarLetras(string palabra) {
        var letras = palabra.ToCharArray();
        Array.Sort(letras);
        var numberOfObjects = palabra.Length;
        var radius = 4f;
        int p = 1;
        int i = 0;
        foreach (char caratcter in letras) {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            if (p == 1) {
                p = 2;
                crearBotonLetra(caratcter, angle, radius);
            } else {
                p = 1;
                crearBotonLetra(caratcter, angle, radius);
            }
            i++;
        }
    }

    public void crearBotonLetra(char respuesta, float angle, float radius) {
        Vector3 pos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        var x = Instantiate(butonToInstantiate, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
        x.transform.SetParent(canvasParentOfAnswers.transform, false);
        x.transform.LookAt(GameObject.Find("Main Camera").transform);
        var rotation = x.transform.localRotation.eulerAngles;
        rotation += new Vector3(-21, 180, 0);
        x.transform.localRotation = Quaternion.Euler(rotation);
        var spriteObj = Resources.Load("Letras/letra-" + respuesta);
        var imagen = x.GetComponentInChildren<Button>().gameObject.GetComponent<Image>();
        Texture2D tex = spriteObj as Texture2D;
        Rect rec = new Rect(0, 0, tex.width, tex.height);
        var sprite = Sprite.Create(tex, rec, new Vector2(0.5f, 0.5f), 100);
        imagen.sprite = sprite;
    }

    public void crearBoton(webServiceRespuestas.respuestaData respuesta, float angle, float radius) {
        Vector3 pos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        var x = Instantiate(butonToInstantiate, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
        x.transform.SetParent(canvasParentOfAnswers.transform, false);
        x.transform.LookAt(GameObject.Find("Main Camera").transform);
        var rotation = x.transform.localRotation.eulerAngles;
        rotation += new Vector3(-21, 180, 0);
        x.transform.localRotation = Quaternion.Euler(rotation);
        fillButon(respuesta, x);
    }

    public void fillButon(webServiceRespuestas.respuestaData respuesta, GameObject obj) {
        var splitUrk = respuesta.urlImagen.Split('/');
        var path = splitUrk[splitUrk.Length - 1];
        byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
        Texture2D texture = new Texture2D(8, 8);
        texture.LoadImage(byteArray);
        Rect rec = new Rect(0, 0, texture.width, texture.height);
        var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
        obj.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().sprite = sprite;
    }

    IEnumerator esperaSegundos(GameObject objeto) {
        objeto.SetActive(true);
        objeto.GetComponentInChildren<AudioSource>().Play();
        yield return new WaitUntil(() => objeto.GetComponentInChildren<AudioSource>().isPlaying == false);
        objeto.SetActive(false);
    }

    void loadSalonCategoria() {
        var categoria = webServiceCategoria.getCategoriaByIdSqLite(webServicePaquetes.getPaquetesByDescripcionSqLite(preguntas[0].descripcionPaquete).idCategoria).descripcion;
        categoria = categoria.Replace(" ", string.Empty);
        var salon = Instantiate(Resources.Load("Salones/salonTemplate" + categoria) as GameObject);
        salon.SetActive(true);
        salon.transform.localScale = new Vector3(5.216759f, 4, 4);
        salon.transform.localPosition = new Vector3(10.68f, 1.11f, 6.93f);
        salon.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    public void salir() {
        SceneManager.LoadScene("menuCategorias");
    }

    void OnApplicationQuit() {
        if (manager.getUsuario() != "") {
            webServiceRegistro.validarAccionSqlite("El usuario no termino la partida", manager.getUsuario(), "Ejercicio sin terminar");
            webServiceLog.cerrarLog(manager.getUsuario());
        }
    }

}
