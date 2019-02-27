﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class CursoManager : MonoBehaviour {

    public Text textoPuntaje;
    public Text textoPuntajeMarcador;
    public GameObject correctoimg;
    public GameObject incorrectoimg;
    public GameObject scoreFinal;

    int num;
    Vector3 temp1, temp2, temp3, temp4, temp5, temp6;
    Vector3 rotation1, rotation2, rotation3, rotation4, rotation5, rotation6;
    Transform transform1, transform2, transform3, transform4, transform5, transform6;
    GameObject Imagen1, Imagen2, Imagen3, Imagen4, Imagen5, Imagen6;

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
        scoreFinal.SetActive(false);
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
        PosAleatoria(1);
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
        StartCoroutine(esperaSegundos(0.5f, correctoimg));
        score++;
        textoPuntaje.text = score + "";
        if (preguntas.Length > countPreguntas) {
            llamarPreguntas(countPreguntas);
        } else {
            GameObject.Find("Pregunta1").SetActive(false);
            textoPuntajeMarcador.text = score + "";
            scoreFinal.SetActive(true);
            //SceneManager.LoadScene("menuCategorias");
        }
    }

    public void incorrecto() {
        Debug.Log("incorrecto  --- " + preguntas.Length + "CountPreguntas: " + countPreguntas);
        StartCoroutine(esperaSegundos(0.5f, incorrectoimg));
        if (preguntas.Length > countPreguntas) {
            llamarPreguntas(countPreguntas);
        } else {
            GameObject.Find("Pregunta1").SetActive(false);
            textoPuntajeMarcador.text = score + "";
            //scoreFinal.GetComponentInChildren<Text>().text = score + "";
            scoreFinal.SetActive(true);
            //SceneManager.LoadScene("menuCategorias");
        }
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
