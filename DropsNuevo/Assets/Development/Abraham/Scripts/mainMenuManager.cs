using UnityEngine;
using System.Collections;

public class mainMenuManager : MonoBehaviour {

    public GameObject[] vistas;
    GameObject vistaActiva;

    public void Start() {
        vistaActiva = vistas[0];
    }

    /**
     * Función que se ejecuta al pulzar el boton Jugar
     * 
     */
    public void cambiarVista(int vista) {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            return;
        }
        vistaActiva.SetActive(false);
        vistas[vista].SetActive(true);
        vistaActiva = vistas[vista];
    }

    /**
     * Coroutine que espera a que termine de reprocucir el audio de click de los botones
     * para poder ir a la nueva escena
     */
    IEnumerator loadScene(string scene) {
        yield return new WaitUntil(() => this.GetComponent<AudioSource>().isPlaying == false);
        StartCoroutine(GameObject.Find("AppManager").GetComponent<appManager>().cambiarEscena(scene,"mainMenu"));
    }
}
