using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class keyboardManager : MonoBehaviour {

    GameObject userInput;                   //< InputField que almacena los datos de correo o matricula de usuario
    GameObject passInput;                   //< InputField que almacena los datos de contraseña del usuario
    GameObject teclasLetras;                //< Conjunto de botones que simulan las teclas de un teclado
    GameObject teclasOtros;                 //< Conjunto de botones que simulan las teclas especiales de un teclado

    public static string sesion;

    bool isMinusculas = false;              //< Bandera que detecta si esta o no en mayusculas el teclado
    bool btnOtros = true;                   //< Bandera que detecta si estan activadas o no las teclas especiales
    bool focusTxtUsuario = true;            //< Bandera que nos dice cual Input esta usando
    bool clickLogin = false;

    /**
     * Función que se llama al inicio de la escena 
     *
     */ 
    void Start() {
        userInput = GameObject.Find("inputMatricula");
        passInput = GameObject.Find("inputContraseña");
        teclasLetras = GameObject.Find("tecladoLetras");
        teclasOtros = GameObject.Find("tecladoEspecial");
        teclasOtros.SetActive(false);
    }

    /**
     * Función que se manda llamar al hacer click en una tecla del teclado
     * @param key, caracter que escribira en el input que se tenga seleccionado
     */
    public void GetKeyboardInput(string key) {
        if (isMinusculas) {
            key = key.ToLower();
        }
        if (focusTxtUsuario) {
            userInput.GetComponent<InputField>().text += key;
        } else {
            passInput.GetComponent<InputField>().text += key;
        }
    }

    /**
     * Función que elimina el ultimo caracter de el input que se tiene seleccionado
     * @param
     */ 
    public void DeleteChar() {
        if (focusTxtUsuario) {
            if (userInput.GetComponent<InputField>().text != "") {
                userInput.GetComponent<InputField>().text = userInput.GetComponent<InputField>().text.Remove(userInput.GetComponent<InputField>().text.Length - 1); ;
            }
        } else {
            if (passInput.GetComponent<InputField>().text != "") {
                passInput.GetComponent<InputField>().text = passInput.GetComponent<InputField>().text.Remove(passInput.GetComponent<InputField>().text.Length - 1);
            }
        }

    }

    /**
     * Función que activa o desactiva las mayusculas
     * @param
     */
    public void BtnMinusculas() {
        isMinusculas = !isMinusculas;

        if (isMinusculas) {
            foreach (Text t in teclasLetras.GetComponentsInChildren<Text>()) {
                t.text = t.text.ToLower();
            }
        } else {
            foreach (Text t in teclasLetras.GetComponentsInChildren<Text>()) {
                t.text = t.text.ToUpper();
            }
        }
    }

    /**
     * Función que activa o desactiva los caracteres especiales
     * @param
     */ 
    public void BtnOtros() {
        btnOtros = !btnOtros;
        teclasOtros.SetActive(!btnOtros);
    }


    /**
     * Función que activa el input de matricula o correo usuario
     * @param
     */ 
    public void ClickTxtUsuario() {
        focusTxtUsuario = true;
        userInput.GetComponent<Image>().color = new Color(1, 1, 0.8f);
        passInput.GetComponent<Image>().color = Color.white;
        print("usuario");
    }

    /**
     * Función que activa el input de contraseña
     * @param
     */
    public void ClickTxtPass() {
        focusTxtUsuario = false;
        userInput.GetComponent<Image>().color = Color.white;
        passInput.GetComponent<Image>().color = new Color(1, 1, 0.8f);
        print("pass");
    }


    /**
     * Función que se activa cuando el usuario da click en el boton continuar
     * @param
     */
    public void DemoLogin() {
        StartCoroutine(webServiceLogin.getUserData(userInput.GetComponentInChildren<Text>().text, passInput.GetComponentInChildren<Text>().text));
        clickLogin = true;
    }
}
