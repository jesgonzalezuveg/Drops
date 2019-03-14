using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class paquetesManager : MonoBehaviour {

    private appManager manager;             ///< manager referencia al componente appManager
    public Image imagen;                    ///< imagen referencia a la imagen que contendra la imagen del usuario
    public GameObject textoPaquetes;        ///< textoPaquetes referencia al objeto que se muestra u oculta dependiendo si existen paquetes por descargar
    public GameObject listaPaquetes;        ///< listaPaquetes referencia al objeto que contiene los paquetes ya instalados
    public GameObject listaPaquetesNuevos;  ///< listaPaquetesNuevos referencia al objeto que contiene los paquetes nuevos por descargar
    public GameObject configuracionModal;   ///< configuracionModal referencia al modal de configuracion de curso
    public GameObject scrollBar;            ///< scrollBar referencia al scrollbar para seleccionar el numero maximo de preguntas por curso
    private bool bandera = true;            ///< bandera bandera que valida si ya se obtuo la imagen del usuario

    /**
     * Funcion que se manda llamar al inicio de la escena (frame 0)
     * Inicializa la referencia del appManager
     */
    private void Awake() {
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
    }

    /**
     * Funcion que se manda llamar al inicio de la escena (frame 1)
     * set numeroPreguntas al que el usuario ya habia seleccionado
     * Oculta el modal de configuracion
     * obtiene los datos de la BD local
     */
    private void Start() {
        if (manager.isFirstLogin) {
            manager.isFirstLogin = false;
        }
        scrollBar.GetComponent<Slider>().value = manager.numeroPreguntas;
        setVisibleModal(false);
        manager.setBanderas(true);
        if (manager.isOnline) {
            StartCoroutine(webServiceCategoria.getCategorias());
            StartCoroutine(webServicePaquetes.getPaquetes());
            StartCoroutine(webServiceAcciones.getAcciones());
            StartCoroutine(webServiceEjercicio.getEjercicios());
        } else {
            var paquetesLocales = webServicePaquetes.getPaquetesSqLite();
            if (paquetesLocales != null) {
                manager.setPaquetes(paquetesLocales.paquete);
            } else {
                fillEmpty();
            }
        }
    }


    /**
     * Funcion que se manda llamar cada frame
     * Si no existe imagen de usuario la inserta
     * verifica si existen paquetes nuevos para descargar
     */
    private void Update() {
        if (manager.getImagen() != "" && bandera) {
            StartCoroutine(getUserImg());
            bandera = false;
        }

        if (listaPaquetesNuevos.transform.childCount <= 0) {
            GameObject.Find("ListaPaquetes").GetComponent<paquetesManager>().textoPaquetes.SetActive(true);
        } else {
            GameObject.Find("ListaPaquetes").GetComponent<paquetesManager>().textoPaquetes.SetActive(false);
        }
    }

    /**
     * Coroutine que obtiene la imagen del usuario
     * no importa si inicio con Facebook o es usuario UVEG
     */
    IEnumerator getUserImg() {
        if (manager.isOnline) {
            if (manager.GetComponent<appManager>().getImagen() != "") {
                string path = manager.GetComponent<appManager>().getImagen().Split('/')[manager.GetComponent<appManager>().getImagen().Split('/').Length - 1];
                if (File.Exists(Application.persistentDataPath + path)) {
                    byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
                    Texture2D texture = new Texture2D(8, 8);
                    texture.LoadImage(byteArray);
                    Rect rec = new Rect(0, 0, texture.width, texture.height);
                    var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
                    imagen.sprite = sprite;
                } else {
                    WWW www = new WWW(manager.GetComponent<appManager>().getImagen());
                    yield return www;
                    Texture2D texture = www.texture;
                    byte[] bytes = texture.EncodeToPNG();
                    File.WriteAllBytes(Application.persistentDataPath + path, bytes);
                    Rect rec = new Rect(0, 0, texture.width, texture.height);
                    var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
                    imagen.sprite = sprite;
                }
            }
        }
    }

    /**
     * Funcion que se manda llamar al tener un paquete listo para jugar
     * Inserta la tarjeta fichaPaqueteJugar en listaPaquetes
     * @pack paqueteData estructura que tiene los datos del paquete a jugar
     */
    public void newCardJugar(webServicePaquetes.paqueteData pack) {
        var fichaPaquete = Instantiate(Resources.Load("fichaPaqueteJugar") as GameObject);
        fichaPaquete.name = "fichaPack" + pack.id;
        StartCoroutine(llenarFicha(fichaPaquete, pack.urlImagen));
        fichaPaquete.transform.SetParent(listaPaquetes.transform);
        fichaPaquete.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        fichaPaquete.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        fichaPaquete.GetComponent<RectTransform>().localScale = new Vector3(1.33f, 1.33f, 1.33f);
        fichaPaquete.GetComponent<packManager>().paquete = pack;
    }

    /**
     * Funcion que se manda llamar al tener un paquete listo para jugar aunque es posible actualizarlo
     * Inserta la tarjeta fichaPaqueteActualizar en listaPaquetes
     * @pack paqueteData estructura que tiene los datos del paquete a jugar
     */
    public void newCardActualizar(webServicePaquetes.paqueteData pack) {
        var fichaPaquete = Instantiate(Resources.Load("fichaPaqueteActualizar") as GameObject);
        fichaPaquete.name = "fichaPack" + pack.id;
        StartCoroutine(llenarFicha(fichaPaquete, pack.urlImagen));
        fichaPaquete.transform.SetParent(listaPaquetes.transform);
        fichaPaquete.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        fichaPaquete.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        fichaPaquete.GetComponent<RectTransform>().localScale = new Vector3(1.33f, 1.33f, 1.33f);
        fichaPaquete.GetComponent<packManager>().paquete = pack;
    }

    /**
     * Funcion que se manda llamar al tener un paquete sin descargar
     * Inserta la tarjeta fichaPaquete en listaPaquetesNuevos
     * @pack paqueteData estructura que tiene los datos del paquete a descargar
     */
    public void newCardDescarga(webServicePaquetes.paqueteData pack) {
        var fichaPaquete = Instantiate(Resources.Load("fichaPaquete") as GameObject);
        fichaPaquete.name = "fichaPack" + pack.id;
        StartCoroutine(llenarFicha(fichaPaquete, pack.descripcion, pack.urlImagen));
        fichaPaquete.transform.SetParent(listaPaquetesNuevos.transform);
        fichaPaquete.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        fichaPaquete.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        fichaPaquete.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        fichaPaquete.GetComponent<packManager>().paquete = pack;
    }

    /**
     * Funcion que se manda llamar cuando termina de insertar todas las 
     * tarjetas de paquetes en sus respectivos lugares
     * Llena los espacios vacios con placeHoldrs para que la cuadricula se vea bien.
     * (Solo se utiliza en en objeto listaPaquetes)
     */
    public void fillEmpty() {
        var hijos = listaPaquetes.GetComponentsInChildren<packManager>(true);
        if (hijos.Length <= 5) {
            var obj = Instantiate(Resources.Load("placeHolder")) as GameObject;
            obj.transform.position = new Vector3(0, 0, 0);
            obj.transform.SetParent(listaPaquetes.transform);
            obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            obj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
            fillEmpty();
        }else if (hijos.Length % 2 == 1) {
            Debug.Log("Insertar otro");
            var obj = Instantiate(Resources.Load("placeHolder")) as GameObject;
            obj.transform.position = new Vector3(0, 0, 0);
            obj.transform.SetParent(listaPaquetes.transform);
            obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            obj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        }
        listaPaquetes.GetComponent<gridScrollLayout>().bandera = true;
        listaPaquetesNuevos.GetComponent<gridScrollLayout>().bandera = true;
    }

    /**
     * Funcion que se manda llamar al hacer click en el btnConfiguracion
     * Apaga el raycastTarget de los botones de paquetes
     * Muestra u oculta el modalConfiguracion
     * @isVisible bool que se encarga de activar o desactivar el modal
     */
    public void setVisibleModal(bool isVisible) {
        configuracionModal.SetActive(isVisible);
        if (isVisible == false) {
            gameObject.GetComponent<GraphicRaycaster>().enabled = true;
            manager.GetComponent<appManager>().numeroPreguntas = scrollBar.GetComponent<Slider>().value;
        } else {
            gameObject.GetComponent<GraphicRaycaster>().enabled = false;
        }
    }

    /**
     * Coroutine que llena los datos de las tarjetas que se insertan dependiendo el paquete
     * @ficha referencia al GameObject de la tarjeta
     * @descripcion descripcion del paquete
     * @urlImagen imagen del paquete que se inserta
     */
    IEnumerator llenarFicha(GameObject ficha, string descripcion, string urlImagen) {
        ficha.transform.GetChild(1).GetComponent<Text>().text = descripcion;
        string path = urlImagen.Split('/')[urlImagen.Split('/').Length - 1];
        if (File.Exists(Application.persistentDataPath + path)) {
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
            Texture2D texture = new Texture2D(8, 8);
            texture.LoadImage(byteArray);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            ficha.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        } else {
            WWW www = new WWW(urlImagen);
            yield return www;
            Texture2D texture = www.texture;
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + path, bytes);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            ficha.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        }
    }

    /**
     * Coroutine que llena los datos de las tarjetas que se insertan dependiendo el paquete
     * @ficha referencia al GameObject de la tarjeta
     * @urlImagen imagen del paquete que se inserta
     */
    IEnumerator llenarFicha(GameObject ficha, string urlImagen) {
        string path = urlImagen.Split('/')[urlImagen.Split('/').Length - 1];
        if (File.Exists(Application.persistentDataPath + path)) {
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
            Texture2D texture = new Texture2D(8, 8);
            texture.LoadImage(byteArray);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            ficha.GetComponent<Image>().sprite = sprite;
        } else {
            WWW www = new WWW(urlImagen);
            yield return www;
            Texture2D texture = www.texture;
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + path, bytes);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            ficha.GetComponent<Image>().sprite = sprite;
        }
    }

    /**
     * Funcion que se manda llamar al hacer click en el boton salir
     * Cierra la aplicacion de manera segura.
     */
    public void salir() {
        Application.Quit();
    }
}
