using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuManager : MonoBehaviour {

    private void Start() {
        /*if (SystemInfo.deviceType == oculus) {
            Esconder boton de facebook
        }*/
    }

    /**
     * Función que se ejecuta al pulzar el boton login con pair code
     * 
     */
    public void pairCode() {
        SceneManager.LoadScene("sampleScene");
    }

    /**
     * Función que se ejecuta al pulzar el boton login
     * 
     */
    public void login() {
        SceneManager.LoadScene("login");
    }

    /**
     * Función que se ejecuta al pulzar el boton login invitado
     * 
     */
    public void invitado() {
        SceneManager.LoadScene("menuCategorias");
    }
}
