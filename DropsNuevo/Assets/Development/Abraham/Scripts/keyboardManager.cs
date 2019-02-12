using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class keyboardManager : MonoBehaviour {

    GameObject userInput;
    GameObject passInput;
    GameObject teclasLetras;
    GameObject teclasOtros;

    bool isMinusculas = false;
    bool btnOtros = true;
    public bool focusTxtUsuario = true;

    // Use this for initialization
    void Start() {
        userInput = GameObject.Find("inputMatricula");
        passInput = GameObject.Find("inputContraseña");
        teclasLetras = GameObject.Find("tecladoLetras");
        teclasOtros = GameObject.Find("tecladoEspecial");
        teclasOtros.SetActive(false);
    }

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
        userInput.GetComponent<Image>().color = new Color(1, 1, 0.8f);
        passInput.GetComponent<Image>().color = Color.white;
        print("usuario");
    }

    public void ClickTxtPass() {
        focusTxtUsuario = false;
        userInput.GetComponent<Image>().color = Color.white;
        passInput.GetComponent<Image>().color = new Color(1, 1, 0.8f);
        print("pass");
    }

    public void DemoLogin() {
        Debug.Log("Usuario: " + userInput.GetComponentInChildren<Text>().text + "\nContraseña: " + passInput.GetComponentInChildren<Text>().text);
        SceneManager.LoadScene(1);
    }
}
