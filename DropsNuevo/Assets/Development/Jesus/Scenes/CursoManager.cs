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
    public GameObject[] preguntasTipos; 

    // Variables para la poscision aleatoria
    int num;
    Vector3 temp1, temp2, temp3, temp4, temp5, temp6;
    Vector3 rotation1, rotation2, rotation3, rotation4, rotation5, rotation6;
    Transform transform1, transform2, transform3, transform4, transform5, transform6;
    GameObject Imagen1, Imagen2, Imagen3, Imagen4, Imagen5, Imagen6;
    // Variables para la poscision aleatoria

    webServicePreguntas.preguntaData[] preguntas = null;
    List<webServiceRespuestas.Data> respuestasTodas = new List<webServiceRespuestas.Data>();
    appManager manager;

    int countPreguntas = 0;
    int score;

    //Variables para ejercicio de seleccion multiple
    int countSelectMultiple; //Contador que indica el numero de respuestas correctas en el ejercicio
    public Sprite opcionCorrecta; //Imagen que aparece cuando se selecciona una respuesta correcta.

    //Variables para ejercicio de relacionar pares.
    int numPares;
    string par1;
    string par2;
    string par1Name;
    string par2Name;

    //Variables para ejercicio de completar la frase
    int numLetras;
    int letra;

    // Start is called before the first frame update
    void Start() {
        par1 = "";
        par2 = "";
        par1Name = "";
        par2Name = "";
        numPares = 0;
        numLetras = 0;
        letra = 1;
        countSelectMultiple = 0;
        score = 0;
        textoPuntaje.text = score + "";
        correctoimg.SetActive(false);
        incorrectoimg.SetActive(false);
        scoreFinal.SetActive(false);
        desactivarPreguntas();
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
        preguntas = manager.preguntasCategoria;
        for (var i = 0; i < preguntas.Length; i++) {
            Debug.Log(manager.preguntasCategoria[i].descripcion);
            Debug.Log(manager.preguntasCategoria[i].id);
            Debug.Log("Tipo de ejericio "+manager.preguntasCategoria[i].idTipoEjercicio);
            var respuestas = webServiceRespuestas.getRespuestasByPreguntaSqLite(manager.preguntasCategoria[i].id);
            respuestasTodas.Add(respuestas);
        }

        llamarPreguntas(countPreguntas);
        
    }

    public void llamarPreguntas(int position) {
        Debug.Log("pregunta" + position);
        if (position<preguntas.Length) {
            //Validamos de que tipo de ejercicio se trata
            if (manager.preguntasCategoria[position].idTipoEjercicio == "1") {
                Debug.Log("La pregunta es de tipo 1");
                preguntasTipos[0].SetActive(true);
                GameObject.Find("txtQuestionS").GetComponent<Text>().text = manager.preguntasCategoria[position].descripcion;
                //GameObject.Find("txtQuestionM").GetComponent<Text>().text = SystemInfo.deviceType + "";
                int count = 1;
                for (int i = 0; i < respuestasTodas[position].respuestas.Length; i++) {
                    string url = respuestasTodas[position].respuestas[i].urlImagen;
                    var splitUrk = url.Split('/');
                    var urlReal = splitUrk[splitUrk.Length - 1];
                    ponerImagen(count + "S", urlReal, manager.preguntasCategoria[position].idTipoEjercicio, respuestasTodas[position].respuestas[i].correcto);
                    count++;
                }
            }else if (manager.preguntasCategoria[position].idTipoEjercicio == "2") {
                Debug.Log("La pregunta es de tipo 2");
                preguntasTipos[3].SetActive(true);

                GameObject.Find("txtQuestionC").GetComponent<Text>().text = manager.preguntasCategoria[position].descripcion;
                //GameObject.Find("txtQuestionM").GetComponent<Text>().text =SystemInfo.deviceType+"";
                int count = 1;
                for (int i = 0; i < respuestasTodas[position].respuestas.Length; i++) {
                    string palabra = respuestasTodas[position].respuestas[i].descripcion.ToUpper();
                    Debug.Log(palabra);
                    foreach (char letra in palabra) {
                        ponerImagenLetra(letra, count+"C", count);
                        count++;
                    }
                    numLetras = count;
                }

                for (int j = count; j<= 15; j++) {
                    var objeto = "objRespuesta" + j + "C";
                    removeTrigger(objeto);
                    removeColider(objeto);
                }
            }else if (manager.preguntasCategoria[position].idTipoEjercicio == "3") {
                Debug.Log("La pregunta es de tipo 3");
                preguntasTipos[1].SetActive(true);
                GameObject.Find("txtQuestionM").GetComponent<Text>().text = manager.preguntasCategoria[position].descripcion;
                int count = 1;
                for (int i = 0; i < respuestasTodas[position].respuestas.Length; i++) {
                    string url = respuestasTodas[position].respuestas[i].urlImagen;
                    var splitUrk = url.Split('/');
                    var urlReal = splitUrk[splitUrk.Length - 1];
                    ponerImagen(count + "M", urlReal, manager.preguntasCategoria[position].idTipoEjercicio, respuestasTodas[position].respuestas[i].correcto);
                    count++;
                }
            }else if (manager.preguntasCategoria[position].idTipoEjercicio == "4") {
                Debug.Log("La pregunta es de tipo 4");
                preguntasTipos[2].SetActive(true);
                GameObject.Find("txtQuestionP").GetComponent<Text>().text = manager.preguntasCategoria[position].descripcion;
                int count = 1;
                for (int i = 0; i < respuestasTodas[position].respuestas.Length; i++) {
                    string url = respuestasTodas[position].respuestas[i].urlImagen;
                    var splitUrk = url.Split('/');
                    var urlReal = splitUrk[splitUrk.Length - 1];
                    ponerImagen(count + "P", urlReal, manager.preguntasCategoria[position].idTipoEjercicio, respuestasTodas[position].respuestas[i].relacion);
                    count++;
                    numPares++;
                }
                numPares = numPares / 2;
            }
        }
    }

    public void ponerImagenLetra(char letra, string i, int orden) {
        var objeto = "objRespuesta" + i;
        var spriteObj= Resources.Load("Letras/letra-" + letra);
        var imagen = GameObject.Find(objeto);
        Texture2D tex = spriteObj as Texture2D;
        Rect rec = new Rect(0, 0, tex.width, tex.height);
        var sprite = Sprite.Create(tex, rec, new Vector2(0.5f, 0.5f), 100);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => {
            int letraOrden = orden;
            string obj = objeto;
            validarOrden(obj, letraOrden);

        });
        imagen.GetComponent<EventTrigger>().triggers.Add(entry);
        imagen.GetComponent<SpriteRenderer>().sprite =sprite;
    }

    public void ponerImagen(string i, string path, string tipo, string respuesta) {
        if (File.Exists(Application.persistentDataPath + path)) {
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
            Texture2D texture = new Texture2D(8, 8);
            texture.LoadImage(byteArray);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            var objeto = "objRespuesta" + i;
            var imagen = GameObject.Find(objeto);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            if (tipo == "1") {

                if (respuesta == "True") {
                    entry.callback.AddListener((eventData) => {
                        string objName = objeto;
                        correctoSimple(objName);
                    });
                } else {
                    entry.callback.AddListener((eventData) => {
                        string objName = objeto;
                        incorrecto(objeto);
                    });
                }
                imagen.GetComponent<EventTrigger>().triggers.Add(entry);
            }else if (tipo == "3") {
                if (respuesta == "True") {
                    countSelectMultiple++;
                    entry.callback.AddListener((eventData) => {
                        //continuar
                        Debug.Log("correcto");
                        string objName = objeto;
                        correctoMultiple(objName);
                    });
                } else {
                    entry.callback.AddListener((eventData) => {
                        Debug.Log("incorrecto");
                        string objName = objeto;
                        incorrecto(objeto);
                    });
                }
                imagen.GetComponent<EventTrigger>().triggers.Add(entry);
            }else if (tipo == "4") {
                entry.callback.AddListener((eventData) => {
                    //continuar
                    string objName = objeto;
                    string relacion = respuesta;
                    validarPares(objName, relacion);
                });
                imagen.GetComponent<EventTrigger>().triggers.Add(entry);
            }

            imagen.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    public void correctoSimple(string obj) {
        countPreguntas++;
        Debug.Log("correcto  --- " + preguntas.Length + "CountPreguntas: " + countPreguntas);
        StartCoroutine(esperaSegundos(0.5f, correctoimg));
        score++;
        removeTrigger(obj);
        textoPuntaje.text = score + "";
        if (preguntas.Length > countPreguntas) {
            desactivarPreguntas();
            llamarPreguntas(countPreguntas);
        } else {
            desactivarPreguntas(); ;
            textoPuntajeMarcador.text = score + "";
            scoreFinal.SetActive(true);
            //SceneManager.LoadScene("menuCategorias");
        }
    }

    public void correctoMultiple(string obj) {
        var myGameObject = GameObject.Find(obj);
        countSelectMultiple--;
        if (countSelectMultiple != 0) {
            myGameObject.GetComponent<SpriteRenderer>().sprite = opcionCorrecta;
            Debug.Log("Faltan respuestas");
        } else {
            countPreguntas++;
            Debug.Log("Respuestas completas");
            score++;
            removeTrigger(obj);
            textoPuntaje.text = score + "";
            Debug.Log(preguntas.Length);
            Debug.Log(countPreguntas);
            if (preguntas.Length > countPreguntas) {
                desactivarPreguntas();
                llamarPreguntas(countPreguntas);
            } else {
                desactivarPreguntas();
                textoPuntajeMarcador.text = score + "";
                scoreFinal.SetActive(true);
            }
        }
    }

    public void validarPares(string obj, string relacion) {
        var myGameObject = GameObject.Find(obj);
        if (par1 == "" && par2 == "") {
            Debug.Log("entro a la validacion de pares");
            par1Name = obj;
            par1 = relacion;
            //myGameObject.GetComponent<Renderer>().material.color = new Color32(255, 255, 225, 100);
            myGameObject.GetComponent<SpriteRenderer>().sprite = opcionCorrecta;
        }else if (par1 != "" && par2 == "") {
            Debug.Log("entro a la validacion 2 de pares");
            par2 = relacion;
            par2Name = obj;

            if (par1 == par2) {
                var par1Seleccionado = GameObject.Find(par1Name);
                var par2Seleccionado = GameObject.Find(par2Name);
                Debug.Log("Par correcto");
                numPares--;
                if (numPares > 0) {
                    removeTrigger(par1Name);
                    removeTrigger(par2Name);
                    par1Seleccionado.SetActive(false);
                    par2Seleccionado.SetActive(false);
                    par1 = "";
                    par2 = "";
                    par1Name = "";
                    par2Name = "";
                } else {
                    removeTrigger(par1Name);
                    removeTrigger(par2Name);
                    par1Seleccionado.SetActive(false);
                    par2Seleccionado.SetActive(false);
                    par1 = "";
                    par2 = "";
                    par1Name = "";
                    par2Name = "";
                    correctoPar(true);
                }
            } else {
                Debug.Log("Par incorrecto");
                correctoPar(false);
            }
        }

        Debug.Log("------------------");
        Debug.Log(par1);
        Debug.Log(par2);
        Debug.Log(par1Name);
        Debug.Log(par2Name);
        Debug.Log("------------------");
    }

    public void validarOrden(string objName, int letraSeleccionada) {
        if (letraSeleccionada == letra) {
            var myGameObject = GameObject.Find(objName);
            myGameObject.GetComponent<SpriteRenderer>().sprite = null;
            removeTrigger(objName);
            removeColider(objName);
            letra++;
        } else {
            numLetras = 0;
            letra = 1;
            for (int i = 1; i <= 15; i++) {
                var objeto = "objRespuesta" + i + "C";
                addCollider(objName);
            }
            incorrecto(objName);
        }

        if (numLetras == letra) {
            numLetras = 0;
            letra = 1;
            for (int i = 1; i <= 15; i++) {
                var objeto = "objRespuesta" + i + "C";
                addCollider(objName);
            }
            correctoSimple(objName);
        }
    }

    public void correctoPar(bool res) {
        if (res==true) {
            countPreguntas++;
            Debug.Log("correcto  --- " + preguntas.Length + "CountPreguntas: " + countPreguntas);
            StartCoroutine(esperaSegundos(0.5f, correctoimg));
            score++;
            textoPuntaje.text = score + "";
            if (preguntas.Length > countPreguntas) {
                desactivarPreguntas();
                llamarPreguntas(countPreguntas);
            } else {
                desactivarPreguntas(); ;
                textoPuntajeMarcador.text = score + "";
                scoreFinal.SetActive(true);
                //SceneManager.LoadScene("menuCategorias");
            }
        } else {
            countPreguntas++;
            Debug.Log("incorrecto  --- " + preguntas.Length + "CountPreguntas: " + countPreguntas);
            StartCoroutine(esperaSegundos(0.5f, incorrectoimg));
            if (preguntas.Length > countPreguntas) {
                desactivarPreguntas();
                llamarPreguntas(countPreguntas);
            } else {
                desactivarPreguntas();
                textoPuntajeMarcador.text = score + "";
                //scoreFinal.GetComponentInChildren<Text>().text = score + "";
                scoreFinal.SetActive(true);
                //SceneManager.LoadScene("menuCategorias");
            }
        }
    }

    public void incorrecto(string objName) {
        countPreguntas++;
        Debug.Log("incorrecto  --- " + preguntas.Length + "CountPreguntas: " + countPreguntas);
        removeTrigger(objName);
        StartCoroutine(esperaSegundos(0.5f, incorrectoimg));
        if (preguntas.Length > countPreguntas) {
            desactivarPreguntas();
            llamarPreguntas(countPreguntas);
        } else {
            desactivarPreguntas();
            textoPuntajeMarcador.text = score + "";
            //scoreFinal.GetComponentInChildren<Text>().text = score + "";
            scoreFinal.SetActive(true);
            //SceneManager.LoadScene("menuCategorias");
        }
    }

    public void removeTrigger(string objName) {
        var myGameObject = GameObject.Find(objName);
        myGameObject.GetComponent<EventTrigger>().triggers.RemoveRange(0, myGameObject.GetComponent<EventTrigger>().triggers.Count);
    }

    public void removeColider(string objName) {
        var myGameObject = GameObject.Find(objName);
        myGameObject.GetComponent<Collider>().enabled = false;
    }

    public void addCollider(string objName) {
        var myGameObject = GameObject.Find(objName);
        myGameObject.GetComponent<Collider>().enabled = true;
    }

    public void desactivarPreguntas() {
        preguntasTipos[0].SetActive(false);
        preguntasTipos[1].SetActive(false);
        preguntasTipos[2].SetActive(false);
        preguntasTipos[3].SetActive(false);
    }

    IEnumerator esperaSegundos(float segundos, GameObject objeto) {
        objeto.SetActive(true);
        yield return new WaitForSeconds(segundos);
        objeto.SetActive(false);
    }

    public void salir() {
        SceneManager.LoadScene("menuCategorias");
    }

    public void PosAleatoria(int tipo) {
        //el valor de la variable debe ser acorde al numero de respuestas en la pregunta
        int maxvalues = 0;
        //if para identificar a que pregunta entrará
        if (tipo == 1) {
            print("Entro a Q1");
            Imagen1 = GameObject.FindGameObjectWithTag("objRespuesta1");
            Imagen2 = GameObject.FindGameObjectWithTag("objRespuesta2");
            Imagen3 = GameObject.FindGameObjectWithTag("objRespuesta3");
            Imagen4 = GameObject.FindGameObjectWithTag("objRespuesta4");

            transform1 = Imagen1.GetComponent<Transform>();
            transform2 = Imagen2.GetComponent<Transform>();
            transform3 = Imagen3.GetComponent<Transform>();
            transform4 = Imagen4.GetComponent<Transform>();

            temp1 = transform1.position;
            temp2 = transform2.position;
            temp3 = transform3.position;
            temp4 = transform4.position;

            rotation1 = transform1.rotation.eulerAngles;
            rotation2 = transform2.rotation.eulerAngles;
            rotation3 = transform3.rotation.eulerAngles;
            rotation4 = transform4.rotation.eulerAngles;
            maxvalues = 5;
        }

        if (tipo == 2) {
            print("Entro a Q3");
            Imagen1 = GameObject.FindGameObjectWithTag("Q3.1");
            Imagen2 = GameObject.FindGameObjectWithTag("Q3.2");
            Imagen3 = GameObject.FindGameObjectWithTag("Q3.3");
            Imagen4 = GameObject.FindGameObjectWithTag("Q3.4");

            transform1 = Imagen1.GetComponent<Transform>();
            transform2 = Imagen2.GetComponent<Transform>();
            transform3 = Imagen3.GetComponent<Transform>();
            transform4 = Imagen4.GetComponent<Transform>();

            temp1 = transform1.position;
            temp2 = transform2.position;
            temp3 = transform3.position;
            temp4 = transform4.position;

            rotation1 = transform1.rotation.eulerAngles;
            rotation2 = transform2.rotation.eulerAngles;
            rotation3 = transform3.rotation.eulerAngles;
            rotation4 = transform4.rotation.eulerAngles;
            maxvalues = 5;
        }

        if (tipo == 3) {
            print("Entro a multi");
            Imagen1 = GameObject.FindGameObjectWithTag("MultiR1");
            Imagen2 = GameObject.FindGameObjectWithTag("MultiR2");
            Imagen3 = GameObject.FindGameObjectWithTag("MultiR3");
            Imagen4 = GameObject.FindGameObjectWithTag("MultiR4");
            Imagen5 = GameObject.FindGameObjectWithTag("MultiR5");

            transform1 = Imagen1.GetComponent<Transform>();
            transform2 = Imagen2.GetComponent<Transform>();
            transform3 = Imagen3.GetComponent<Transform>();
            transform4 = Imagen4.GetComponent<Transform>();
            transform5 = Imagen5.GetComponent<Transform>();

            temp1 = transform1.position;
            temp2 = transform2.position;
            temp3 = transform3.position;
            temp4 = transform4.position;
            temp5 = transform5.position;

            rotation1 = transform1.rotation.eulerAngles;
            rotation2 = transform2.rotation.eulerAngles;
            rotation3 = transform3.rotation.eulerAngles;
            rotation4 = transform4.rotation.eulerAngles;
            rotation5 = transform5.rotation.eulerAngles;
            maxvalues = 6;
        }

        if (tipo == 4) {
            print("Entro a par");
            Imagen1 = GameObject.FindGameObjectWithTag("Par1.1");
            Imagen2 = GameObject.FindGameObjectWithTag("Par1.2");
            Imagen3 = GameObject.FindGameObjectWithTag("Par2.1");
            Imagen4 = GameObject.FindGameObjectWithTag("Par2.2");
            Imagen5 = GameObject.FindGameObjectWithTag("Par3.1");
            Imagen6 = GameObject.FindGameObjectWithTag("Par3.2");

            transform1 = Imagen1.GetComponent<Transform>();
            transform2 = Imagen2.GetComponent<Transform>();
            transform3 = Imagen3.GetComponent<Transform>();
            transform4 = Imagen4.GetComponent<Transform>();
            transform5 = Imagen5.GetComponent<Transform>();
            transform6 = Imagen6.GetComponent<Transform>();

            temp1 = transform1.position;
            temp2 = transform2.position;
            temp3 = transform3.position;
            temp4 = transform4.position;
            temp5 = transform5.position;
            temp6 = transform6.position;

            rotation1 = transform1.rotation.eulerAngles;
            rotation2 = transform2.rotation.eulerAngles;
            rotation3 = transform3.rotation.eulerAngles;
            rotation4 = transform4.rotation.eulerAngles;
            rotation5 = transform5.rotation.eulerAngles;
            rotation6 = transform6.rotation.eulerAngles;
            maxvalues = 7;
        }

        //Generar en orden aleatoria los numeros de 1 al 5
        int i = 1;
        HashSet<Int32> numeros = new HashSet<Int32>();
        System.Random ran = new System.Random();

        while (numeros.Count < maxvalues) {
            numeros.Add(ran.Next(maxvalues));
        }

        foreach (int n in numeros) {
            if (n != 0) {
                num = n;
                if (i == 1) {
                    asignarPos(transform1);
                    i++;
                } else if (i == 2) {
                    asignarPos(transform2);
                    i++;
                } else if (i == 3) {
                    asignarPos(transform3);
                    i++;
                } else if (i == 4) {
                    asignarPos(transform4);
                    i++;
                } else if (i == 5) {
                    asignarPos(transform5);
                    i++;

                } else if (i == 6) {
                    asignarPos(transform6);
                    i++;

                }

            }
        }
    }

    void asignarPos(Transform imagen) {
        switch (num) {
            case 1:
                imagen.position = temp1;
                imagen.rotation = Quaternion.Euler(rotation1);

                break;
            case 2:
                imagen.position = temp2;
                imagen.rotation = Quaternion.Euler(rotation2);

                break;
            case 3:
                imagen.position = temp3;
                imagen.rotation = Quaternion.Euler(rotation3);

                break;
            case 4:
                imagen.position = temp4;
                imagen.rotation = Quaternion.Euler(rotation4);

                break;

            case 5:
                imagen.position = temp5;
                imagen.rotation = Quaternion.Euler(rotation5);

                break;
            case 6:
                imagen.position = temp6;
                imagen.rotation = Quaternion.Euler(rotation6);

                break;
        }
    }

}
