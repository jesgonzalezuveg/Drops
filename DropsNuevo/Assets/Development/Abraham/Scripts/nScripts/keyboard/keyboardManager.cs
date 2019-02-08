using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class keyboardManager : MonoBehaviour {

    public GameObject display;
    public GameObject passDisplay;
    public GameObject teclasLetras;
    public GameObject teclasOtros;

    bool isMinusculas = false;
    bool btnOtros = true;
    public bool focusTxtUsuario = true;

    // Use this for initialization
    void Start() {
        display = GameObject.Find("inputMatricula");
        passDisplay = GameObject.Find("inputContraseña");
        teclasLetras = GameObject.Find("tecladoLetras");
        teclasOtros = GameObject.Find("tecladoEspecial");
        teclasOtros.SetActive(false);
    }

    public void GetKeyboardInput(string key) {
        if (isMinusculas) {
            key = key.ToLower();
        }

        if (focusTxtUsuario) {
            display.GetComponent<InputField>().text += key;
        } else {
            passDisplay.GetComponent<InputField>().text += key;
        }
    }

    public void DeleteChar() {
        if (focusTxtUsuario) {
            if (display.GetComponent<InputField>().text != "") {
                display.GetComponent<InputField>().text = display.GetComponent<InputField>().text.Remove(display.GetComponent<InputField>().text.Length - 1); ;
            }
        } else {
            if (passDisplay.GetComponent<InputField>().text != "") {
                passDisplay.GetComponent<InputField>().text = passDisplay.GetComponent<InputField>().text.Remove(passDisplay.GetComponent<InputField>().text.Length - 1);
            }
        }

    }

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

    public void BtnOtros() {
        btnOtros = !btnOtros;
        teclasOtros.SetActive(!btnOtros);
    }

    public void ClickTxtUsuario() {
        focusTxtUsuario = true;
        display.GetComponent<Image>().color = new Color(1, 1, 0.8f);
        passDisplay.GetComponent<Image>().color = Color.white;
        print("usuario");
    }

    public void ClickTxtPass() {
        focusTxtUsuario = false;
        display.GetComponent<Image>().color = Color.white;
        passDisplay.GetComponent<Image>().color = new Color(1, 1, 0.8f);
        print("pass");
    }

    public void DemoLogin() {
        Debug.Log("Usuario: " + display.GetComponentInChildren<Text>().text + "\nContraseña: " + passDisplay.GetComponentInChildren<Text>().text);
        SceneManager.LoadScene(1);
    }
}
