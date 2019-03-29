using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class CursoManager : MonoBehaviour {

    public Text textoRacha;
    public Text textoMultiplicador;
    public Text textoPuntajeObtenido;
    public Text textoPuntaje;
    public Text textoPuntajeMarcador;
    public Text textoCompletado;
    public GameObject correctoimg;
    public GameObject incorrectoimg;
    public GameObject scoreFinal;
    public Text preguntaText;

    public AudioClip good;
    public AudioClip great;
    public AudioClip awsome;
    public AudioClip perfect;
    public AudioClip ops;

    public GameObject butonToInstantiate;
    public GameObject butonToInstantiateText;
    public GameObject canvasParentOfAnswers;

    webServicePreguntas.preguntaData[] preguntas = null;
    appManager manager;

    string descripcionTipoEjercicio;

    int countPreguntas = 0;
    int score;
    int racha = 0;
    int multiplicador;

    int correctasAContestar = 0;
    int correctas = 0;
    string parUno = "";
    string parDos = "";
    bool seleccion = false;
    string fraseACompletar = "";
    string fraseCompletada = "";

    string idIntento = "";
    string idPregunta = "";
    string idRespuesta = "";

    void Start() {
        multiplicador = 1;
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
                        webServiceRegistro.validarAccionSqlite("Respondió correctamente(Simple)", manager.getUsuario(), "Respondió pregunta");
                        respuestaCorrecta();
                    }
                    break;
                case "Completar palabra":
                    textoCompletado.text = fraseCompletada;
                    if (fraseCompletada == fraseACompletar) {
                        webServiceRegistro.validarAccionSqlite("Respondió correctamente(Completar palabra): " + fraseCompletada, manager.getUsuario(), "Respondió pregunta");
                        fraseCompletada = "";
                        fraseACompletar = "l";
                        webServiceDetalleIntento.insertarDetalleIntentoSqLite("True", idPregunta, idRespuesta, idIntento);
                        respuestaCorrecta();
                    }
                    break;
                case "Seleccion Multiple":
                    if (correctas >= correctasAContestar) {
                        webServiceRegistro.validarAccionSqlite("Respondió correctamente(Seleccion Multiple)", manager.getUsuario(), "Respondió pregunta");
                        respuestaCorrecta();
                    }
                    break;
                case "Relacionar":
                    if (seleccion) {
                        if (parDos != "") {
                            if (parUno == parDos) {
                                webServiceDetalleIntento.insertarDetalleIntentoSqLite("True", idPregunta, idRespuesta, idIntento);
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
                        if (correctas >= correctasAContestar / 2) {
                            webServiceRegistro.validarAccionSqlite("Respondió correctamente(Relacionar)", manager.getUsuario(), "Respondió pregunta");
                            seleccion = false;
                            respuestaCorrecta();
                        }
                    }
                    break;
                case "Seleccion simple texto":
                    if (correctas >= correctasAContestar) {
                        webServiceRegistro.validarAccionSqlite("Respondió correctamente(Seleccion simple texto)", manager.getUsuario(), "Respondió pregunta");
                        respuestaCorrecta();
                    }
                    break;
                default:

                    break;
            }
        } else if (correctas < 0) {
            webServiceRegistro.validarAccionSqlite("Respondió incorrectamente(" + descripcionTipoEjercicio + ")", manager.getUsuario(), "Respondió pregunta");
            countPreguntas++;
            correctas = 0;
            racha = 0;
            multiplicador = 1;
            textoRacha.text ="";
            textoMultiplicador.text = "";
            StartCoroutine(activaObjeto(incorrectoimg));
            webServiceIntento.updateIntentoSqlite(idIntento, score.ToString());
            webServiceDetalleIntento.insertarDetalleIntentoSqLite("False", idPregunta, idRespuesta, idIntento);
        }
    }

    public void respuestaCorrecta() {
        countPreguntas++;
        correctas = 0;
        verificarRacha();
        textoPuntaje.text = score + "";
        StartCoroutine(activaObjeto(correctoimg));
        webServiceIntento.updateIntentoSqlite(idIntento, score.ToString());
    }

    public void verificarRacha(){
        int puntajePregunta;
        racha++;
        textoRacha.text = racha + "";
        if (racha >= 2) {
            puntajePregunta = 100 * multiplicador;
            textoPuntajeObtenido.text = puntajePregunta + "";
            score = score + puntajePregunta;
            if (multiplicador < 4) {
                multiplicador++;
            }
            textoMultiplicador.text = "X" + multiplicador;
        } else {
            puntajePregunta = 100;
            textoPuntajeObtenido.text = puntajePregunta + "";
            score = score + puntajePregunta;
        }
    }

    public void llamarPreguntas() {
        if (countPreguntas < preguntas.Length) {
            idPregunta = preguntas[countPreguntas].id;
            descripcionTipoEjercicio = preguntas[countPreguntas].descripcionEjercicio;
            webServiceRegistro.validarAccionSqlite("Pregunta: " + preguntas[countPreguntas].descripcion, manager.getUsuario(), "Entró a pregunta");
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
            webServiceRegistro.validarAccionSqlite("Puntaje obtenido: " + score, manager.getUsuario(), "Puntaje obtenido");
            webServiceRegistro.validarAccionSqlite("Terminó ejercicio", manager.getUsuario(), "Terminó ejercicio");
            scoreFinal.SetActive(true);
        }
    }

    public void imprimePregunta() {
        preguntaText.text = preguntas[countPreguntas].descripcion;
        if (descripcionTipoEjercicio == "Completar palabra") {
            var respuestasOfQuestion = webServiceRespuestas.getRespuestasByPreguntaSqLite(preguntas[countPreguntas].id).respuestas[0];
            idRespuesta = respuestasOfQuestion.id;
            var palabra = respuestasOfQuestion.descripcion;
            llenarLetras(palabra);
            fraseACompletar = palabra.ToUpper();
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
        palabra = palabra.ToUpper();
        var letras = shuffleArray(palabra);
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
        //Validar la ñ o caracteres especiales

        Vector3 pos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        var x = Instantiate(butonToInstantiate, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
        x.transform.SetParent(canvasParentOfAnswers.transform, false);
        x.transform.LookAt(GameObject.Find("CenterEyeAnchor").transform);
        var rotation = x.transform.localRotation.eulerAngles;
        rotation += new Vector3(-21, 180, 0);
        x.transform.localRotation = Quaternion.Euler(rotation);
        x.AddComponent<clickManager>();
        Debug.Log("Letras/letra-" + respuesta);
        var spriteObj = Resources.Load("Letras/letra-" + respuesta);
        var imagen = x.GetComponentInChildren<Button>().gameObject.GetComponent<Image>();
        Texture2D tex = spriteObj as Texture2D;
        Rect rec = new Rect(0, 0, tex.width, tex.height);
        var sprite = Sprite.Create(tex, rec, new Vector2(0.5f, 0.5f), 100);
        imagen.sprite = sprite;
        addEvent(x, respuesta);
    }

    public void crearBoton(webServiceRespuestas.respuestaData respuesta, float angle, float radius) {
        GameObject x;
        if (descripcionTipoEjercicio != "Seleccion simple texto") {
            x = crearObjeto(angle, radius, butonToInstantiate);
            var splitUrk = respuesta.urlImagen.Split('/');
            var path = splitUrk[splitUrk.Length - 1];
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
            Texture2D texture = new Texture2D(8, 8);
            texture.LoadImage(byteArray);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            x.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().sprite = sprite;
        } else {
            x = crearObjeto(angle, radius, butonToInstantiateText);
            x.GetComponentInChildren<Text>().text = respuesta.descripcion;
            //llenar texto en base a la respuesta
        }
        if (descripcionTipoEjercicio == "Relacionar") {
            addEventPares(x, respuesta);
        } else {
            addEvent(x, respuesta);
        }
    }

    public GameObject crearObjeto(float angle, float radius, GameObject boton) {
        GameObject x;
        Vector3 pos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        x = Instantiate(boton, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
        x.transform.SetParent(canvasParentOfAnswers.transform, false);
        x.transform.LookAt(GameObject.Find("CenterEyeAnchor").transform);
        var rotation = x.transform.localRotation.eulerAngles;
        rotation += new Vector3(-21, 180, 0);
        x.transform.localRotation = Quaternion.Euler(rotation);
        if (!x.GetComponent<OVRRaycaster>()) {
            x.AddComponent<OVRRaycaster>();
        }
        return x;
    }

    void addEvent(GameObject obj, char caracter) {
        obj.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            fraseCompletada += caracter;
            if (fraseCompletada[fraseCompletada.Length - 1] == fraseACompletar[fraseCompletada.Length - 1]) {

            } else {
                webServiceRegistro.validarAccionSqlite("Respondió incorrectamente: " + fraseCompletada, manager.getUsuario(), "Respondió pregunta");
                correctas = -1;
                racha = 0;
                multiplicador = 1;
                textoRacha.text = "";
                textoMultiplicador.text = "";
            }
            Destroy(obj);
        });
    }

    void addEvent(GameObject obj, webServiceRespuestas.respuestaData respuesta) {
        obj.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            idRespuesta = respuesta.id;
            if (respuesta.correcto == "True") {
                webServiceDetalleIntento.insertarDetalleIntentoSqLite("True", idPregunta, respuesta.id, idIntento);
                correctas++;
            } else {
                correctas = -1;
            }
            Destroy(obj);
        });
    }

    void addEventPares(GameObject obj, webServiceRespuestas.respuestaData respuesta) {
        obj.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            idRespuesta = respuesta.id;
            if (seleccion) {
                parDos = respuesta.relacion;
            } else {
                parUno = respuesta.relacion;
                webServiceDetalleIntento.insertarDetalleIntentoSqLite("True", idPregunta, idRespuesta, idIntento);
                seleccion = true;
            }
            idRespuesta = respuesta.id;
            Destroy(obj);
        });
    }

    void valAudio(GameObject obj) {
        switch (racha) {
            case 0:
                obj.GetComponentInChildren<AudioSource>().clip = ops;
                break;
            case 1:
                obj.GetComponentInChildren<AudioSource>().clip = good;
                break;
            case 2:
                obj.GetComponentInChildren<AudioSource>().clip = good;
                break;
            case 3:
                obj.GetComponentInChildren<AudioSource>().clip = great;
                break;
            case 4:
                obj.GetComponentInChildren<AudioSource>().clip = awsome;
                break;
            default:
                obj.GetComponentInChildren<AudioSource>().clip = perfect;
                break;
        }
    }

    IEnumerator activaObjeto(GameObject objeto) {
        destroyChildrens();
        objeto.SetActive(true);
        valAudio(objeto);
        objeto.GetComponentInChildren<AudioSource>().Play();
        yield return new WaitUntil(() => objeto.GetComponentInChildren<AudioSource>().isPlaying == false);
        objeto.SetActive(false);
        correctasAContestar = 0;
        textoPuntajeObtenido.text = "";
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

    public webServicePreguntas.preguntaData[] shuffleArray(webServicePreguntas.preguntaData[] preguntas) {
        for (int t = 0; t < preguntas.Length; t++) {
            var tmp = preguntas[t];
            int r = UnityEngine.Random.Range(t, preguntas.Length);
            preguntas[t] = preguntas[r];
            preguntas[r] = tmp;
        }
        return preguntas;
    }

    public char[] shuffleArray(string palabra) {
        char[] palabraChar = palabra.ToCharArray();
        for (int t = 0; t < palabraChar.Length; t++) {
            var tmp = palabraChar[t];
            int r = UnityEngine.Random.Range(t, palabraChar.Length);
            palabraChar[t] = palabraChar[r];
            palabraChar[r] = tmp;
        }
        return palabraChar;
    }

    void OnApplicationQuit() {
        if (manager.getUsuario() != "") {
            webServiceRegistro.validarAccionSqlite("El usuario no termino la partida", manager.getUsuario(), "Ejercicio sin terminar");
            webServiceLog.cerrarLog(manager.getUsuario());
        }
    }

}
