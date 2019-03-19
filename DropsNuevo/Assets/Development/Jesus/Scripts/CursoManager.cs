using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class CursoManager : MonoBehaviour {

    public Text textoPuntaje;
    public Text textoPuntajeMarcador;
    public Text textoCompletado;
    public GameObject correctoimg;
    public GameObject incorrectoimg;
    public GameObject scoreFinal;
    public Text preguntaText;

    public GameObject butonToInstantiate;
    public GameObject canvasParentOfAnswers;

    webServicePreguntas.preguntaData[] preguntas = null;
    appManager manager;

    string descripcionTipoEjercicio;

    int countPreguntas = 0;
    int score;

    int correctasAContestar = 0;
    int correctas = 0;
    string parUno = "";
    string parDos = "";
    bool seleccion = false;
    string fraseACompletar = "";
    string fraseCompletada = "";

    string idIntento = "";

    void Start() {
        scoreFinal.SetActive(false);
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
        preguntas = manager.preguntasCategoria;
        loadSalonCategoria();
        var idUsuario = webServiceUsuario.consultarIdUsuarioSqLite(manager.getUsuario());
        var idLog = webServiceLog.getLastLogSqLite(idUsuario);
        webServiceIntento.insertarIntentoSqLite("0", manager.getUsuario());
        idIntento = webServiceIntento.consultarUltimoIdIntentoByIdLogSqLite(idLog);
        llamarPreguntas();
    }

    private void Update() {
        if (correctas >= 0 && countPreguntas < preguntas.Length) {
            switch (descripcionTipoEjercicio) {
                case "Seleccion simple":
                    if (correctas >= correctasAContestar) {
                        countPreguntas++;
                        correctas = 0;
                        score++;
                        textoPuntaje.text = score + "";
                        StartCoroutine(activaObjeto(correctoimg));
                    }
                    break;
                case "Completar palabra":
                    textoCompletado.text = fraseCompletada;
                    if (fraseCompletada == fraseACompletar) {
                        fraseCompletada = "";
                        fraseACompletar = "l";
                        countPreguntas++;
                        correctas = 0;
                        score++;
                        textoPuntaje.text = score + "";
                        StartCoroutine(activaObjeto(correctoimg));
                    }
                    break;
                case "Seleccion Multiple":
                    if (correctas >= correctasAContestar) {
                        countPreguntas++;
                        correctas = 0;
                        score++;
                        textoPuntaje.text = score + "";
                        StartCoroutine(activaObjeto(correctoimg));
                    }
                    break;
                case "Relacionar":
                    if (seleccion) {
                        if (parDos != "") {
                            if (parUno == parDos) {
                                correctas++;
                                parUno = "a";
                                parDos = "";
                                seleccion = false;
                            } else {
                                seleccion = false;
                                parUno = "a";
                                parDos = "";
                                correctas = -1;
                            }
                        }
                        if (correctas >= 3) {
                            countPreguntas++;
                            seleccion = false;
                            correctas = 0;
                            score++;
                            textoPuntaje.text = score + "";
                            StartCoroutine(activaObjeto(correctoimg));
                        }
                    }
                    break;
                case "Seleccion simple texto":
                    if (correctas >= correctasAContestar) {
                        countPreguntas++;
                        correctas = 0;
                        score++;
                        textoPuntaje.text = score + "";
                        StartCoroutine(activaObjeto(correctoimg));
                    }
                    break;
                default:

                    break;
            }
        } else if(correctas < 0){
            countPreguntas++;
            correctas = 0;
            StartCoroutine(activaObjeto(incorrectoimg));
        }
    }

    public void llamarPreguntas() {
        if (countPreguntas < preguntas.Length) {
            webServiceRegistro.validarAccionSqlite("Pregunta: " + preguntas[countPreguntas].descripcion, manager.getUsuario(), "Entró a pregunta");
            descripcionTipoEjercicio = preguntas[countPreguntas].descripcionEjercicio;
            switch (descripcionTipoEjercicio) {
                case "Seleccion simple":
                    imprimePregunta();
                    break;
                case "Completar palabra":
                    fraseCompletada = "";
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
                    break;
            }
        } else {
            destroyChildrens();
            textoPuntajeMarcador.text = score + "";
            scoreFinal.SetActive(true);
        }
    }

    public void imprimePregunta() {
        preguntaText.text = preguntas[countPreguntas].descripcion;
        if (descripcionTipoEjercicio == "Completar palabra") {
            var respuestasOfQuestion = webServiceRespuestas.getRespuestasByPreguntaSqLite(preguntas[countPreguntas].id);
            var palabra = respuestasOfQuestion.respuestas[0].descripcion;
            llenarLetras(palabra);
            fraseACompletar = palabra;
        } else {
            var respuestasOfQuestion = webServiceRespuestas.getRespuestasByPreguntaSqLite(preguntas[countPreguntas].id);
            llenarRespuestas(respuestasOfQuestion.respuestas);
        }
    }

    public void llenarRespuestas(webServiceRespuestas.respuestaData[] respuestas) {
        var numberOfObjects = respuestas.Length;
        if (numberOfObjects == 6) {
            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 30, 0));
        }
        var radius = 4f;
        int p = 1;
        int i = 0;
        destroyChildrens();
        foreach (var respuesta in respuestas) {
            if (respuesta.correcto == "True") {
                correctasAContestar++;
            }
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            crearBoton(respuesta, angle, radius);
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
            crearBotonLetra(caratcter, angle, radius);
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
        addEvent(x, respuesta);
    }

    public void crearBoton(webServiceRespuestas.respuestaData respuesta, float angle, float radius) {
        Vector3 pos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        var x = Instantiate(butonToInstantiate, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
        x.transform.SetParent(canvasParentOfAnswers.transform, false);
        x.transform.LookAt(GameObject.Find("Main Camera").transform);
        var rotation = x.transform.localRotation.eulerAngles;
        rotation += new Vector3(-21, 180, 0);
        x.transform.localRotation = Quaternion.Euler(rotation);
        var splitUrk = respuesta.urlImagen.Split('/');
        var path = splitUrk[splitUrk.Length - 1];
        byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
        Texture2D texture = new Texture2D(8, 8);
        texture.LoadImage(byteArray);
        Rect rec = new Rect(0, 0, texture.width, texture.height);
        var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
        x.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().sprite = sprite;
        if (descripcionTipoEjercicio == "Relacionar") {
            addEventPares(x, respuesta);
        } else {
            addEvent(x, respuesta);
        }
    }

    void addEvent(GameObject obj, char caracter) {
        obj.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            fraseCompletada += caracter;
            if (fraseCompletada[fraseCompletada.Length - 1] == fraseACompletar[fraseCompletada.Length - 1]) {
            } else {
                correctas = -1;
            }
            Destroy(obj);
        });
    }

    void addEvent(GameObject obj, webServiceRespuestas.respuestaData respuesta) {
        obj.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            if (respuesta.correcto == "True") {
                correctas++;
            } else {
                correctas = -1;
            }
            string idI = idIntento;
            string idR = respuesta.id;
            string idP = respuesta.idPregunta;
            string correcto = respuesta.correcto;
            webServiceDetalleIntento.insertarDetalleIntentoSqLite(correcto, idP, idR, idI);
            Destroy(obj);
        });
    }

    void addEventPares(GameObject obj, webServiceRespuestas.respuestaData respuesta) {
        obj.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            if (seleccion) {
                parDos = respuesta.relacion;
            } else {
                parUno = respuesta.relacion;
                seleccion = true;
            }
            Destroy(obj);
        });
    }

    IEnumerator activaObjeto(GameObject objeto) {
        destroyChildrens();
        objeto.SetActive(true);
        objeto.GetComponentInChildren<AudioSource>().Play();
        yield return new WaitUntil(() => objeto.GetComponentInChildren<AudioSource>().isPlaying == false);
        objeto.SetActive(false);
        correctasAContestar = 0;
        llamarPreguntas();
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

    void destroyChildrens() {
        if (canvasParentOfAnswers.transform.childCount > 0) {
            foreach (var obj in canvasParentOfAnswers.GetComponentsInChildren<Canvas>()) {
                DestroyImmediate(obj.gameObject);
            }
        }
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
