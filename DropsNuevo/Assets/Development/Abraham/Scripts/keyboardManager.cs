﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyboardManager : MonoBehaviour {

    GameObject teclasLetras;                ///< Conjunto botones que simulan las teclas de un teclado
    GameObject teclasOtros;                 ///< Conjunto botones que simulan las teclas especiales de un teclado
    string usuario = "";
    string nombre = "";
    string password = "";                   ///< string que contenera la verdadera contraseña, ya que el texto que aparecera en pantalla solo son asteriscos
    string password2 = "";

    bool isMinusculas = false;              ///< Bandera detecta si esta o no en mayusculas el teclado
    bool btnOtros = true;                   ///< Bandera detecta si estan activadas o no las teclas especiales

    public GameObject[] inputs;

    GameObject inputActivo;
    public bool isPasswordInputActive;
    public bool isSecondPassword;
    public bool isNombre;
    public bool isUsuario;

    /** Función que se llama al inicio de la escena 
     * Inicia las referencias a lo GO
     */
    void Start() {
        inputActivo = inputs[0];
        teclasLetras = GameObject.Find("tecladoLetras");
        teclasOtros = GameObject.Find("tecladoEspecial");
        teclasOtros.SetActive(false);
    }
    private void Update() {
        if (isUsuario) {
            if (usuario.Length <= 0) {
                inputActivo.GetComponentInChildren<Text>().text = "Correo o usuario";
            }
        }
        if (isNombre) {
            if (nombre.Length <= 0) {
                inputActivo.GetComponentInChildren<Text>().text = "Nombre";
            }
        }
        if (isPasswordInputActive) {
            if (password.Length <= 0) {
                inputActivo.GetComponentInChildren<Text>().text = "Ingresa tu contraseña";
            }
        }
        if (isSecondPassword) {
            if (password2.Length <= 0) {
                inputActivo.GetComponentInChildren<Text>().text = "Confirma tu contraseña";
            }
        }
    }

    public void setUsuario(string usuario) {
        this.usuario = usuario;
    }

    public void setNombre(string nombre) {
        this.nombre = nombre;
    }

    public void setPassword(string pass) {
        password = pass;
    }

    public void setPassword2(string pass2) {
        password2 = pass2;
    }

    public void setIsUsuario(bool isUsuario) {
        clearAll();
        this.isUsuario = isUsuario;
    }

    public void setIsName(bool isNombre) {
        clearAll();
        this.isNombre = isNombre;
    }

    public void setIsPasswordInputActive(bool isPassword) {
        clearAll();
        isPasswordInputActive = isPassword;
    }

    public void setIsSecondPassword(bool isSecondPassword) {
        clearAll();
        this.isSecondPassword = isSecondPassword;
    }

    public void clearAll() {
        isNombre = false;
        isUsuario = false;
        isPasswordInputActive = false;
        isSecondPassword = false;
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
        if (isUsuario) {
            usuario += key;
            inputActivo.GetComponentInChildren<Text>().text = "";
            inputActivo.GetComponentInChildren<Text>().text = usuario;
        }
        if (isNombre) {
            nombre += key;
            inputActivo.GetComponentInChildren<Text>().text = "";
            inputActivo.GetComponentInChildren<Text>().text = nombre;
        }
        if (isPasswordInputActive) {
            password += key;
            inputActivo.GetComponentInChildren<Text>().text = "";
            for (int i = 0; i < password.Length; i++) {
                inputActivo.GetComponentInChildren<Text>().text += "*";
            }
        }
        if (isSecondPassword) {
            password2 += key;
            inputActivo.GetComponentInChildren<Text>().text = "";
            for (int i = 0; i < password2.Length; i++) {
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
            inputActivo.GetComponentInChildren<Text>().text = inputActivo.GetComponentInChildren<Text>().text.Remove(inputActivo.GetComponentInChildren<Text>().text.Length - 1);
            if (isUsuario) {
                if (usuario.Length > 0) {
                    usuario = usuario.Remove(usuario.Length - 1);
                }
                if (usuario.Length <= 0) {
                    inputActivo.GetComponentInChildren<Text>().text = "Correo o usuario";
                }
            }
            if (isNombre) {
                if (nombre.Length > 0) {
                    nombre = nombre.Remove(nombre.Length - 1);
                }
                if (nombre.Length <= 0) {
                    inputActivo.GetComponentInChildren<Text>().text = "Nombre";
                }
            }
            if (isPasswordInputActive) {
                if (password.Length > 0) {
                    password = password.Remove(password.Length - 1);
                }
                if (password.Length <= 0) {
                    inputActivo.GetComponentInChildren<Text>().text = "Ingresa tu contraseña";
                }
            }
            if (isSecondPassword) {
                if (password2.Length > 0) {
                    password2 = password2.Remove(password2.Length - 1);
                }
                if (password2.Length <= 0) {
                    inputActivo.GetComponentInChildren<Text>().text = "Confirma tu contraseña";
                }
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
        Debug.Log("Buscar en BD Local");

        // Consultar en BD local (sqlite)
        var usuario = webServiceUsuario.consultarLoginUsuarioSqLite(inputs[0].GetComponentInChildren<Text>().text, password);
        if (usuario != null) {
            if (usuario.password != "") {
                Debug.Log("usuario no es null");
                GameObject.FindObjectOfType<appManager>().setNombre(usuario.nombre);
                GameObject.FindObjectOfType<appManager>().setUsuario(usuario.usuario);
                GameObject.FindObjectOfType<appManager>().setGradoEstudios(usuario.programa);
                GameObject.FindObjectOfType<appManager>().setImagen(usuario.imagen);
                StartCoroutine(GameObject.FindObjectOfType<appManager>().cambiarEscena("menuCategorias", "mainMenu"));
            } else {
                GameObject.Find("Player").GetComponent<PlayerManager>().setMensaje(false, "");
                GameObject.Find("Mascota").GetComponentInChildren<Text>().text = "Contraseña incorrecta";
            }
        } else {
            // Consultar en SII ambas BD
            StartCoroutine(webServiceUsuario.getUserData(inputs[0].GetComponentInChildren<Text>().text, password));
        }
    }

    public void registrar() {
        if (usuario == "" || nombre == "" || password == "") {
            GameObject.Find("Mascota").GetComponentInChildren<Text>().text = "Faltan campos por llenar";
            return;
        }
        if (usuario.Length < 8 || usuario.Length > 35) {
            GameObject.Find("Mascota").GetComponentInChildren<Text>().text = "El usuario debe tener entre 8 y 35 caracteres";
            return;
        }
        string[] charAEliminar = { " ", "!", "\"", "#", "$", "%", "&", "\'", "(", ")", "*", "+", ",", "-", ".", "/", ":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}","ñ","Ñ" };
        foreach (string caracter in charAEliminar) {
            string charPosition = caracter + "";
            if (password.Contains(charPosition)) {
                GameObject.Find("Mascota").GetComponentInChildren<Text>().text = "La contraseña contiene caracteres invalidos";
                return;
            }
        }
        if (password.Length <8 || password.Length > 50) {
            GameObject.Find("Mascota").GetComponentInChildren<Text>().text = "La contraseña debe tener entre 8 y 50 caracteres";
            return;
        }
        if (password != password2) {
            GameObject.Find("Mascota").GetComponentInChildren<Text>().text = "Las contraseñas no coinciden";
            return;
        }

        foreach (var input in inputs) {
            input.GetComponentInChildren<Text>().text = "";
        }
        usuario = usuario.ToLower();
        StartCoroutine(webServiceUsuario.insertUsuario(usuario,nombre,password));
    }
}
