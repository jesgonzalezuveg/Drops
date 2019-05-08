using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyboardManager : MonoBehaviour {

    GameObject teclasLetras;                ///< Conjunto botones que simulan las teclas de un teclado
    GameObject teclasOtros;                 ///< Conjunto botones que simulan las teclas especiales de un teclado
    string password = "";                   ///< string que contenera la verdadera contraseña, ya que el texto que aparecera en pantalla solo son asteriscos
    string usuario = "";

    bool isMinusculas = false;              ///< Bandera detecta si esta o no en mayusculas el teclado
    bool btnOtros = true;                   ///< Bandera detecta si estan activadas o no las teclas especiales

    public GameObject[] inputs;

    GameObject inputActivo;
    public bool isPasswordInputActive;

    /** Función que se llama al inicio de la escena 
     * Inicia las referencias a lo GO
     */
    void Start() {
        inputActivo = inputs[0];
        teclasLetras = GameObject.Find("tecladoLetras");
        teclasOtros = GameObject.Find("tecladoEspecial");
        teclasOtros.SetActive(false);
    }

    public void setIsPasswordInputActive(bool isPassword) {
        isPasswordInputActive = isPassword;
    }

    public void setUsuario(string usuario) {
        this.usuario = usuario;
    }

    /** Función que se manda llamar al hacer click en una tecla del teclado
     * @param key, caracter que escribira en el input que se tenga seleccionado
     */
    public void GetKeyboardInput(string key) {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        if (isMinusculas) {
            key = key.ToLower();
        }
        if (!isPasswordInputActive) {
            usuario += key;
            inputActivo.GetComponentInChildren<Text>().text = "";
            inputActivo.GetComponentInChildren<Text>().text = usuario;
        } else {
            password += key;
            inputActivo.GetComponentInChildren<Text>().text = "";
            for (int i = 0; i < password.Length;i++) {
                inputActivo.GetComponentInChildren<Text>().text += "*";
            }
        }
    }

    /** Función que elimina el ultimo caracter de el input que se tiene seleccionado
     * @param
     */
    public void DeleteChar() {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        if (inputActivo.GetComponentInChildren<Text>().text != "") {
            inputActivo.GetComponentInChildren<Text>().text = inputActivo.GetComponentInChildren<Text>().text.Remove(inputActivo.GetComponentInChildren<Text>().text.Length - 1); ;
            if (isPasswordInputActive) {
                password = password.Remove(password.Length - 1);
            } else {
                usuario = usuario.Remove(usuario.Length - 1);
            }
        }
    }

    /** Función que activa o desactiva las mayusculas
     * @param
     */
    public void BtnMinusculas() {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
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

    /** Función que activa o desactiva los caracteres especiales
     * @param
     */
    public void BtnOtros() {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        btnOtros = !btnOtros;
        teclasOtros.SetActive(!btnOtros);
    }


    /** Función que activa el input de matricula o correo usuario
     * @param
     */
    public void clickInput(GameObject input) {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        inputActivo.GetComponent<Image>().color = Color.white;
        inputActivo = input;
        inputActivo.GetComponent<Image>().color = new Color(1, 1, 0.8f);
    }


    /** Función que se activa cuando el usuario da click en el boton continuar
     * @param
     */
    public void login() {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        GameObject.Find("Player").GetComponent<PlayerManager>().setMensaje(true, "Cargando....");
        StartCoroutine(webServiceUsuario.getUserData(inputs[0].GetComponentInChildren<Text>().text, password));
    }
}
