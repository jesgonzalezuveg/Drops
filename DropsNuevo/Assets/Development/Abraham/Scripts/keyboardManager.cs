using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyboardManager : MonoBehaviour {

    GameObject userInput;                   ///< Text almacena los datos de correo o matricula de usuario
    GameObject passInput;                   ///< Text almacena los datos de contraseña del usuario
    GameObject teclasLetras;                ///< Conjunto botones que simulan las teclas de un teclado
    GameObject teclasOtros;                 ///< Conjunto botones que simulan las teclas especiales de un teclado
    string password = "12345";              ///< string que contenera la verdadera contraseña, ya que el texto que aparecera en pantalla solo son asteriscos

    bool isMinusculas = false;              ///< Bandera detecta si esta o no en mayusculas el teclado
    bool btnOtros = true;                   ///< Bandera detecta si estan activadas o no las teclas especiales
    bool focusTxtUsuario = true;            ///< Bandera nos dice cual Input esta usando

    public Text mensaje;                    ///< mensaje mensaje que muestra si se ingreso o no el usuario y contraseña de manera correcta


    /** Función que se llama al inicio de la escena 
     * Inicia las referencias a lo GO
     */
    void Start() {
        userInput = GameObject.Find("inputMatricula2");
        passInput = GameObject.Find("inputContraseña2");
        teclasLetras = GameObject.Find("tecladoLetras");
        teclasOtros = GameObject.Find("tecladoEspecial");
        teclasOtros.SetActive(false);
        mensaje.text = "";
    }

    /** Función que se manda llamar al hacer click en una tecla del teclado
     * @param key, caracter que escribira en el input que se tenga seleccionado
     */
    public void GetKeyboardInput(string key) {
        //if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
        //    return;
        //}
        if (isMinusculas) {
            key = key.ToLower();
        }
        if (focusTxtUsuario) {
            userInput.GetComponent<Text>().text += key;
        } else {
            passInput.GetComponent<Text>().text += '*';
            password += key;
        }
    }

    /** Función que elimina el ultimo caracter de el input que se tiene seleccionado
     * @param
     */
    public void DeleteChar() {
        //if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
        //    return;
        //}
        if (focusTxtUsuario) {
            if (userInput.GetComponent<Text>().text != "") {
                userInput.GetComponent<Text>().text = userInput.GetComponent<Text>().text.Remove(userInput.GetComponent<Text>().text.Length - 1); ;
            }
        } else {
            if (passInput.GetComponent<Text>().text != "") {
                passInput.GetComponent<Text>().text = passInput.GetComponent<Text>().text.Remove(passInput.GetComponent<Text>().text.Length - 1);
                password = password.Remove(password.Length - 1);
            }
        }
    }

    /** Función que activa o desactiva las mayusculas
     * @param
     */
    public void BtnMinusculas() {
        //if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
        //    return;
        //}
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
        //if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
        //    return;
        //}
        btnOtros = !btnOtros;
        teclasOtros.SetActive(!btnOtros);
    }


    /** Función que activa el input de matricula o correo usuario
     * @param
     */
    public void ClickTxtUsuario() {
        //if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
        //    return;
        //}
        focusTxtUsuario = true;
        userInput.GetComponentInParent<Image>().color = new Color(1, 1, 0.8f);
        passInput.GetComponentInParent<Image>().color = Color.white;
    }

    /** Función que activa el input de contraseña
     * @param
     */
    public void ClickTxtPass() {
        //if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
        //    return;
        //}
        focusTxtUsuario = false;
        userInput.GetComponentInParent<Image>().color = Color.white;
        passInput.GetComponentInParent<Image>().color = new Color(1, 1, 0.8f);
    }


    /** Función que se activa cuando el usuario da click en el boton continuar
     * @param
     */
    public void login() {
        //if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
        //    return;
        //}
        GameObject.Find("Player").GetComponent<PlayerManager>().setMensaje(true, "Cargando....");
        StartCoroutine(webServiceUsuario.getUserData(userInput.GetComponentInChildren<Text>().text, password));
    }
}
