using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;
using UnityEngine.Events;

public class pairingCode : MonoBehaviour
{
    string code;
    string code2;
    public static String status;
    public static int valCodigoSii;
    private int salir;
    private int countFrames;
    private int cargaCodigo;
    UnityEvent listenerCode= new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        code = "";
        code2 = "";
        status = "5";
        valCodigoSii = 3;
        countFrames = 0;
        cargaCodigo = 0;
        salir = 0;
        code = "aLRB52";
    }

    // Update is called once per frame
    void Update()
    {
        if (countFrames>=30) {
            if (salir == 0) {
                StartCoroutine(WebServiceCodigo.obtenerCodigo(code, 1));
                if (valCodigoSii == 1) {
                    pairingCode.valCodigoSii = 3;
                    Debug.Log("El código ya exixte");
                    code = generateCode();
                } else if (valCodigoSii == 0) {
                    pairingCode.valCodigoSii = 3;
                    Debug.Log("El código no exixte");
                    guardarCodigoSqlite(code);
                    StartCoroutine(WebServiceCodigo.insertarCodigo(code));
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = code2;
                    listenerCode.AddListener(emparejarCodigo);
                    salir = 1;
                } else {
                    Debug.Log("Esperando Respuesta del Web Service");
                }
            }

            // Iniciar Listener
            if (salir == 1) {
                if (status == "2") {
                    Debug.Log("Quitting");
                    int res = editarCodigoSqlite(code);
                    if (res == 1) {
                        Debug.Log("Se modifico el status");
                        listenerCode.RemoveListener(emparejarCodigo);
                        salir = 2;
                        GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "Codigo emparejado";
                    } else {
                        Debug.Log("No se modifico el status");
                    }
                } else {
                    listenerCode.Invoke();
                }
            }

            countFrames = 0;
        } else {
            countFrames++;
            cargaCodigo++; if (salir==0) {
                if (cargaCodigo == 1) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "X";
                } else if (cargaCodigo == 2) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XD";
                } else if (cargaCodigo == 3) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XDX";
                } else if (cargaCodigo == 4) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XDXD";
                } else if (cargaCodigo == 5) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XDXDX";
                } else if (cargaCodigo == 6) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XDXDXD";
                    cargaCodigo = 0;
                }
            }
        }
    }

    void emparejarCodigo() {
        //Output message to the console
        Debug.Log(status);
        Debug.Log(valCodigoSii);
        StartCoroutine(WebServiceCodigo.obtenerCodigo(code, 2));
    }

    /** Funcion que sirve para generar un código de 8 caracteres de manera aleatoria
   *
   *@param  chars Lista de caracteres 
   *@param  stringChars Contenedor de los 6 caracteres que contendra el codigo
   *@param  random Funcion para elección aleatoria
   *@param  finalString Código obtentido
   *@method conexionDB.alterGeneral metodo que recibe una query ya sea para realizar un insert, update o delete
   **/
    public string generateCode() {
        code2 = "";
        var chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var stringChars = new char[6];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++) {
            stringChars[i] = chars[random.Next(chars.Length)];
            if (i == 2) {
                code2 = code2 + stringChars[i] + "-";
            } else {
                code2 = code2 + stringChars[i];
            }
        }

        var finalString = new string(stringChars);

        return finalString;
    }

    private int guardarCodigoSqlite(string codigo) {
        string query = "INSERT INTO codigo (descripcion, status, fechaRegistro, fechaModificacion) VALUES ('" + codigo + "', 1, datetime(), datetime())";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    private int editarCodigoSqlite(string codigo) {
        string query = "UPDATE codigo SET status = 2, fechaModificacion = datetime() WHERE descripcion = '" + codigo + "' AND status = 1";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    private int existeCodigoSqlite(string codigo) {
        string query = "SELECT * FROM codigo WHERE descripcion = '"+ codigo +"' AND status = 1";
        var result = conexionDB.selectGeneral(query);

        if (result != "0") {
            return 1;
        } else {
            return 0;
        }
    }
}
